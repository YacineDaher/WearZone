using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WearZone.Models;
using System.Collections.Generic;

namespace WearZone.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public IList<Product> NewArrivals { get; set; } = new List<Product>();

        public void OnGet()
        {
            // Mock Data for UI demonstration
            NewArrivals = new List<Product>
            {
                new Product { Id = 1, Name = "Adapt Camo Seamless Leggings", Price = 60.00m, ImageUrl = "https://images.unsplash.com/photo-1506619216599-9d930b331667?w=500&auto=format&fit=crop&q=60", IsNewArrival = true, Category = new Category { Name = "Women" } },
                new Product { Id = 2, Name = "Vital Seamless 2.0 Crop Top", Price = 35.00m, ImageUrl = "https://images.unsplash.com/photo-1581655353564-df123a1eb820?w=500&auto=format&fit=crop&q=60", IsNewArrival = true, Category = new Category { Name = "Women" } },
                new Product { Id = 3, Name = "Apex T-Shirt", Price = 40.00m, ImageUrl = "https://images.unsplash.com/photo-1618354691373-d851c5c3a990?w=500&auto=format&fit=crop&q=60", Category = new Category { Name = "Men" } },
                new Product { Id = 4, Name = "Arrival Shorts", Price = 30.00m, ImageUrl = "https://images.unsplash.com/photo-1596755094514-f87e34085b2c?w=500&auto=format&fit=crop&q=60", Category = new Category { Name = "Men" } }
            };
        }
    }
}
