What should do to create a database (and then add migrations) for project with DDD layers:
1) Add nuget packages Efcore, efcore tools and Microsoft.EntityFrameworkCore.Design to Domain layer(where is the Domain level located). 
2) { efcore/ dotnet ef } --startup-project ./server_app.Presentation/server_app.Presentation.csproj migrations add InitMig --context MainDbContext  --output-dir Migrations --project ./server_app.Domain/server_app.Domain.csproj
3) Create db -> "CREATE DATABASE marketplacedb  WITH LC_COLLATE='en_US.UTF-8' LC_CTYPE='en_US.UTF-8' TEMPLATE=template0;"
4) efcore --startup-project ./server_app.Presentation/server_app.Presentation.csproj database update --context MainDbContext --project ./server_app.Domain/server_app.Domain.csproj --verbose --connection "Database=marketplacedb;Server=localhost;Port=5432;User Id = postgres;Password=nigPostgres_Pas5432;Pooling=true"
