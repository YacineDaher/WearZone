using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WearZone.Data;
using WearZone.Models;

namespace WearZone.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var stats = new DashboardStats
            {
                TotalProducts = await _context.Products.CountAsync(),
                ActiveProducts = await _context.Products.CountAsync(p => p.IsActive),
                TotalOrders = await _context.Orders.CountAsync(),
                PendingOrders = await _context.Orders.CountAsync(o => o.Status == "Pending"),
                TotalRevenue = await _context.Orders.SumAsync(o => (decimal?)o.TotalAmount) ?? 0,
                TotalCustomers = await _context.Users.CountAsync(),
                LowStockProducts = await _context.Products.CountAsync(p => p.StockQuantity < 10 && p.StockQuantity > 0),
                OutOfStockProducts = await _context.Products.CountAsync(p => p.StockQuantity == 0)
            };

            // Recent orders
            stats.RecentOrders = await _context.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .OrderByDescending(o => o.OrderDate)
                .Take(10)
                .ToListAsync();

            // Top selling products
            stats.TopProducts = await _context.OrderItems
                .GroupBy(oi => oi.ProductId)
                .Select(g => new TopProduct
                {
                    Product = _context.Products.First(p => p.Id == g.Key),
                    TotalSold = g.Sum(x => x.Quantity),
                    Revenue = g.Sum(x => x.Price * x.Quantity)
                })
                .OrderByDescending(tp => tp.TotalSold)
                .Take(5)
                .ToListAsync();

            return View(stats);
        }
    }

    public class DashboardStats
    {
        public int TotalProducts { get; set; }
        public int ActiveProducts { get; set; }
        public int TotalOrders { get; set; }
        public int PendingOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalCustomers { get; set; }
        public int LowStockProducts { get; set; }
        public int OutOfStockProducts { get; set; }
        public List<Order> RecentOrders { get; set; } = new();
        public List<TopProduct> TopProducts { get; set; } = new();
    }

    public class TopProduct
    {
        public Product Product { get; set; } = null!;
        public int TotalSold { get; set; }
        public decimal Revenue { get; set; }
    }
}
