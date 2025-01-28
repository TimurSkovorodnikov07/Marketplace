# Не рабочий(пока чтт)

# .NET Core SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Sets the working directory
WORKDIR /app

ARG ENVIRONMENT=Development
EXPOSE 5183

#COPY *.sln .
COPY ["server_app.Presentation/server_app.Presentation.csproj", "server_app.Presentation/"]
COPY ["server_app.Infrastructure/server_app.Infrastructure.csproj", "server_app.Infrastructure/"]
COPY ["server_app.Domain/server_app.Domain.csproj", "server_app.Domain/"]
COPY ["server_app.Application/server_app.Application.csproj", "server_app.Application/"]

# .NET Core Restore
RUN dotnet restore server_app.Presentation/server_app.Presentation.csproj

# Copy All Files
COPY . .

# .NET Core Build and Publish
RUN dotnet publish server_app.Presentation/server_app.Presentation.csproj -c Development -o /publish

# ASP.NET Core Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /publish ./

ENV ASPNETCORE_ENVIRONMENT $ENVIRONMENT

ENTRYPOINT ["dotnet", "DDD.Services.Api.dll"]