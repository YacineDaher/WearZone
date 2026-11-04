using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WearZone.Data;
using WearZone.Models;
using System.Threading.Tasks;

namespace WearZone.Pages.Checkout
{
    public class ConfirmationModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ConfirmationModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public int OrderId { get; set; }

        public void OnGet(int? id)
        {
            if (id.HasValue)
            {
                OrderId = id.Value;
            }
        }

        public async Task<IActionResult> OnPostSimulateDeliveryAsync(int id)
        {
            if (!User.IsInRole("Admin"))
            {
                return Forbid(); 
            }

            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                order.Status = "Delivered";
                await _context.SaveChangesAsync();
                return new JsonResult(new { success = true });
            }
            return new JsonResult(new { success = false });
        }
    }
}