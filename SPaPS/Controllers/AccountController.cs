using SPaPS.Models.AccountModels;
using Microsoft.AspNetCore.Mvc;
using SPaPS.Models;
using Microsoft.AspNetCore.Identity;
using SPaPS.Data;

namespace SPaPS.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly SPaPSContext _context;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, SPaPSContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            IdentityUser user = new IdentityUser()
            {
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber
            };

            var createUser = await _userManager.CreateAsync(user, model.Password);

            if (!createUser.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

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

            return View();
        }
    }
}
