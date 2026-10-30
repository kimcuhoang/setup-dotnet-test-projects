# use PowerShell instead of sh:
set windows-shell := ["powershell.exe", "-NoLogo", "-Command"]

default:
	clear
	just --list

install-dotnet-tools:
	clear
	dotnet new tool-manifest --force
	dotnet tool install --local dotnet-outdated-tool
	dotnet tool install --local dotnet-ef

update-dotnet-tools:
	clear
	dotnet tool update --all

check-deprecated:
    dotnet list package --deprecated --include-transitive

check-vulnerable:
    dotnet list package --vulnerable --include-transitive

latest-nuget-packages:
	clear
	dotnet outdated -u

clean-untrack:
	clear
	git clean -fdx

add-migration name:
	clear
	dotnet ef migrations add {{name}} \
		-p src/Services/MainService/DNP.ThisIsAMainService -s src/Services/MainService/DNP.ThisIsAMainService \
		-c PeopleDbContext -o Infrastructures/Persistence/Migrations

remove-migration:
	clear
	dotnet ef migrations remove \
	-p src/Services/MainService/DNP.ThisIsAMainService -s src/Services/MainService/DNP.ThisIsAMainService \
	-c PeopleDbContext

revert-migration name:
	clear
	dotnet ef database update {{name}} \
	-p src/Services/MainService/DNP.ThisIsAMainService -s src/Services/MainService/DNP.ThisIsAMainService \
	-c PeopleDbContext

migration-bundle:
	clear
	dotnet ef migrations bundle \
	-p src/Services/MainService/DNP.ThisIsAMainService -s src/Services/MainService/DNP.ThisIsAMainService \
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
	dotnet test src/Services/MainService/DNP.ThisIsAMainServices.Tests --no-build --verbosity normal

ms-test: build
	clear
	dotnet run --project src/Services/MainService/DNP.ThisIsAMainServices.Tests --no-build --no-restore --verbosity normal

start: migration-bundle migration-run
	clear
	dotnet watch --project src/Services/MainService/DNP.ThisIsAMainService \
		--no-build --no-launch-profile --no-restore --verbosity normal 