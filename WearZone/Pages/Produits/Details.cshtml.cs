using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WearZone.Data;
using WearZone.Models;
using System.Threading.Tasks;

namespace WearZone.Pages.Produits
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Product Product { get; set; } = default!;
        public IList<Product> RelatedProducts { get; set; } = new List<Product>();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category) // Ensure Category is loaded
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            Product = product;

            // Fetch 4 related products (Same Category, Different ID)
            if (Product.Category != null)
            {
                RelatedProducts = await _context.Products
                    .Where(p => p.Category.Id == Product.Category.Id && p.Id != Product.Id)
                    .OrderBy(r => Guid.NewGuid()) // Random mixes it up
                    .Take(4)
                    .ToListAsync();
            }

            return Page();
        }
    }
}
