using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Steam2.Data;
using Steam2.Models;

namespace Steam2.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProfileController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Profile
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var UserId = GetId();

            if (UserId != string.Empty)
            {
                var profile = await _context.Profile
                    .FirstOrDefaultAsync(m => m.Id == UserId);

                if (profile != null)
                {
                    if (profile.Role == "Admin")
                    {
                        ViewData["Admin"] = "Yes";
                    }
                    if (profile.Role == "Creator")
                    {
                        ViewData["Creator"] = "Yes";
                    }
                }

                return View(profile);
            }

            return NotFound();
        }

        // GET: Profile/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Profile == null)
            {
                return NotFound();
            }

            var profile = await _context.Profile
                .FirstOrDefaultAsync(m => m.Id == id);
            if (profile == null)
            {
                return NotFound();
            }

            return View(profile);
        }

        // GET: Profiles/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Profiles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Name,LoginName,Context,Country,Region,Role,State,ConnecedUsers")] Profile profile)
        {
            profile.Id = GetId();

            if (profile.Id != string.Empty)
            {
                _context.Add(profile);
                if (profile.Role == "Admin")
                {
                    ViewData["Admin"] = "Yes";
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return NotFound();
        }

        // GET: Profile/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Profile == null)
            {
                return NotFound();
            }

            var profile = await _context.Profile.FindAsync(id);
            if (profile == null)
            {
                return NotFound();
            }
            return View(profile);
        }

        // POST: Profile/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,LoginName,Context,Country,Region,Role,State,ConnectedUsers")] Profile profile)
        {
            if (id != profile.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(profile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProfileExists(profile.Id))
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
            return View(profile);
        }

        // GET: Profile/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Profile == null)
            {
                return NotFound();
            }

            var profile = await _context.Profile
                .FirstOrDefaultAsync(m => m.Id == id);
            if (profile == null)
            {
                return NotFound();
            }

            return View(profile);
        }

        // POST: Profile/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Profile == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Profile'  is null.");
            }
            var profile = await _context.Profile.FindAsync(id);
            if (profile != null)
            {
                _context.Profile.Remove(profile);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProfileExists(string id)
        {
          return _context.Profile.Any(e => e.Id == id);
        }

        private string GetId()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                var userIdClaim = claimsIdentity.Claims
                    .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

                if (userIdClaim != null)
                {
                    return userIdClaim.Value;
                }
            }

            return string.Empty;
        }
    }
}
