# Базовий образ для рантайму ASP.NET
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Образ для побудови проєкту
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копіюємо все в контейнер
COPY . .

# Вказуємо правильний шлях до csproj
RUN dotnet restore "WebApplication2/WebApplication2.csproj"

# Публікуємо в релізі
RUN dotnet publish "WebApplication2/WebApplication2.csproj" -c Release -o /app/publish

# Формуємо фінальний образ
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

# Запускаємо застосунок
ENTRYPOINT ["dotnet", "WebApplication2.dll"]
