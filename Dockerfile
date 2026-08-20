# Usa a imagem oficial do SDK do .NET para construir o projeto
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia e restaura
COPY ["LojaBackEnd.csproj", "./"]
RUN dotnet restore "LojaBackEnd.csproj"

# Compila a aplicação
COPY . .
RUN dotnet publish "LojaBackEnd.csproj" -c Release -o /app/publish

# --- A MÁGICA ACONTECE AQUI ---
# Usa o ALPINE, um Linux super leve perfeito para servidores gratuitos
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# Mantém a economia de memória ligada
ENV DOTNET_gcServer=0
ENV DOTNET_EnableDiagnostics=0

# O Alpine precisa disso para rodar C# sem dar conflito de idioma/texto
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1

# Liga a API
ENTRYPOINT ["dotnet", "LojaBackEnd.dll"]