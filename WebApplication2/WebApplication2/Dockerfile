FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .

# ДОДАНО: Вивести всі файли, щоб перевірити структуру
RUN ls -R

# Після перегляду виводу — відкоригуй шлях нижче
RUN dotnet restore "WebApplication2.csproj"
RUN dotnet publish "WebApplication2.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "WebApplication2.dll"]
