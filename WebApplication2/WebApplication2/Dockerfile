FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копіюємо весь вміст (включно з вкладеною WebApplication2/)
COPY . .

# Restore (вказуємо правильний шлях до csproj)
RUN dotnet restore "WebApplication2/WebApplication2/WebApplication2.csproj"

# Publish
RUN dotnet publish "WebApplication2/WebApplication2/WebApplication2.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app

# Копіюємо з build stage опубліковану папку
COPY --from=build /app/publish .

# Запуск DLL
ENTRYPOINT ["dotnet", "WebApplication2.dll"]
