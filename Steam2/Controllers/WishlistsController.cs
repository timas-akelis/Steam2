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
    public class WishlistsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WishlistsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Wishlists
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
                }
            }
            var wish = _context.Wishlist.Where(x => x.ProfileID == GetId()).ToList();
            List<Game> allGames = new List<Game>();
            for(int i = 0; i < wish.Count; i++)
            {
                var game = _context.Game.Where(x => x.Id == wish[i].GamesID).FirstOrDefault();
                if (game != null) allGames.Add(game);
            }
            return View(allGames);
        }

        // GET: Wishlists/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Wishlist == null)
            {
                return NotFound();
            }

            var wishlist = await _context.Wishlist
                .FirstOrDefaultAsync(m => m.Id == id);
            if (wishlist == null)
            {
                return NotFound();
            }

            return View(wishlist);
        }

        //// GET: Wishlists/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        // POST: Wishlists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        public async Task<IActionResult> Create(string GameId)
        {
            Wishlist wishlist = new Wishlist();
            wishlist.Id = Extension.CreateId();
            wishlist.ProfileID = GetId();
            wishlist.GamesID = GameId;
            wishlist.Date = DateTime.Now;

            var dup = _context.Wishlist.Where(m => m.GamesID == GameId).Any();
            if (!dup)
            {
                _context.Add(wishlist);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Wishlists/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Wishlist == null)
            {
                return NotFound();
            }

            var wishlist = await _context.Wishlist.FindAsync(id);
            if (wishlist == null)
            {
                return NotFound();
            }
            return View(wishlist);
        }

        // POST: Wishlists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,GamesID,ProfileID,Date")] Wishlist wishlist)
        {
            if (id != wishlist.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(wishlist);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WishlistExists(wishlist.Id))
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
            return RedirectToAction(nameof(Index));
        }

        // GET: Wishlists/Delete/5
        public async Task<IActionResult> Delete(string Id)
        {
            if (Id == null || _context.Wishlist == null)
            {
                return NotFound();
            }

            var wishlist = await _context.Wishlist
                .FirstOrDefaultAsync(m => m.GamesID == Id);
            if (wishlist == null)
            {
                return NotFound();
            }

            _context.Wishlist.Remove(wishlist);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // POST: Wishlists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Wishlist == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Wishlist'  is null.");
            }
            var wishlist = await _context.Wishlist.FindAsync(id);
            if (wishlist != null)
            {
                _context.Wishlist.Remove(wishlist);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WishlistExists(string id)
        {
          return _context.Wishlist.Any(e => e.Id == id);
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
