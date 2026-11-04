using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WearZone.Data;
using WearZone.Models;

namespace WearZone.Pages.Produits
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Product> Products { get; set; } = default!;

        // Filter Properties
        [BindProperty(SupportsGet = true)]
        public string? Gender { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Category { get; set; } // Using Name or Id? Assuming Name filter for URL

        [BindProperty(SupportsGet = true)]
        public string? SubCategory { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Brand { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public bool? New { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool? Trending { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public string? Sort { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public const int PageSize = 12;

        public async Task OnGetAsync()
        {
            var query = _context.Products
                .Include(p => p.Category)
                .AsQueryable();

            // Apply Filters
            if (!string.IsNullOrEmpty(Gender))
            {
                query = query.Where(p => p.Gender == Gender);
            }

            if (!string.IsNullOrEmpty(Category))
            {
                query = query.Where(p => p.Category != null && p.Category.Name == Category);
            }
            
            if (!string.IsNullOrEmpty(SubCategory))
            {
                query = query.Where(p => p.SubCategory == SubCategory);
            }

            if (!string.IsNullOrEmpty(Brand))
            {
                query = query.Where(p => p.Brand == Brand);
            }

            if (New == true)
            {
                query = query.Where(p => p.IsNewArrival);
            }
            
            if (Trending == true)
            {
                query = query.Where(p => p.IsTrending);
            }

            if (!string.IsNullOrEmpty(SearchString))
            {
                var searchTerms = SearchString.Trim().ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                
                foreach (var term in searchTerms)
                {
                    query = query.Where(p => 
                        p.Name.ToLower().Contains(term) || 
                        p.Description.ToLower().Contains(term) ||
                        p.Brand.ToLower().Contains(term) ||
                        p.SubCategory.ToLower().Contains(term) ||
                        p.Color.ToLower().Contains(term) ||
                        (p.Category != null && p.Category.Name.ToLower().Contains(term))
                    );
                }
            }

            // Apply Sorting
            switch (Sort)
            {
                case "price_asc":
                    query = query.OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    query = query.OrderByDescending(p => p.Price);
                    break;
                case "newest":
                default:
                     query = query.OrderByDescending(p => p.Id);
                     break;
            }

            // Pagination Logic
            var totalItems = await query.CountAsync();
            TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);
            
            // Ensure valid page
            if (CurrentPage < 1) CurrentPage = 1;
            
            Products = await query
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }
    }
}
