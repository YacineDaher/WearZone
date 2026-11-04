using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace WearZone.Models
{
    public class Panier
    {
        [Key]
        public int Id { get; set; }
        
        public string? UserId { get; set; } // Stores the Identity User ID (or Session ID for guests)
        
        public List<ElementPanier> Elements { get; set; } = new List<ElementPanier>();
    }
}
