using System;

namespace WearZone.Models
{
    public class WishlistItem
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public DateTime DateAdded { get; set; } = DateTime.Now;
    }
}
