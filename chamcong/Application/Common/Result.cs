namespace chamcong.Application.Common
{
    public class Result
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public int StatusCode { get; set; }

        public static Result Ok(string message = "Success") => new Result { Success = true, Message = message, StatusCode = 200 };
        public static Result Created(string message = "Created") => new Result { Success = true, Message = message, StatusCode = 201 };
        public static Result Failure(string message, int statusCode = 400) => new Result { Success = false, Message = message, StatusCode = statusCode };
    }

    public class Result<T> : Result
    {
        public T? Data { get; set; }

        public static Result<T> Ok(T data, string message = "Success") => new Result<T> { Success = true, Data = data, Message = message, StatusCode = 200 };
        public static Result<T> Created(T data, string message = "Created") => new Result<T> { Success = true, Data = data, Message = message, StatusCode = 201 };
        public static new Result<T> Failure(string message, int statusCode = 400) => new Result<T> { Success = false, Message = message, StatusCode = statusCode };
    }
}
