FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 10000
ENV ASPNETCORE_URLS=http://+:10000

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Liga_Api/Liga_Api.csproj", "Liga_Api/"]
RUN dotnet restore "Liga_Api/Liga_Api.csproj"
COPY . .


WORKDIR "/src/Liga_Api"

RUN dotnet build "Liga_Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Liga_Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Liga_Api.dll"]