using ProductsAPI.Models;

namespace ProductsAPI.Data
{
    public class RiskEntity {

        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public int Invesment { get; set; }

        public int RiskRate { get; set; }

        public int TotalMoney { get; set; }

        public int Month { get; set; }

        public AppUser AppUser { get; set; }
    }
}