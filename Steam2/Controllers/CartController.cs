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
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        




        // GET: Cart
        public async Task<IActionResult> Index()
        {
            var carta = _context.Cart.Where(x => x.ProfileID == GetId()).ToList();
            List<Game> allGames = new List<Game>();
            for (int i = 0; i < carta.Count; i++)
            {
                var games = _context.Game.Where(x => x.Id == carta[i].GamesID).ToList();
                for (int j = 0; j < games.Count; j++)
                {
                    allGames.Add(games[j]);
                }
            }
            List<Tuple<Game, Cart>> GameLibary = new List<Tuple<Game, Cart>>();
            var tuples = allGames.Zip(_context.Cart, (x, y) => new Tuple<Game, Cart>(x, y));
            GameLibary.AddRange(tuples);
            return View(GameLibary);
        }

        [Authorize]
        public async Task<IActionResult> Transaction()
        {
            var carta = _context.Cart.Where(x => x.ProfileID == GetId()).ToList();
            List<Game> cartGames = new List<Game>();
            decimal totalPrice = 0;
            for (int i = 0; i < carta.Count; i++)
            {
                Game cartGame = _context.Game.Where(x => x.Id == carta[i].GamesID).FirstOrDefault();
                if (cartGame != null) cartGames.Add(cartGame);
                totalPrice += GetPriceWithSale(cartGame.Id);
            }
            ViewBag.Message = totalPrice.ToString();
            return View(cartGames);
        }

        public async Task<IActionResult> ConfirmTransaction()
        {
            /* WHAT THE FUCK IS THIS
            var carta = _context.Cart.ToList();
            List<Game> allGames = new List<Game>();
            for (int i = 0; i < carta.Count; i++)
            {
                var games = _context.Game.Where(x => x.Id == carta[i].GamesID).ToList();
                for (int j = 0; j < games.Count; j++)
                {
                    allGames.Add(games[j]);
                }
            }
            List<Tuple<Game, Cart>> BoughtGames = new List<Tuple<Game, Cart>>();
            var tuples = allGames.Zip(_context.Cart, (x, y) => new Tuple<Game, Cart>(x, y));
            BoughtGames.AddRange(tuples);
            string jsonGames = System.Text.Json.JsonSerializer.Serialize(BoughtGames);*/
            var carta = _context.Cart.Where(x => x.ProfileID == GetId()).ToList();
            List<Game> cartGames = new List<Game>();
            for (int i = 0; i < carta.Count; i++)
            {
                Game cartGame = _context.Game.Where(x => x.Id == carta[i].GamesID).FirstOrDefault();
                if (cartGame != null) cartGames.Add(cartGame);
            }

            string jsonGames = System.Text.Json.JsonSerializer.Serialize<List<Game>>(cartGames);
            var boughtGamesList = System.Text.Json.JsonSerializer.Deserialize<List<Game>>(jsonGames);

            // create order here probably

            _context.Cart.RemoveRange(_context.Cart.Where(x => x.ProfileID == GetId()).ToList());
            await _context.SaveChangesAsync();

            return RedirectToAction("Create", "Librarie", new { BoughtGames = jsonGames });
        }



        // GET: Cart/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Cart == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        public async Task<IActionResult> Create(string GameId)
        {
            Cart newGame = new Cart();
            newGame.Id = Extension.CreateId();
            newGame.ProfileID = GetId();
            newGame.GamesID = GameId;
            newGame.Date = DateTime.Now;

            var dup = _context.Cart.Where(p => p.ProfileID == GetId()).Where(m => m.GamesID == GameId).Any();
            if (!dup)
            {
                _context.Add(newGame);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        //--------------------------------Extra functions-----------------------------


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

        private decimal GetPriceWithSale(string GameId)
        {
            Game game = _context.Game.Where(m => m.Id == GameId).FirstOrDefault();
            if (game == null) return 0;

            Sales sale = _context.Sales.Where(s => s.Id == game.SaleId).FirstOrDefault();
            if (sale == null) return game.Price;

            return game.Price - (game.Price * sale.Amount);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Cart == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cart != null)
            {
                _context.Cart.Remove(cart);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /* Probably not needed
         * 
        // GET: Cart/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Cart == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }
            return View(cart);
        }

        // POST: Cart/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,GamesID,ProfileID,Date")] Cart cart)
        {
            if (id != cart.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartExists(cart.Id))
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
            return View(cart);
        }

        // GET: Cart/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Cart == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // POST: Cart/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Cart == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Cart'  is null.");
            }
            var cart = await _context.Cart.FindAsync(id);
            if (cart != null)
            {
                _context.Cart.Remove(cart);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartExists(string id)
        {
          return _context.Cart.Any(e => e.Id == id);
        }*/


    }
}
