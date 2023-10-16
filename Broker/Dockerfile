FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Broker/Broker.csproj", "Broker/"]
COPY ["ClassLibrary_SEP3/ClassLibrary_SEP3.csproj", "ClassLibrary_SEP3/"]
RUN dotnet restore "Broker/Broker.csproj"
COPY . .
WORKDIR "/src/Broker"
RUN dotnet build "Broker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Broker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Broker.dll"]
