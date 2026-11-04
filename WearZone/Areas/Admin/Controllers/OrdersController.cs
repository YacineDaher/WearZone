using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WearZone.Data;
using WearZone.Models;

namespace WearZone.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Orders
        public async Task<IActionResult> Index(string status, string searchTerm)
        {
            var query = _context.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .AsQueryable();

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(o => o.Status == status);
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(o => 
                    o.Id.ToString().Contains(searchTerm) || 
                    o.FullName.Contains(searchTerm) ||
                    o.UserId.Contains(searchTerm));
            }

            var orders = await query.OrderByDescending(o => o.OrderDate).ToListAsync();
            
            return View(orders);
        }

        // GET: Admin/Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
                
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        private static readonly string[] AllowedStatuses =
            { "Pending", "Processing", "Shipped", "Delivered", "Cancelled" };

        // POST: Admin/Orders/UpdateStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            if (string.IsNullOrEmpty(status) || !AllowedStatuses.Contains(status))
            {
                return Json(new { success = false });
            }

            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                order.Status = status;
                await _context.SaveChangesAsync();

                TempData["Success"] = $"Order #{id} status updated to {status}";
                return Json(new { success = true, status = status });
            }

            return Json(new { success = false });
        }

        // POST: Admin/Orders/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == id);
                
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Order deleted successfully!";
            }
            
            return RedirectToAction(nameof(Index));
        }
    }
}
