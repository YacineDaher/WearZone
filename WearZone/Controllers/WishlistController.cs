using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WearZone.Data;
using WearZone.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace WearZone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Only logged-in users can have a wishlist
    public class WishlistController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public WishlistController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("toggle")]
        public async Task<IActionResult> ToggleWishlist([FromBody] int productId)
        {
            var userId = User.Identity.Name; // Using username/email as ID
            
            var existingItem = await _context.WishlistItems
                .FirstOrDefaultAsync(w => w.UserId == userId && w.ProductId == productId);

            bool isAdded;

            if (existingItem != null)
            {
                _context.WishlistItems.Remove(existingItem);
                isAdded = false;
            }
            else
            {
                var item = new WishlistItem
                {
                    UserId = userId,
                    ProductId = productId,
                    DateAdded = DateTime.Now
                };
                _context.WishlistItems.Add(item);
                isAdded = true;
            }

            await _context.SaveChangesAsync();

            return Ok(new { added = isAdded });
        }
        
        [HttpGet("check")]
        public async Task<IActionResult> CheckWishlist(int productId)
        {
            var userId = User.Identity.Name;
            var exists = await _context.WishlistItems.AnyAsync(w => w.UserId == userId && w.ProductId == productId);
            return Ok(new { inWishlist = exists });
        }
    }
}
