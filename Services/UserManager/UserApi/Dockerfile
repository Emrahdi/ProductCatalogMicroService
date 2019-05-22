FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["../../Services/UserManager/UserApi/UserApi.csproj", "../../Services/UserManager/UserApi/"]
RUN dotnet restore "../../Services/UserManager/UserApi/UserApi.csproj"
COPY . .
WORKDIR "/src/../../Services/UserManager/UserApi"
RUN dotnet build "UserApi.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "UserApi.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "UserApi.dll"]