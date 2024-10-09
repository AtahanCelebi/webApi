using System.Threading;
using System.Threading.Tasks;
using Mapster;
using Microsoft.EntityFrameworkCore;
using ProductsAPI.Data;
using ProductsAPI.Models.Risks;
using ProductsAPI.Services;

public class RiskServices : IRiskServices
{
    private readonly MainDbContext _mainDbContext;

    public RiskServices(MainDbContext mainDbContext)
    {
        _mainDbContext = mainDbContext;
    }

    public async Task<IServiceResult<RiskResponseModel>> List()
    {
        var risk = await _mainDbContext.Risks.Where(x => x.Invesment == 5)
        .Select(x => new RiskResponseModel {
            TotalMoney = x.TotalMoney,
            Month = x.Month,
            RiskRate = x.RiskRate,
            Invesment = x.Invesment
        })
        .FirstOrDefaultAsync();

        var result = new ServiceResult<RiskResponseModel>(risk, true, "Success");
        return result;
    }
}
