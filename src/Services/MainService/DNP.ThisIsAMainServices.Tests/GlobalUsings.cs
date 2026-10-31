global using Bogus;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.DependencyInjection;
global using Shouldly;
global using System.Net;
global using System.Text.Json;
global using Xunit.v3;
global using PersonDomain = DNP.ThisIsAMainService.Features.People.Domain.Person;
global using DNP.ThisIsAMainServices.Tests.Setup;



using DNP.ThisIsAMainServices.Tests;
[assembly: AssemblyFixture(typeof(TestAssemblyFixture))]
[assembly: CollectionBehavior<AutoBucketCollectionFactory>()]