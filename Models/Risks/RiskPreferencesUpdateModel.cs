using System.ComponentModel.DataAnnotations;

namespace ProductsAPI.Models.Risks
{
    public class RiskPreferencesUpdateModel
    {
        public Guid Id { get; set; }
        public int Invesment { get; set; }

        public int RiskRate { get; set; }

        public int TotalMoney { get; set; }

        public int Month { get; set; }
    }
}