namespace Elwark.Account.Shared
{
    public class ApiResponse : ApiResponse<bool>
    {
        private ApiResponse(ProblemDetails? error)
            : base(false, error)
        {
        }

        public static ApiResponse Success() =>
            new ApiResponse(null);

        public new static ApiResponse Fail(ProblemDetails error) =>
            new ApiResponse(error);
    }

    public class ApiResponse<T>
    {
        protected ApiResponse(T data, ProblemDetails? error)
        {
            Data = data;
            Error = error;
        }

        public T Data { get; }

        public ProblemDetails? Error { get; }

        public bool IsSuccess => Error is null;

        public bool IsFail => !IsSuccess;

        public static ApiResponse<T> Success(T data) =>
            new ApiResponse<T>(data, null);

        public static ApiResponse<T> Fail(ProblemDetails error) =>
            new ApiResponse<T>(default!, error);
    }
}