using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WearZone.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }
        public string? SecondaryImageUrl { get; set; }
        public string? BackImageUrl { get; set; }
        public string? SideImageUrl { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public bool IsNewArrival { get; set; }

        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }

        // New Filtering Properties
        public string? Gender { get; set; } // "Men", "Women", "Kids", "Unisex"
        
        public string? SubCategory { get; set; } // "Hoodies", "T-Shirts", "Accessories"
        
        public string? Brand { get; set; } // "Nike", "Adidas", "WearZone"
        
        public bool IsTrending { get; set; }
        
        public string? AgeGroup { get; set; } // "Adult", "Teen", "Toddler"

        // Inventory Management
        public string? SKU { get; set; } // Stock Keeping Unit
        
        public int StockQuantity { get; set; } = 0;
        
        public bool IsActive { get; set; } = true; // Can be disabled without deleting
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime? UpdatedAt { get; set; }
        
        // Additional product details
        public string? Color { get; set; }
        
        public string? Size { get; set; } // "S, M, L, XL" or "One Size"
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal? OriginalPrice { get; set; } // For showing discounts
        
        public int ViewCount { get; set; } = 0; // Track popularity
    }
}