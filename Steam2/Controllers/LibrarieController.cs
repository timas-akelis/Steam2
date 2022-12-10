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
    public class LibrarieController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LibrarieController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Librarie
        public async Task<IActionResult> Index()
        {
            var UserId = GetId();

            if (UserId != string.Empty)
            {
                var profile = _context.Profile
                    .FirstOrDefault(m => m.Id == UserId);

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
            }

            var libra = _context.Library.Where(x => x.ProfileID == GetId()).ToList();
            List<Game> allGames = new List<Game>();
            for (int i = 0; i < libra.Count; i++)
            {
                var game = _context.Game.Where(x => x.Id == libra[i].GamesID).FirstOrDefault();
                if (game != null) allGames.Add(game);
            }
            List<Tuple<Game, Library>> GameLibary = new List<Tuple<Game, Library>>();
            var tuples = allGames.Zip(_context.Library, (x, y) => new Tuple<Game, Library>(x, y));
            GameLibary.AddRange(tuples);

            return View(GameLibary);
        }

        // GET: Librarie/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Library == null)
            {
                return NotFound();
            }

            var library = await _context.Library
                .FirstOrDefaultAsync(m => m.Id == id);
            if (library == null)
            {
                return NotFound();
            }

            return View(library);
        }


        // POST: Librarie/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.


        //[HttpPost]
        //[ValidateAntiForgeryToken] IF you link to the Create method from somewhere else or you just want to write a custom Create, remove these 2. probably.


        public async Task<IActionResult> Create(string BoughtGames)
        {
            var boughtGamesList = System.Text.Json.JsonSerializer.Deserialize<List<Game>>(BoughtGames);

            string ProfileId = GetId();
            foreach (var game in boughtGamesList)
            {
                Library newGame = new Library();
                newGame.Id = Extension.CreateId();
                newGame.ProfileID = ProfileId;
                newGame.GamesID = game.Id;
                newGame.Date = DateTime.Now;
                newGame.HoursPlayed = 0;
                newGame.RecentHoursPlayer = 0;

                var dup = _context.Library.Where(m => m.GamesID == game.Id).Any();
                if (!dup)
                {
                    _context.Add(newGame);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    //a message to say you bought a duplicate game or something maybe
                }
            }
            //Create order
            return RedirectToAction("Create","Orders", new { BoughtGames = BoughtGames });

            //return RedirectToAction(nameof(Index));
        }

        // GET: Librarie/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Library == null)
            {
                return NotFound();
            }

            var library = await _context.Library.FindAsync(id);
            if (library == null)
            {
                return NotFound();
            }
            return View(library);
        }

        // POST: Librarie/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,GamesID,ProfileID,Date")] Library library)
        {
            if (id != library.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(library);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LibraryExists(library.Id))
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
            return View(library);
        }

        // GET: Librarie/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Library == null)
            {
                return NotFound();
            }

            var library = await _context.Library
                .FirstOrDefaultAsync(m => m.Id == id);
            if (library == null)
            {
                return NotFound();
            }

            return View(library);
        }

        // POST: Librarie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Library == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Library'  is null.");
            }
            var library = await _context.Library.FindAsync(id);
            if (library != null)
            {
                _context.Library.Remove(library);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LibraryExists(string id)
        {
          return _context.Library.Any(e => e.Id == id);
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
