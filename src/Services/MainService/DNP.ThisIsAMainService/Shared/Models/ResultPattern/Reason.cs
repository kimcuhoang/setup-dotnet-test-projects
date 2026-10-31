namespace DNP.ThisIsAMainService.Shared.Models.ResultPattern;

public enum ReasonType
{
    NotFound,
    Conflict,
    NotAcceptable,
    Forbidden
}
public class Reason
{
    public ReasonType Type { get; }
    public string Message { get; }

    internal Reason(ReasonType type, string message)
    {
        Type = type;
        Message = message;
    }

    public static Reason NotAcceptable(string message = "Invalid request.") => new(ReasonType.NotAcceptable, message);
    public static Reason Forbidden(string message = "Forbidden.") => new(ReasonType.Forbidden, message);
    public static Reason NotFound(string message = "Resource not found.") => new(ReasonType.NotFound, message);
    public static Reason Conflict(string message = "Conflict.") => new(ReasonType.Conflict, message);
}