public interface IServiceResult<T>
{
    T Data { get; }
    bool Success { get; }
    string Message { get; }
    bool HasErrors { get; }
    ErrorSummary Errors { get; }
}

public class ServiceResult<T> : IServiceResult<T>
{
    public T Data { get; private set; }
    public bool Success { get; private set; }
    public string Message { get; private set; }
    public bool HasErrors => Errors != null;
    public ErrorSummary Errors { get; private set; }

    public ServiceResult(T data, bool success, string message, ErrorSummary errors = null)
    {
        Data = data;
        Success = success;
        Message = message;
        Errors = errors;
    }
}
