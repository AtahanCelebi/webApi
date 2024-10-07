using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductsAPI.Data;
using ProductsAPI.Models.Risks;
using Microsoft.EntityFrameworkCore;
using ProductsAPI.Services;

namespace ProductsAPI.Controllers
{

    [ApiController]
    [Authorize]
    [Route("api/[controller]")] // => api/stock
    public class RisksController : ControllerBase
    {

        private readonly MainDbContext _mainDbContext;

        private readonly IUserSession _userSession;

        private readonly IRiskServices _risksService;

        public RisksController(MainDbContext mainDbContext, IUserSession userSession, IRiskServices riskServices)
        {
            _mainDbContext = mainDbContext;
            _userSession = userSession;
            _risksService = riskServices;
        }

        [HttpGet("preferences")]
        public async Task<IActionResult> GetRiskPreferences()
        {
            // Check if _mainDbContext.Risks is not null
            if (_mainDbContext.Risks == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Risk table not available.");
            }

            // Check if UserId is available
            if (_userSession.UserId == null)
            {
                return BadRequest("User is not authenticated.");
            }

            // Fetch the risk preferences from the database
            var riskEntity = await _mainDbContext.Risks
                                 .Where(x => x.UserId == _userSession.UserId)
                                 .ToListAsync();

            if (riskEntity == null || !riskEntity.Any())
            {
                return NotFound("Risk preferences couldn't be found.");
            }

            return Ok(riskEntity);
        }


        [HttpPost]
        public async Task<IActionResult> CreateRiskPreference(RiskPreferencesModel model)
        {
            if (model is null) return NotFound("Risk preference's fields couldn't empty!");

            var getUser = await this._mainDbContext.Risks.Where(x => x.UserId == this._userSession.UserId).FirstOrDefaultAsync();
            if (getUser != null) return NotFound("Risk preferences already have been created!");

            var riskEntity = new RiskEntity
            {
                Id = Guid.NewGuid(),
                UserId = this._userSession.UserId,
                Invesment = model.Invesment,
                TotalMoney = model.TotalMoney,
                RiskRate = model.RiskRate,
                Month = model.Month,
            };

            _mainDbContext.Add(riskEntity);
            _ = await _mainDbContext.SaveChangesAsync();

            return Ok(model);

        }

        [HttpPut]
        public async Task<IActionResult> UpdateRiskPreference(RiskPreferencesUpdateModel model)
        {
            var riskEntity = await this._mainDbContext.Risks
            .Where(x => x.Id == model.Id && x.UserId == this._userSession.UserId)
            .SingleOrDefaultAsync();

            if (riskEntity == null) return NotFound("Risk preferences options are not found.");

            try
            {
                riskEntity.Invesment = model.Invesment;
                riskEntity.RiskRate = model.RiskRate;
                riskEntity.Month = model.Month;
                riskEntity.TotalMoney = model.TotalMoney;

                _ = await _mainDbContext.SaveChangesAsync();
                return Ok("Updated successfully.");
            }
            catch (Exception)
            {
                return NotFound();
            }
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteRiskPreference(Guid id)
        {
            var riskEntity = await this._mainDbContext.Risks
            .Where(x => x.Id == id && x.UserId == this._userSession.UserId)
            .SingleOrDefaultAsync();

            if (riskEntity == null) return NotFound("Risk preferences couldn't found.");

            _ = _mainDbContext.Remove(riskEntity);

            try
            {
                _ = await _mainDbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}