# Etapa de construcción
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS buildApp
WORKDIR /src
COPY . .
RUN dotnet publish "AA.csproj" -c Release -o /consoleapp

# Etapa de ejecución
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /application

# Copiar la aplicación publicada desde la etapa de construcción
COPY --from=buildApp /consoleapp ./

# Crear un volumen para almacenar archivos importantes (ej. data)
VOLUME ["/appdata"]

# Exponer el puerto 7251
EXPOSE 7251

# Configurar el punto de entrada
ENTRYPOINT ["dotnet", "AA.dll"]