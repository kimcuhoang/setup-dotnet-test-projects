using DNP.ThisIsAMainService.Common.Primitives;

namespace DNP.ThisIsAMainService.Domain.People;

public class Person : Entity
{
    public string Name { get; set; }

    public static Person Default => new()
    {
        Id = Guid.Parse("91e08a8b-0512-418d-9275-ee893842f82a"),
        Name = "John Doe"
    };
}
