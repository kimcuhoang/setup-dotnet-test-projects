namespace DNP.PeopleService.Features.People.Domain;

public class Person
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public static Person Default => new()
    {
        Id = Guid.Parse("91e08a8b-0512-418d-9275-ee893842f82a"),
        Name = "John Doe"
    };
}
