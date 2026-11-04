using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WearZone.Data;
using WearZone.Models;

namespace WearZone.Pages.Account.Orders
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;

        public IndexModel(ApplicationDbContext context, Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Order> Orders { get; set; }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user?.Id;

            // Fallback for old orders that might have been saved with email
            // This OR condition allows seeing BOTH new (GUID) and potentially old (Email) orders if necessary, 
            // or we just stick to strict ID. 
            // Given the previous fix, let's look for the ID.
            
            if (userId != null)
            {
                Orders = await _context.Orders
                    .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                    .Where(o => o.UserId == userId || o.UserId == User.Identity.Name) // Handle both for backward compatibility during transition
                    .OrderByDescending(o => o.OrderDate)
                    .ToListAsync();
            }
            else
            {
                Orders = new List<Order>();
            }
        }
    }
}
