using System.ComponentModel.DataAnnotations;

namespace ProductsAPI.Models.Risks
{
    public class RiskPreferencesModel
    {
        [Required]
        public int Invesment { get; set; }

        [Required]
        public int RiskRate { get; set; }

        [Required]
        public int TotalMoney { get; set; }

        [Required]
        public int Month { get; set; }
    }
}