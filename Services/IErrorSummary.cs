public interface IErrorSummary
{
    string Message { get; }
    List<ErrorItem> Items { get; }
    int StatusCode { get; }
}
