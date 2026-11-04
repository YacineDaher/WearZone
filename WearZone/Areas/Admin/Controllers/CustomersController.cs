using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WearZone.Data;
using WearZone.Models;

namespace WearZone.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CustomersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Admin/Customers
        public async Task<IActionResult> Index(string searchTerm)
        {
            var users = _userManager.Users.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                users = users.Where(u => 
                    u.Email.Contains(searchTerm) || 
                    u.FirstName.Contains(searchTerm) ||
                    u.LastName.Contains(searchTerm));
            }

            var customerList = await users
                .OrderByDescending(u => u.RegisteredAt)
                .ToListAsync();

            // Get order counts for each customer
            var customersWithStats = new List<CustomerViewModel>();
            foreach (var user in customerList)
            {
                var orderCount = await _context.Orders.CountAsync(o => o.UserId == user.Id);
                var totalSpent = await _context.Orders
                    .Where(o => o.UserId == user.Id)
                    .SumAsync(o => (decimal?)o.TotalAmount) ?? 0;

                customersWithStats.Add(new CustomerViewModel
                {
                    User = user,
                    OrderCount = orderCount,
                    TotalSpent = totalSpent
                });
            }

            return View(customersWithStats);
        }

        // GET: Admin/Customers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var orders = await _context.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .Where(o => o.UserId == id)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            var wishlistItems = await _context.WishlistItems
                .Include(w => w.Product)
                .Where(w => w.UserId == id)
                .ToListAsync();

            var viewModel = new CustomerDetailsViewModel
            {
                User = user,
                Orders = orders,
                WishlistItems = wishlistItems,
                TotalSpent = orders.Sum(o => o.TotalAmount),
                OrderCount = orders.Count
            };

            return View(viewModel);
        }
    }

    public class CustomerViewModel
    {
        public ApplicationUser User { get; set; } = null!;
        public int OrderCount { get; set; }
        public decimal TotalSpent { get; set; }
    }

    public class CustomerDetailsViewModel
    {
        public ApplicationUser User { get; set; } = null!;
        public List<Order> Orders { get; set; } = new();
        public List<WishlistItem> WishlistItems { get; set; } = new();
        public decimal TotalSpent { get; set; }
        public int OrderCount { get; set; }
    }
}
