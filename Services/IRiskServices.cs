
using ProductsAPI.Models;
using ProductsAPI.Models.Risks;

namespace ProductsAPI.Services;
public interface IRiskServices
{
    Task<IServiceResult<RiskResponseModel>> List();
}
