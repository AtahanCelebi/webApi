using Microsoft.AspNetCore.Identity;
using ProductsAPI.Data;

namespace ProductsAPI.Models
{
    public class AppUser: IdentityUser<Guid>
     {
        public Guid Id { get; set; }
        public string FullName { get; set; } = null!;
    
        public DateTime DateAdded { get; set; }

        public int IsAdmin { get; set; }

        public RiskEntity Risk { get; set; }
    }

}