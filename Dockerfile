FROM mcr.microsoft.com/dotnet/sdk:8.0 AS buildApp
WORKDIR /src
COPY . .
RUN dotnet publish "AA.csproj" -c Release -o /consoleapp

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /application
COPY --from=buildApp /consoleapp ./
ENTRYPOINT ["dotnet", "AA.dll"]