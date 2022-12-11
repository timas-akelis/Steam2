using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Steam2.Data;
using Steam2.Models;

namespace Steam2.Controllers
{
    public class AchievementsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AchievementsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Achievements
        public async Task<IActionResult> Index()
        {
              return View(await _context.Achievement.ToListAsync());
        }

        // GET: Achievements/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Achievement == null)
            {
                return NotFound();
            }

            var achievement = await _context.Achievement
                .FirstOrDefaultAsync(m => m.Id == id);
            if (achievement == null)
            {
                return NotFound();
            }

            return View(achievement);
        }

        // GET: Achievements/Create
        public IActionResult Create(string GameID)
        {
            Achievement achievement = new Achievement();
            achievement.GamesID = GameID;
            return View(achievement);
        }

        // POST: Achievements/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        public async Task<IActionResult> Create2([Bind("Id,Name,Description,GamesID,ProfileID,Icon,Achieved,Acquired,PlayerPerc")] Achievement achievement)
        {
            achievement.Id = Extension.CreateId();
            achievement.ProfileID = GetId();
            achievement.PlayerPerc = 0;

            _context.Add(achievement);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Games");
        }

        // GET: Achievements/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Achievement == null)
            {
                return NotFound();
            }

            var achievement = await _context.Achievement.FindAsync(id);
            if (achievement == null)
            {
                return NotFound();
            }
            return View(achievement);
        }

        // POST: Achievements/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Description,GamesID,ProfileID,Icon,Achieved,Acquired,PlayerPerc")] Achievement achievement)
        {
            if (id != achievement.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(achievement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AchievementExists(achievement.Id))
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
            return View(achievement);
        }

        // GET: Achievements/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Achievement == null)
            {
                return NotFound();
            }

            var achievement = await _context.Achievement
                .FirstOrDefaultAsync(m => m.Id == id);
            if (achievement == null)
            {
                return NotFound();
            }

            return View(achievement);
        }

        // POST: Achievements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Achievement == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Achievement'  is null.");
            }
            var achievement = await _context.Achievement.FindAsync(id);
            if (achievement != null)
            {
                _context.Achievement.Remove(achievement);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AchievementExists(string id)
        {
          return _context.Achievement.Any(e => e.Id == id);
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
