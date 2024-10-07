using Microsoft.AspNetCore.Identity;

namespace ProductsAPI.Models
{
    public class AppRole : IdentityRole<Guid>
    {
        public int IsAdmin { get; set; }
    }
}