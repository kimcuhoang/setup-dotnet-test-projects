# use PowerShell instead of sh:
set windows-shell := ["powershell.exe", "-NoLogo", "-Command"]

default:
	clear
	just --list

dotnet-tools command:
	clear
	dotnet new tool-manifest --force
	dotnet tool {{command}} --local dotnet-outdated-tool
	dotnet tool {{command}} --local dotnet-ef

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
	deploy/efbundle.exe --verbose --connection "Data Source=localhost,1433;Initial Catalog=People-Service;User ID=sa;Password=P@ssword;TrustServerCertificate=true;"


build:
	clear
	dotnet clean
	dotnet build NET.slnx

test: build
	clear
	dotnet test src/Services/PeopleService/DNP.PeopleService.Tests.xUnitV3 --no-build --verbosity normal

ms-test: build
	clear
	dotnet run --project src/Services/PeopleService/DNP.PeopleService.Tests.xUnitV3 --no-build --no-restore --verbosity normal

start: migration-bundle migration-run build
	clear
	dotnet watch --project src/Services/PeopleService/DNP.PeopleService \
		--no-build --no-launch-profile --no-restore --verbosity normal 