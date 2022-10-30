using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Steam2.Data;
using Steam2.Models;

namespace Steam2.Views.Shared
{
    public class DeleteModel : PageModel
    {
        private readonly Steam2.Data.ApplicationDbContext _context;

        public DeleteModel(Steam2.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public Profile Profile { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var profile = await _context.User.FirstOrDefaultAsync(m => m.Id == id);

            if (profile == null)
            {
                return NotFound();
            }
            else 
            {
                Profile = profile;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }
            var profile = await _context.User.FindAsync(id);

            if (profile != null)
            {
                Profile = profile;
                _context.User.Remove(Profile);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
