global using Bogus;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.DependencyInjection;
global using Shouldly;
global using System.Net;
global using System.Text.Json;
global using Xunit.v3;
global using PersonDomain = DNP.ThisIsAMainService.Shared.Domain.Person;
global using DNP.ThisIsAMainService.Tests.Setup;



using DNP.ThisIsAMainService.Tests;
[assembly: AssemblyFixture(typeof(TestAssemblyFixture))]
[assembly: CollectionBehavior<AutoBucketCollectionFactory>()]