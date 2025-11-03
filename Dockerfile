# Етап 1: базовий runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Етап 2: build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копіюємо всі файли у контейнер
COPY . .

# Відновлюємо залежності
RUN dotnet restore "WebApplication2.csproj"

# Публікуємо застосунок
RUN dotnet publish "WebApplication2.csproj" -c Release -o /app/publish

# Етап 3: фінальний образ
FROM base AS final
WORKDIR /app

# Копіюємо результати публікації
COPY --from=build /app/publish .

# Запускаємо застосунок
ENTRYPOINT ["dotnet", "WebApplication2.dll"]
