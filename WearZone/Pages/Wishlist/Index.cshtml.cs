using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WearZone.Data;
using WearZone.Models;

namespace WearZone.Pages.Wishlist
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<WishlistItem> WishlistItems { get; set; }

        public async Task OnGetAsync()
        {
            var userId = User.Identity.Name;
            
            WishlistItems = await _context.WishlistItems
                .Include(w => w.Product)
                .Where(w => w.UserId == userId)
                .OrderByDescending(w => w.DateAdded)
                .ToListAsync();
        }
    }
}
