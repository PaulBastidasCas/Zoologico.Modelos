FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 10000
ENV ASPNETCORE_URLS=http://+:10000

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Prueba1/Prueba1.csproj", "Prueba1/"]
RUN dotnet restore "Prueba1/Prueba1.csproj"
COPY . .
WORKDIR "/src/Prueba1"
RUN dotnet build "Prueba1.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Prueba1.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Prueba1.dll"]