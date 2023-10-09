using SPaPS.Models.AccountModels;
using Microsoft.AspNetCore.Mvc;
using SPaPS.Models;
using Microsoft.AspNetCore.Identity;
using SPaPS.Data;
using DataAccess.Services;
using SPaPS.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SPaPS.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SPaPSContext _context;
        private readonly IEmailSenderEnhance _emailService;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager, SPaPSContext context, IEmailSenderEnhance emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
            _emailService = emailService;
        }

        [HttpGet]
        public IActionResult Login()
        {

            if (TempData["Success"] != null)
                ModelState.AddModelError("Success", Convert.ToString(TempData["Success"]));


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var result  = await _signInManager.PasswordSignInAsync(userName: model.Email, password: model.Password, isPersistent: false, lockoutOnFailure: true);

            if (!result.Succeeded || result.IsLockedOut || result.IsNotAllowed)
            {
                ModelState.AddModelError("Error", "Погрешно корисничко име или лозинка!");
                return View(model);
            }

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            ViewBag.ClientTypes = new SelectList(_context.References.Where(x => x.ReferenceTypeId == 1).ToList(), "ReferenceId", "Description");
            ViewBag.Cities = new SelectList(_context.References.Where(x => x.ReferenceTypeId == 2).ToList(), "ReferenceId", "Description");
            ViewBag.Countries = new SelectList(_context.References.Where(x => x.ReferenceTypeId == 3).ToList(), "ReferenceId", "Description");
            ViewBag.Roles = new SelectList(_roleManager.Roles.ToList(), "Name", "Name");
            ViewBag.Services = new SelectList(_context.Services.ToList(), "ServiceId", "Description");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            var userExists = await _userManager.FindByEmailAsync(model.Email);

            if (userExists != null)
            {
                ModelState.AddModelError("Error", "Корисникот веќе постои!");
                return View(model);
            }

            IdentityUser user = new IdentityUser()
            {
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            var newPassword = Shared.GeneratePassword(8);

            var createUser = await _userManager.CreateAsync(user, newPassword);

            if (!createUser.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            await _userManager.AddToRoleAsync(user, model.Role);

            Client client = new Client()
            {
                UserId = user.Id,
                Name = model.Name,
                Address = model.Address,
                IdNo = model.IdNo,
                ClientTypeId = model.ClientTypeId,
                CityId = model.CityId,
                CountryId = model.CountryId
            };

            await _context.Clients.AddAsync(client);
            await _context.SaveChangesAsync();


            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var callback = Url.Action(action: "ResetPassword", controller: "Account", values: new { token, email = user.Email}, HttpContext.Request.Scheme);

            /* https://localhost:5001/Account/ResetPassword?token=123asdrew123&email=nikola.stankovski@foxit.mk */

            EmailSetUp emailSetUp = new EmailSetUp()
            {
                To = user.Email,
                Template = "Register",
                Username = user.Email,
                Callback = callback,
                Token = token,
                RequestPath = _emailService.PostalRequest(Request),                 
            };

            await _emailService.SendEmailAsync(emailSetUp);

            TempData["Success"] = "Успешно креиран корисник!";

            return RedirectToAction(nameof(Login));
        }



        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }


        [HttpPost]

        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var callback = Url.Action(action: "ResetPassword", controller: "Account", values: new { token, email = user.Email }, HttpContext.Request.Scheme);

            EmailSetUp emailSetUp = new EmailSetUp()
            {
                To = user.Email,
                Template = "ResetPassword",
                Username = user.Email,
                Callback = callback,
                Token = token,
                RequestPath = _emailService.PostalRequest(Request),
            };

            await _emailService.SendEmailAsync(emailSetUp);

            TempData["Success"] = "Ве молиме проверете го вашето влезно сандаче!";

            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {

            ResetPasswordModel model = new ResetPasswordModel()
            {
                Email = email,
                Token = token
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {

            var user = await _userManager.FindByEmailAsync(model.Email);

            var resetPassword = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);

            if (!resetPassword.Succeeded)
            {
                ModelState.AddModelError("Error", "Се случи грешка. Обидете се повторно!");
                return View();
            }

            TempData["Success"] = "Успешно промената лозинка!";

            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            var loggedInUserEmail = User.Identity.Name;

            var user = await _userManager.FindByEmailAsync(loggedInUserEmail);

            var changePassword = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

            if (!changePassword.Succeeded)
            {
                ModelState.AddModelError("Error", "Се случи грешка. Обидете се повторно!");
                return View();
            }

            ModelState.AddModelError("Success", "Успешно променета лозинка!");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ChangeUserInfo()
        {
            var loggedInUserEmail = User.Identity.Name;

            var applicationUser = await _userManager.FindByEmailAsync(loggedInUserEmail);
            var clientUser = await _context.Clients.Where(x => x.UserId == applicationUser.Id).FirstOrDefaultAsync();

            ViewBag.ClientTypes = new SelectList(_context.References.Where(x => x.ReferenceTypeId == 1).ToList(), "ReferenceId", "Description");
            ViewBag.Cities = new SelectList(_context.References.Where(x => x.ReferenceTypeId == 2).ToList(), "ReferenceId", "Description");
            ViewBag.Countries = new SelectList(_context.References.Where(x => x.ReferenceTypeId == 3).ToList(), "ReferenceId", "Description");

            ChangeUserInfo model = new ChangeUserInfo()
            {
                Name = clientUser.Name,
                Address = clientUser.Address,
                CityId = clientUser.CityId,
                CountryId = clientUser.CountryId,
                ClientTypeId = clientUser.ClientTypeId,
                IdNo = clientUser.IdNo,
                PhoneNumber = applicationUser.PhoneNumber
            };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangeUserInfo(ChangeUserInfo model)
        {
            ViewBag.ClientTypes = new SelectList(_context.References.Where(x => x.ReferenceTypeId == 1).ToList(), "ReferenceId", "Description");
            ViewBag.Cities = new SelectList(_context.References.Where(x => x.ReferenceTypeId == 2).ToList(), "ReferenceId", "Description");
            ViewBag.Countries = new SelectList(_context.References.Where(x => x.ReferenceTypeId == 3).ToList(), "ReferenceId", "Description");


            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Error", "Се случи грешка. Обидете се повторно.");

                return View(model);
            }

            var loggedInUserEmail = User.Identity.Name;

            var applicationUser = await _userManager.FindByEmailAsync(loggedInUserEmail);
            var clientUser = await _context.Clients.Where(x => x.UserId == applicationUser.Id).FirstOrDefaultAsync();

            applicationUser.PhoneNumber = model.PhoneNumber;

            var appUserResult = await _userManager.UpdateAsync(applicationUser);

            if (!appUserResult.Succeeded)
            {
                ModelState.AddModelError("Error", "Се случи грешка. Обидете се повторно.");

                return View(model);
            }

            clientUser.Address = model.Address;
            clientUser.Name = model.Name;
            clientUser.CityId = model.CityId;
            clientUser.CountryId = model.CountryId;
            clientUser.ClientTypeId = model.ClientTypeId;
            clientUser.IdNo = model.IdNo;
            clientUser.UpdatedOn = DateTime.Now;

            try
            {
                _context.Clients.Update(clientUser);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                ModelState.AddModelError("Error", "Се случи грешка. Обидете се повторно.");

                return View(model);
            }


            ModelState.AddModelError("Success", "Успешно променети информации");

            return View(model);
        }

    }
}
