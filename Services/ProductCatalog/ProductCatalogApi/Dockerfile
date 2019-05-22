FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["../../Services/ProductCatalog/ProductCatalogApi/ProductCatalogApi.csproj", "../../Services/ProductCatalog/ProductCatalogApi/"]
RUN dotnet restore "../../Services/ProductCatalog/ProductCatalogApi/ProductCatalogApi.csproj"
COPY . .
WORKDIR "/src/../../Services/ProductCatalog/ProductCatalogApi"
RUN dotnet build "ProductCatalogApi.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "ProductCatalogApi.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ProductCatalogApi.dll"]