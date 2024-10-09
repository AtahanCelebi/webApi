public class ErrorSummary : IErrorSummary
{
    public string Message { get; set; }
    public List<ErrorItem> Items { get; set; }
    public int StatusCode { get; set; }

    public ErrorSummary(string message)
    {
        Message = message;
        Items = new List<ErrorItem>();
    }

    public ErrorSummary(string message, int statusCode)
    {
        Message = message;
        StatusCode = statusCode;
        Items = new List<ErrorItem>();
    }

    public ErrorSummary(string activityId, string message)
    {
        Message = $"{activityId}: {message}";
        Items = new List<ErrorItem>();
    }
}
