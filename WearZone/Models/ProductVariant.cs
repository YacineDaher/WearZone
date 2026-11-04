using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WearZone.Models
{
    public class ProductVariant
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product? Product { get; set; }

        [Required]
        public string Color { get; set; } // ex: Noir, Bleu

        [Required]
        public string Size { get; set; } // ex: S, M, L, XL

        public int StockQuantity { get; set; }
    }
}