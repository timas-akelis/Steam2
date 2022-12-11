using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Steam2.Data;
using Steam2.Models;
using Steam2.Models.ViewModels;

namespace Steam2.Controllers
{
    public class GamesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GamesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Games
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
            StringBuilder buildId = new StringBuilder();
            /*
            Genre genre1 = new Genre();
            genre1.Id = Extension.CreateId();
            genre1.Name = "RPG";
            genre1.Description = "Role Playing Game";
            buildId = new StringBuilder();
            foreach (Game game in _context.Game.ToList())
            {
                buildId.Append(game.Id);
                buildId.Append(';');
            }
            buildId.Remove(buildId.Length - 1, 1);
            genre1.GamesID = buildId.ToString();
            _context.Genre.Add(genre1);

            Genre genre2 = new Genre();
            genre2.Id = Extension.CreateId();
            genre2.Name = "Puzzle";
            genre2.Description = "Thinking things";
            buildId = new StringBuilder();
            foreach (Game game in _context.Game.Where(x => x.Price > 15).ToList())
            {
                buildId.Append(game.Id);
                buildId.Append(';');
            }
            buildId.Remove(buildId.Length - 1, 1);
            genre2.GamesID = buildId.ToString();
            _context.Genre.Add(genre2);
            _context.SaveChanges();

            Genre genre3 = new Genre();
            genre3.Id = Extension.CreateId();
            genre3.Name = "Action";
            genre3.Description = "Do things fast";
            buildId = new StringBuilder();
            foreach (Game game in _context.Game.Where(x => x.Price > 450).ToList())
            {
                buildId.Append(game.Id);
                buildId.Append(';');
            }
            buildId.Remove(buildId.Length - 1, 1);
            genre3.GamesID = buildId.ToString();
            _context.Genre.Add(genre3);
            _context.SaveChanges();*/

            GamesSales VM = new GamesSales(_context.Game.ToList(), _context.Sales.ToList());
            return View(VM);
        }

        // GET: Games/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Game == null)
            {
                return NotFound();
            }

            var game = await _context.Game
                .FirstOrDefaultAsync(m => m.Id == id);
            if (game == null)
            {
                return NotFound();
            }

            //List<List<string>> ListedGenres = _context.Genre.ToList().Select(x => x.GamesID).Select(s => s.Split(';').ToList()).ToList();
            // Are you ready to witness the POWER of lambda?
            // BEHOLD
            /*List<string> ConFiltered = _context.Genre.ToList().Select(x => x.GamesID)
                                            .Select(s => s.Split(';').ToList())
                                            .Where(lst => lst.Contains(id))
                                            .Select(lst => string.Join(";", lst))
                                            .ToList();
            
            good thing it doesnt work
            well, it could work, but i already dont get it
            thanks chatgpt
             */

            List<string> AppliedGenres = new List<string>();
            foreach (Genre genre in _context.Genre.ToList())
            {
                List<string> ids = genre.GamesID.Split(';').ToList();
                foreach (string gameId in ids)
                {
                    if (gameId == id)
                    {
                        AppliedGenres.Add(genre.Name);
                        break;
                    }
                }
            }

            GameGenreAchievementComment VM = new GameGenreAchievementComment(game, AppliedGenres, new Achievement(), new Comment());

            return View(VM);
        }

        public async Task<IActionResult> AddSaleId(string SaleId, string GameId)
        {
            //_context.Update()
            Game saleGame = _context.Game.Where(x => x.Id == GameId).FirstOrDefault();
            if (saleGame != null)
            {
                saleGame.SaleId = SaleId;
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RemoveSaleId(string SaleId, string GameId)
        {
            //_context.Update()
            Game saleGame = _context.Game.Where(x => x.Id == GameId).FirstOrDefault();
            if (saleGame != null)
            {
                saleGame.SaleId = "";
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> AddToLibrary(string Id)
        {
            //Library newGame = new Library();
            //newGame.Id = CreateId();
            //newGame.ProfileID = GetId();
            //newGame.GamesID = Id;
            //newGame.Date = DateTime.Now;
            //newGame.HoursPlayed = 0;
            //newGame.RecentHoursPlayer = 0;
            //string ProfileId = GetId();
            //return RedirectToAction("Create", "Librarie", new { newGame = newGame});

            //return RedirectToAction("Create", "Librarie", new { GameId = Id });

            return RedirectToAction("Create", "Cart", new { GameId = Id });
        }

        // GET: Games/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Games/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Title,PublishingDate,Price,Rating,Description,Developer,Publisher,SaleId")] Game game)
        {
            game.Id = Extension.CreateId();
            game.SaleId = "None";
            game.PublishingDate = DateTime.Now;

            _context.Add(game);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index)); // ??
            
        }

        // GET: Games/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Game == null)
            {
                return NotFound();
            }

            var game = await _context.Game.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            return View(game);
        }

        // POST: Games/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Title,PublishingDate,Price,Rating,Description,Developer,Publisher,SaleId")] Game game)
        {
            if (id != game.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(game);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameExists(game.Id))
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
            return View(game);
        }

        // GET: Games/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Game == null)
            {
                return NotFound();
            }

            var game = await _context.Game
                .FirstOrDefaultAsync(m => m.Id == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // POST: Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Game == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Game'  is null.");
            }
            var game = await _context.Game.FindAsync(id);
            if (game != null)
            {
                _context.Game.Remove(game);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Games/AddWishlist/5
        [Authorize]
        public async Task<IActionResult> AddWishlist(string id)
        {
            //if (id == null || _context.Game == null)
            //{
            //    return NotFound();
            //}

            //var game = await _context.Game.FindAsync(id);
            //if (game == null)
            //{
            //    return NotFound();
            //}
            //return NotFound();
            string ProfileId = GetId();
            return RedirectToAction("Create", "Wishlists", new { GameId = id });
        }

        private bool GameExists(string id)
        {
          return _context.Game.Any(e => e.Id == id);
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
