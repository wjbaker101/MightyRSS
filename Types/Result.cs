using System.Net;

namespace MightyRSS.Types
{
    public class Result
    {
        public string ErrorMessage { get; protected init; }
        public HttpStatusCode StatusCode { get; protected init; }

        public bool IsFailure { get; protected init; }
        public bool IsSuccess => !IsFailure;

        protected Result()
        {
        }

        public static Result Success()
        {
            return new()
            {
                IsFailure = false
            };
        }

        public static Result Error(string errorMessage, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            return new()
            {
                ErrorMessage = errorMessage,
                StatusCode = statusCode,
                IsFailure = true
            };
        }
    }

    public sealed class Result<T> : Result
    {
        public T Value { get; private init; }

        public static Result Of(T value, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return new Result<T>
            {
                Value = value,
                StatusCode = statusCode
            };
        }
    }
}