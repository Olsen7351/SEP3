FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["BlazorAppTEST/BlazorAppTEST/BlazorAppTEST.csproj", "BlazorAppTEST/BlazorAppTEST/"]
COPY ["ClassLibrary_SEP3/ClassLibrary_SEP3.csproj", "ClassLibrary_SEP3/"]
RUN dotnet restore "BlazorAppTEST/BlazorAppTEST/BlazorAppTEST.csproj"
COPY . .
WORKDIR "/src/BlazorAppTEST/BlazorAppTEST"
RUN dotnet build "BlazorAppTEST.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BlazorAppTEST.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BlazorAppTEST.dll"]
