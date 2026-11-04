using System;
using System.Linq;
using System.Collections.Generic;
using WearZone.Data;
using WearZone.Models;

namespace WearZone.Services
{
    public static class DataSeeder
    {
        public static void Initialize(ApplicationDbContext context)
        {
            // 1. Ensure Categories Exist
            var clothingCategory = context.Categories.FirstOrDefault(c => c.Name == "Clothing");
            if (clothingCategory == null)
            {
                clothingCategory = new Category { Name = "Clothing" };
                context.Categories.Add(clothingCategory);
                context.SaveChanges();
            }

            // 2. Add Products
            var products = new List<Product>
            {
                // WOMEN - DRESSES
                new Product
                {
                    Name = "Floral Summer Maxi Dress",
                    Description = "A beautiful, airy floral maxi dress perfect for summer days. Features breathable fabric and an elegant cut.",
                    Price = 89.99m,
                    Gender = "Women",
                    Category = clothingCategory,
                    SubCategory = "Dresses", // Using SubCategory as requested
                    ImageUrl = "https://images.unsplash.com/photo-1572804013309-59a88b7e92f1?auto=format&fit=crop&w=800&q=80",
                    StockQuantity = 50,
                    IsNewArrival = true,
                    SKU = "WZ-WM-DR-001"
                },
                new Product
                {
                    Name = "Elegant Evening Silk Gown",
                    Description = "Stun the crowd with this luxurious silk evening gown. Perfect for formal events.",
                    Price = 199.99m,
                    Gender = "Women",
                    Category = clothingCategory,
                    SubCategory = "Dresses",
                    ImageUrl = "https://images.unsplash.com/photo-1566174053879-31528523f8ae?auto=format&fit=crop&w=800&q=80",
                    StockQuantity = 25,
                    SKU = "WZ-WM-DR-002"
                },
                 new Product
                {
                    Name = "Chic Cocktail Mini Dress",
                    Description = "A stylish black compact dress for parties and night outs.",
                    Price = 120.00m,
                    Gender = "Women",
                    Category = clothingCategory,
                    SubCategory = "Dresses",
                    ImageUrl = "https://images.unsplash.com/photo-1595777457583-95e059d581b8?auto=format&fit=crop&w=800&q=80",
                    StockQuantity = 40,
                    IsTrending = true,
                    SKU = "WZ-WM-DR-003"
                },

                // WOMEN - JEANS & TEES
                new Product
                {
                    Name = "High-Waist Vintage Jeans",
                    Description = "Classic high-waist jeans with a vintage wash.",
                    Price = 59.99m,
                    Gender = "Women",
                    Category = clothingCategory,
                    SubCategory = "Jeans",
                    ImageUrl = "https://images.unsplash.com/photo-1541099649105-f69ad21f3246?auto=format&fit=crop&w=800&q=80",
                    StockQuantity = 60,
                    SKU = "WZ-WM-JN-001"
                },
                new Product
                {
                    Name = "Essential Cotton V-Neck",
                    Description = "Soft organic cotton t-shirt, a wardrobe staple.",
                    Price = 25.00m,
                    Gender = "Women",
                    Category = clothingCategory,
                    SubCategory = "T-Shirts",
                    ImageUrl = "https://images.unsplash.com/photo-1521572163474-6864f9cf17ab?auto=format&fit=crop&w=800&q=80",
                    StockQuantity = 100,
                    SKU = "WZ-WM-TS-001"
                },

                // MEN - JEANS & TEES
                new Product
                {
                    Name = "Slim Fit Denim Jeans",
                    Description = "Modern slim fit jeans crafted from durable denim.",
                    Price = 65.00m,
                    Gender = "Men",
                    Category = clothingCategory,
                    SubCategory = "Jeans",
                    ImageUrl = "https://images.unsplash.com/photo-1604176354204-9268737828e4?auto=format&fit=crop&w=800&q=80",
                    StockQuantity = 75,
                    SKU = "WZ-MN-JN-001"
                },
                new Product
                {
                    Name = "Premium Crew Neck Tee",
                    Description = "High-quality crew neck t-shirt in classic white.",
                    Price = 29.99m,
                    Gender = "Men",
                    Category = clothingCategory,
                    SubCategory = "T-Shirts",
                    ImageUrl = "https://images.unsplash.com/photo-1581655353564-df123a1eb820?auto=format&fit=crop&w=800&q=80",
                    StockQuantity = 200,
                    IsNewArrival = true,
                    SKU = "WZ-MN-TS-001"
                },
                new Product
                {
                    Name = "Urban Graphic Tee",
                    Description = "Streetwear inspired graphic t-shirt.",
                    Price = 34.50m,
                    Gender = "Men",
                    Category = clothingCategory,
                    SubCategory = "T-Shirts",
                    ImageUrl = "https://images.unsplash.com/photo-1576566588028-4147f3842f27?auto=format&fit=crop&w=800&q=80",
                    StockQuantity = 80,
                    SKU = "WZ-MN-TS-002"
                },

                // KIDS - JEANS & TEES
                new Product
                {
                    Name = "Playtime Durable Jeans",
                    Description = "Comfortable and tough jeans for active kids.",
                    Price = 35.00m,
                    Gender = "Kids",
                    Category = clothingCategory,
                    SubCategory = "Jeans",
                    ImageUrl = "https://images.unsplash.com/photo-1519457431-44ccd64a579b?auto=format&fit=crop&w=800&q=80",
                    StockQuantity = 50,
                    AgeGroup = "Kids",
                    SKU = "WZ-KD-JN-001"
                },


                // MEN - ACCESSORIES
                new Product
                {
                    Name = "Classic Aviator Sunglasses",
                    Description = "Timeless aviator sunglasses with UV protection.",
                    Price = 149.99m,
                    Gender = "Men",
                    Category = clothingCategory,
                    SubCategory = "Accessories",
                    ImageUrl = "https://images.unsplash.com/photo-1511499767150-a48a237f0083?auto=format&fit=crop&w=800&q=80",
                    StockQuantity = 30,
                    IsTrending = true,
                    SKU = "WZ-MN-AC-001"
                },


                // WOMEN - SHOES
                new Product
                {
                    Name = "Classic White Sneakers",
                    Description = "Versatile white sneakers comfortable for all-day wear.",
                    Price = 75.00m,
                    Gender = "Women",
                    Category = clothingCategory,
                    SubCategory = "Shoes",
                    ImageUrl = "https://images.unsplash.com/photo-1543163521-1bf539c55dd2?auto=format&fit=crop&w=800&q=80",
                    StockQuantity = 40,
                    SKU = "WZ-WM-SH-001"
                },
                new Product
                {
                    Name = "Elegant Stiletto Heels",
                    Description = "Sophisticated black heels perfect for formal occasions.",
                    Price = 120.00m,
                    Gender = "Women",
                    Category = clothingCategory,
                    SubCategory = "Shoes",
                    ImageUrl = "https://images.unsplash.com/photo-1543163521-1bf539c55dd2?auto=format&fit=crop&w=800&q=80", // Using a placeholder, will update if needed
                    StockQuantity = 20,
                    SKU = "WZ-WM-SH-002"
                },

                // MEN - SHOES

                new Product
                {
                    Name = "Leather Chelsea Boots",
                    Description = "Stylish and durable leather boots for a rugged look.",
                    Price = 140.00m,
                    Gender = "Men",
                    Category = clothingCategory,
                    SubCategory = "Shoes",
                    ImageUrl = "https://images.unsplash.com/photo-1638247025967-b4e38f787b76?auto=format&fit=crop&w=800&q=80",
                    StockQuantity = 30,
                    SKU = "WZ-MN-SH-002"
                },

                // KIDS - SHOES
                new Product
                {
                    Name = "Kids Colorful Sneakers",
                    Description = "Fun and colorful sneakers with velcro straps.",
                    Price = 45.00m,
                    Gender = "Kids",
                    Category = clothingCategory,
                    SubCategory = "Shoes",
                    ImageUrl = "https://images.unsplash.com/photo-1514989940723-e8875ea6ab7d?auto=format&fit=crop&w=800&q=80",
                    StockQuantity = 60,
                    AgeGroup = "Kids",
                    SKU = "WZ-KD-SH-001"
                }
            };

            foreach (var p in products)
            {
                // Check if exists by name to avoid duplicates
                if (!context.Products.Any(db => db.Name == p.Name))
                {
                    context.Products.Add(p);
                }
            }
            context.SaveChanges();

            // 3. Randomize Stock for others
            if (context.Products.Any())
            {
                var outOfStockProducts = context.Products.Where(p => p.StockQuantity == 0).ToList();
                if (outOfStockProducts.Any())
                {
                    var random = new Random();
                    foreach (var product in outOfStockProducts)
                    {
                        product.StockQuantity = random.Next(10, 100);
                    }
                    context.SaveChanges();
                }
            }
        }
    }
}
