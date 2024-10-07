public class ErrorItem
{
    public string FieldName { get; set; }
    public string ErrorMessage { get; set; }

    public ErrorItem(string fieldName, string errorMessage)
    {
        FieldName = fieldName;
        ErrorMessage = errorMessage;
    }
}
