# Utiliza la imagen de .NET SDK para compilar la aplicación
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copia los archivos del proyecto y publica
COPY . .
RUN dotnet publish -c Release -o /out

# Utiliza la imagen de .NET Runtime para ejecutar la aplicación
FROM mcr.microsoft.com/dotnet/runtime:7.0
WORKDIR /app
COPY --from=build /out .

# Ejecuta la aplicación
ENTRYPOINT ["dotnet", "csharp.dll"]
