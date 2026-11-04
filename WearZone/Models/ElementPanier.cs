using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WearZone.Models
{
    public class ElementPanier
    {
        [Key]
        public int Id { get; set; }
        
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        
        public int PanierId { get; set; }
        [ForeignKey("PanierId")]
        public virtual Panier Panier { get; set; }
        
        public int Quantite { get; set; }
    }
}