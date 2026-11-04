using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WearZone.Data;
using WearZone.Models;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

namespace WearZone.Pages.Checkout
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Models.Panier Cart { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            string userId;
            if (User.Identity.IsAuthenticated) userId = User.Identity.Name;
            else userId = Request.Cookies["CartSessionId"];

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToPage("/Panier/Index");
            }

            Cart = await _context.Paniers
                .Include(p => p.Elements)
                .ThenInclude(e => e.Product)
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (Cart == null || !Cart.Elements.Any())
            {
                return RedirectToPage("/Panier/Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            string userId;
            if (User.Identity.IsAuthenticated) userId = User.Identity.Name;
            else userId = Request.Cookies["CartSessionId"];

            if (!string.IsNullOrEmpty(userId))
            {
               var cart = await _context.Paniers
                   .Include(p => p.Elements)
                   .ThenInclude(e => e.Product)
                   .FirstOrDefaultAsync(p => p.UserId == userId);
                   
               if(cart != null && cart.Elements.Any())
               {
                   // Determine the UserId for the Order
                   string orderUserId = "Guest";
                   if (User.Identity.IsAuthenticated)
                   {
                       var user = await _userManager.GetUserAsync(User);
                       orderUserId = user?.Id ?? User.Identity.Name;
                   }

                   // Create Order
                   var order = new Order
                   {
                       UserId = orderUserId,
                       TotalAmount = cart.Elements.Sum(e => e.Product.Price * e.Quantite),
                       Status = "Pending",
                       OrderDate = DateTime.Now
                       // Add Shipping Info from Form here (would need BindProperty)
                   };
                   
                   _context.Orders.Add(order);
                   await _context.SaveChangesAsync(); // Get Order Id

                   // Create Order Items
                   foreach(var element in cart.Elements)
                   {
                       var orderItem = new OrderItem
                       {
                           OrderId = order.Id,
                           ProductId = element.ProductId,
                           Quantity = element.Quantite,
                           Price = element.Product.Price
                       };
                       _context.OrderItems.Add(orderItem);
                   }

                   // Clear Cart
                   _context.ElementsPanier.RemoveRange(cart.Elements);
                   
                   await _context.SaveChangesAsync();
                   
                   return RedirectToPage("/Checkout/Confirmation", new { id = order.Id });
               }
            }

            return RedirectToPage("/Panier/Index"); // Redirect back to cart if something failed
        }
    }
}
