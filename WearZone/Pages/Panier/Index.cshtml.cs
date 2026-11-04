using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WearZone.Data;
using WearZone.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace WearZone.Pages.Panier
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Models.Panier Cart { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            string userId;
            if (User.Identity.IsAuthenticated)
            {
                userId = User.Identity.Name;
            }
            else
            {
                userId = Request.Cookies["CartSessionId"];
            }

            if (string.IsNullOrEmpty(userId))
            {
                // No session, cart is effectively empty
                Cart = new Models.Panier(); 
            }
            else
            {
                Cart = await _context.Paniers
                    .Include(p => p.Elements)
                    .ThenInclude(e => e.Product)
                    .FirstOrDefaultAsync(p => p.UserId == userId);

                if (Cart == null)
                {
                    Cart = new Models.Panier();
                }
            }

            return Page();
        }
    }
}
