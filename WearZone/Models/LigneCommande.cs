using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WearZone.Models
{
    public class LigneCommande
    {
        [Key]
        public int Id { get; set; }
        
        public int CommandeId { get; set; }
        [ForeignKey("CommandeId")]
        public Commande Commande { get; set; }
        
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        
        public int Quantite { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrixUnitaire { get; set; } // Snapped price at time of purchase
    }
}
