public interface IUserSession
{
    Guid UserId { get; }
    string UserName { get; }
    int IsAdmin { get; }
}
