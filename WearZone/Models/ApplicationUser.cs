using Microsoft.AspNetCore.Identity;

namespace WearZone.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime RegisteredAt { get; set; } = DateTime.Now;
        public DateTime? LastLoginAt { get; set; }
        
        // Customer specific fields
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? ZipCode { get; set; }
        public string? Country { get; set; }
    }
}
