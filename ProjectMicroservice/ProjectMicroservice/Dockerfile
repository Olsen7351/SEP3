FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ProjectMicroservice//ProjectMicroservice.csproj", "ProjectMicroservice/"]
RUN dotnet restore "ProjectMicroservice/ProjectMicroservice.csproj"
COPY . .
WORKDIR "/src/ProjectMicroservice"
RUN dotnet build "ProjectMicroservice.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProjectMicroservice.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProjectMicroservice.dll"]
