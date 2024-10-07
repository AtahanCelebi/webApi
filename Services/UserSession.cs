using System.Security.Claims;

public class UserSession : IUserSession
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserSession(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid UserId
    {
        get
        {
            var userIdString = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(userIdString, out var userId) ? userId : Guid.Empty;
        }
    }

    public string UserName => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
    
    public int IsAdmin
    {
        get
        {
            var claimValue = _httpContextAccessor.HttpContext?.User?.FindFirstValue("IsAdmin");
            return int.TryParse(claimValue, out var isAdmin) ? isAdmin : 0;
        }
    }
}
