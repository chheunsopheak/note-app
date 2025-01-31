using Newtonsoft.Json;

namespace noted_api.Wrappers;

public class Result : IResult
{
    public Result()
    {
    }

    [JsonProperty("message")] public string Message { get; set; }

    [JsonProperty("succeeded")] public bool Succeeded { get; set; }

    private static IResult Fail()
    {
        return new Result { Succeeded = false };
    }

    private static IResult Fail(string message)
    {
        return new Result { Succeeded = false, Message = message };
    }

    public static Task<IResult> FailAsync()
    {
        return Task.FromResult(Fail());
    }

    public static Task<IResult> FailAsync(string message)
    {
        return Task.FromResult(Fail(message));
    }

    private static IResult Success()
    {
        return new Result { Succeeded = true };
    }

    private static IResult Success(string message)
    {
        return new Result { Succeeded = true, Message = message };
    }

    public static Task<IResult> SuccessAsync()
    {
        return Task.FromResult(Success());
    }

    public static Task<IResult> SuccessAsync(string message)
    {
        return Task.FromResult(Success(message));
    }
}

public class Result<T> : Result, IResult<T>
{
    public Result()
    {
    }

    public T Data { get; set; }

    private new static Result<T> Fail()
    {
        return new Result<T> { Succeeded = false };
    }

    private new static Result<T> Fail(string message)
    {
        return new Result<T> { Succeeded = false, Message = message };
    }

    public new static Task<Result<T>> FailAsync()
    {
        return Task.FromResult(Fail());
    }

    public static Result<T> FailAsync(T data)
    {
        return new Result<T> { Succeeded = false, Data = data };
    }

    public new static Task<Result<T>> FailAsync(string message)
    {
        return Task.FromResult(Fail(message));
    }

    public static Result<T> FailAsync(T data, string message)
    {
        return new Result<T> { Succeeded = false, Data = data, Message = message };
    }

    private new static Result<T> Success()
    {
        return new Result<T> { Succeeded = true };
    }

    private new static Result<T> Success(string message)
    {
        return new Result<T> { Succeeded = true, Message = message };
    }

    private static Result<T> Success(T data)
    {
        return new Result<T> { Succeeded = true, Data = data };
    }

    private static Result<T> Success(T data, string message)
    {
        return new Result<T> { Succeeded = true, Data = data, Message = message };
    }

    public new static Task<Result<T>> SuccessAsync()
    {
        return Task.FromResult(Success());
    }

    public new static Task<Result<T>> SuccessAsync(string message)
    {
        return Task.FromResult(Success(message));
    }

    public static Task<Result<T>> SuccessAsync(T data)
    {
        return Task.FromResult(Success(data));
    }

    public static Task<Result<T>> SuccessAsync(T data, string message)
    {
        return Task.FromResult(Success(data, message));
    }
}