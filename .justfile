# use PowerShell instead of sh:
set windows-shell := ["powershell.exe", "-NoLogo", "-Command"]

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


build:
	clear
	dotnet build NET.slnx

test: build
	clear
	dotnet test src/Services/PeopleService/DNP.PeopleService.Tests/DNP.PeopleService.Tests.csproj --no-build --verbosity normal