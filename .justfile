# use PowerShell instead of sh:
set windows-shell := ["powershell.exe", "-NoLogo", "-Command"]

default:
	clear
	just --list

net-tools command:
	clear
	dotnet tool {{command}} --global dotnet-outdated-tool
	dotnet tool {{command}} --global dotnet-ef

add-migration name:
	clear
	dotnet ef migrations add {{name}} \
		-p src/Services/PeopleService/DNP.PeopleService -s src/Services/PeopleService/DNP.PeopleService \
		-c PeopleDbContext -o Persistence/Migrations

remove-migration:
	clear
	dotnet ef migrations remove \
	-p src/Services/PeopleService/DNP.PeopleService -s src/Services/PeopleService/DNP.PeopleService \
	-c PeopleDbContext

revert-migration name:
	clear
	dotnet ef database update {{name}} \
	-p src/Services/PeopleService/DNP.PeopleService -s src/Services/PeopleService/DNP.PeopleService \
	-c PeopleDbContext

migration-bundle:
	clear
	dotnet ef migrations bundle \
	-p src/Services/PeopleService/DNP.PeopleService -s src/Services/PeopleService/DNP.PeopleService \
	-c PeopleDbContext \
	-o deploy/efbundle.exe --force

migration-run:
	clear
	deploy/efbundle.exe --verbose --connection "Data Source=localhost,1433;Initial Catalog=people-service;User ID=sa;Password=P@ssword;TrustServerCertificate=true;"


build:
	clear
	dotnet clean
	dotnet build NET.slnx

test: build
	clear
	dotnet test src/Services/PeopleService/DNP.PeopleService.Tests.xUnitV3 --no-build --verbosity quiet

ms-test: build
	clear
	dotnet run --project src/Services/PeopleService/DNP.PeopleService.Tests.xUnitV3 --no-build --verbosity normal