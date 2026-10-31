namespace DNP.ThisIsAMainService.Shared.Models.ResultPattern;

public class Result
{
    public bool IsSuccess { get; }
    public Reason? Reason { get; }

    protected Result(bool isSuccess, Reason? reason)
    {
        IsSuccess = isSuccess;
        Reason = reason;
    }

    public IResult ToTypedResult(Func<IResult> onSuccess)
    {
        if (this.IsSuccess)
        {
            return onSuccess();
        }
        
        return this.Reason!.Type switch
        {
            ReasonType.NotFound => TypedResults.Problem(statusCode: StatusCodes.Status404NotFound, detail: this.Reason.Message),
            ReasonType.Forbidden => TypedResults.Problem(statusCode: StatusCodes.Status403Forbidden, detail: this.Reason.Message),
            ReasonType.Conflict => TypedResults.Problem(statusCode: StatusCodes.Status409Conflict, detail: this.Reason.Message),
            ReasonType.NotAcceptable => TypedResults.Problem(statusCode: StatusCodes.Status406NotAcceptable, detail: this.Reason.Message),
            _ => TypedResults.Problem(statusCode: StatusCodes.Status500InternalServerError, detail: "An unexpected error occurred.")
        };
    }

    public static Result Success() => new(true, null);
    public static Result Failure(Reason reason) => new(false, reason);
}

public sealed class Result<T> : Result
{
    public T? Value { get; }

    private Result(bool isSuccess, T? value, Reason? reason): base(isSuccess, reason)
    {
        Value = value;
    }

    public IResult ToTypedResults(Func<T?, IResult> onSuccess)
    {
        if (this.IsSuccess)
        {
            return onSuccess(this.Value!);
        }

        return this.Reason!.Type switch
        {
            ReasonType.NotFound => TypedResults.Problem(statusCode: StatusCodes.Status404NotFound, detail: this.Reason.Message),
            ReasonType.Forbidden => TypedResults.Problem(statusCode: StatusCodes.Status403Forbidden, detail: this.Reason.Message),
            ReasonType.Conflict => TypedResults.Problem(statusCode: StatusCodes.Status409Conflict, detail: this.Reason.Message),
            ReasonType.NotAcceptable => TypedResults.Problem(statusCode: StatusCodes.Status406NotAcceptable, detail: this.Reason.Message),
            _ => TypedResults.Problem(statusCode: StatusCodes.Status500InternalServerError, detail: "An unexpected error occurred.")
        };
    }

    public static Result<T> Success(T value) => new(true, value, null);
    public static new Result<T> Failure(Reason reason) => new(false, default, reason);
}