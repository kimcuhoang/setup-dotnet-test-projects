namespace DNP.ThisIsAMainService.Domain.People;

public interface IPersonRepository
{
    Task Add(Person person, CancellationToken cancellationToken = default);
}