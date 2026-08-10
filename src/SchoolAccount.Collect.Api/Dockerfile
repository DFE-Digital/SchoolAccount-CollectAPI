# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Packages.props", "."]
COPY ["Directory.Build.props", "."]
COPY ["src/SchoolAccount.Collect.Api/SchoolAccount.Collect.Api.csproj", "src/SchoolAccount.Collect.Api/"]
COPY ["src/SchoolAccount.Collect.Infrastructure/SchoolAccount.Collect.Infrastructure.csproj", "src/SchoolAccount.Collect.Infrastructure/"]
COPY ["src/SchoolAccount.Collect.Application/SchoolAccount.Collect.Application.csproj", "src/SchoolAccount.Collect.Application/"]
COPY ["src/SchoolAccount.Collect.Domain/SchoolAccount.Collect.Domain.csproj", "src/SchoolAccount.Collect.Domain/"]
COPY ["src/SchoolAccount.Collect.SharedKernel/SchoolAccount.Collect.SharedKernel.csproj", "src/SchoolAccount.Collect.SharedKernel/"]
RUN dotnet restore "./src/SchoolAccount.Collect.Api/SchoolAccount.Collect.Api.csproj"
COPY . .
WORKDIR "/src/src/SchoolAccount.Collect.Api"
RUN dotnet build "./SchoolAccount.Collect.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SchoolAccount.Collect.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SchoolAccount.Collect.Api.dll"]