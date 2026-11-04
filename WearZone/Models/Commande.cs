using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WearZone.Models
{
    public class Commande
    {
        [Key]
        public int Id { get; set; }
        
        public string? UserId { get; set; } // Can be null if we allow guest checkout, or strictly tied to User
        
        public DateTime DateCommande { get; set; } = DateTime.Now;
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }
        
        public string Statut { get; set; } = "En attente"; // En attente, Payée, Expédiée, Livrée
        
        // Shipping Info
        public string? Prenom { get; set; }
        public string? Nom { get; set; }
        public string? Adresse { get; set; }
        public string? Ville { get; set; }
        public string? CodePostal { get; set; }
        public string? Pays { get; set; }
        
        public List<LigneCommande> Lignes { get; set; } = new List<LigneCommande>();
    }
}
