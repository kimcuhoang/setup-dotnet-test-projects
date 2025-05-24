using DNP.PeopleService.Features.People.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DNP.PeopleService.Persistence.Configurations;

public class PersonTypeConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.ToTable(nameof(Person));

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(200);
    }
}
