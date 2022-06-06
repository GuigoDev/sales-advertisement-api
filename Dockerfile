FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SalesAdvertisementApi/SalesAdvertisementApi.csproj", "SalesAdvertisementApi/"]
RUN dotnet restore "SalesAdvertisementApi/SalesAdvertisementApi.csproj"
COPY . .
WORKDIR "/src/SalesAdvertisementApi"
RUN dotnet build "SalesAdvertisementApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SalesAdvertisementApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

#Uses Heroku's dynamic port.
CMD ASPNETCORE_URLS="http://*:$PORT" dotnet SalesAdvertisementApi.dll