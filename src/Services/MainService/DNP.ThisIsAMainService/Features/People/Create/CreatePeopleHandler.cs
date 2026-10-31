using DNP.ThisIsAMainService.Shared.Models.ResultPattern;

namespace DNP.ThisIsAMainService.Features.People.Create;


public record CreatePersonPayload(string Name);

public class CreatePeopleCommand(CreatePersonPayload payload)
{
    public CreatePersonPayload Payload {get; } = payload;
}

internal class CreatePeopleCommandHandler()
{
    public async Task<Result> Handler(CreatePeopleCommand command, CancellationToken cancellationToken = default)
    {
        var payload = command.Payload;

        await Task.Yield();

        if (payload.Name.Contains("DUPLICATE", StringComparison.OrdinalIgnoreCase))
        {
            return Result.Failure(Reason.NotAcceptable(message: "This user's name is being used"));
        }

        if (payload.Name.Contains("EXCEPTION", StringComparison.OrdinalIgnoreCase))
        {
            throw new Exception("This is expected exception");
        }

        return Result.Success();
    }
}