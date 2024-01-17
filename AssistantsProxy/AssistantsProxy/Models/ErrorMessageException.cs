using AssistantsProxy.Schema;

namespace AssistantsProxy.Models
{
    public class ErrorMessageException : Exception
    {
        public ErrorMessageException(int statusCode, ErrorMessage? errorMessage)
        {
            StatusCode = statusCode;
            ErrorMessage = errorMessage;
        }

        public int StatusCode { get; init; }

        public ErrorMessage? ErrorMessage { get; init; }
    }
}
