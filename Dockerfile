FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR ./
COPY ["/src/Notebooks.Api/Notebooks.Api.csproj", "src/Notebooks.Api/"]
RUN dotnet restore "src/Notebooks.Api/Notebooks.Api.csproj"
COPY . .
WORKDIR "/src/Notebooks.Api"
RUN dotnet build "Notebooks.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Notebooks.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Notebooks.Api.dll"]
