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
    public class IndexModel : PageModel
    {
        private readonly Steam2.Data.ApplicationDbContext _context;

        public IndexModel(Steam2.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Profile> Profile { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.User != null)
            {
                Profile = await _context.User.ToListAsync();
            }
        }
    }
}
