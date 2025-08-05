# Базовий образ для ASP.NET
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# SDK для збірки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копіюємо все в контейнер
COPY . .

# Виконуємо restore і publish для існуючого проєкту
RUN dotnet restore "WebApplication2/WebApplication2.csproj"
RUN dotnet publish "WebApplication2/WebApplication2.csproj" -c Release -o /app/publish

# Фінальний контейнер
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "WebApplication2.dll"]
