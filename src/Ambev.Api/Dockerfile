FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Ambev.Api/Ambev.Api.csproj", "Ambev.Api/"]
COPY ["src/Ambev.Application/Ambev.Application.csproj", "Ambev.Application/"]
COPY ["src/Ambev.Domain/Ambev.Domain.csproj", "Ambev.Domain/"]
COPY ["src/Ambev.Infrastructure/Ambev.Infrastructure.csproj", "Ambev.Infrastructure/"]
RUN dotnet restore "Ambev.Api/Ambev.Api.csproj"
COPY src/ .
WORKDIR "/src/Ambev.Api"
RUN dotnet build "Ambev.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ambev.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ambev.Api.dll"] 