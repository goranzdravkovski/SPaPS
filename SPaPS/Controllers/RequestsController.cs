using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SPaPS.Data;
using SPaPS.Enums;
using SPaPS.Models;

namespace SPaPS.Controllers
{
    public class RequestsController : Controller
    {
        private readonly SPaPSContext _context;

        public RequestsController(SPaPSContext context)
        {
            _context = context;
        }

        // GET: Requests
        public async Task<IActionResult> Index()
        {
            var Client = await _context.Clients.Where(x => x.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier)).FirstOrDefaultAsync();
            var ClientServices = _context.ClientServices.Where(x => x.ClientId == Client.ClientId).Select(x => x.ServiceId).ToList();
            List<Request> sPaPSContext;
            if (User.IsInRole(Roles.Company.ToString()))
            {
                 sPaPSContext = await _context.Requests.Where(x => ClientServices.Contains(x.ServiceId)).Include(r => r.Activity).Include(r => r.Client).Include(r => r.Service).ToListAsync();
            }
            else
            {
                 sPaPSContext = await _context.Requests.Where(x => x.ClientId == Client.ClientId).Include(r => r.Activity).Include(r => r.Client).Include(r => r.Service).ToListAsync();
            }

            return View(sPaPSContext);
        }

        // GET: Requests/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.Requests == null)
            {
                return NotFound();
            }

            var request = await _context.Requests
                .Include(r => r.Activity)
                .Include(r => r.Client)
                .Include(r => r.Service)
                .FirstOrDefaultAsync(m => m.RequestId == id);
            if (request == null)
            {
                return NotFound();
            }

            return View(request);
        }

        // GET: Requests/Create
        public IActionResult Create()
        {
            ViewData["ActivityId"] = new SelectList(_context.Activities, "ActivityId", "Description");
            ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceId", "Description");
            ViewBag.BiuldingTypes = new SelectList(_context.References.Where(x => x.ReferenceTypeId == 4).ToList(), "ReferenceId", "Description");

            return View();
        }

        // POST: Requests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Request request)
        {
            if (ModelState.IsValid)
            {
                var Client = await _context.Clients.Where(x => x.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier)).FirstOrDefaultAsync();
                request.ClientId = Client.ClientId;
                _context.Add(request);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ActivityId"] = new SelectList(_context.Activities, "ActivityId", "ActivityId", request.ActivityId);
            ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceId", "ServiceId", request.ServiceId);
            return View(request);
        }

        // GET: Requests/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.Requests == null)
            {
                return NotFound();
            }

            var request = await _context.Requests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }
            ViewData["ActivityId"] = new SelectList(_context.Activities, "ActivityId", "ActivityId", request.ActivityId);
            ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceId", "ServiceId", request.ServiceId);
            return View(request);
        }

        // POST: Requests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id,  Request request)
        {
            if (id != request.RequestId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(request);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RequestExists(request.RequestId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ActivityId"] = new SelectList(_context.Activities, "ActivityId", "ActivityId", request.ActivityId);
            ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceId", "ServiceId", request.ServiceId);
            return View(request);
        }

        // GET: Requests/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.Requests == null)
            {
                return NotFound();
            }

            var request = await _context.Requests
                .Include(r => r.Activity)
                .Include(r => r.Client)
                .Include(r => r.Service)
                .FirstOrDefaultAsync(m => m.RequestId == id);
            if (request == null)
            {
                return NotFound();
            }

            return View(request);
        }

        // POST: Requests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.Requests == null)
            {
                return Problem("Entity set 'SPaPSContext.Requests'  is null.");
            }
            var request = await _context.Requests.FindAsync(id);
            if (request != null)
            {
                _context.Requests.Remove(request);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RequestExists(long id)
        {
          return (_context.Requests?.Any(e => e.RequestId == id)).GetValueOrDefault();
        }
    }
}
