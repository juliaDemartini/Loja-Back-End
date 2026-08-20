# Usa a imagem oficial do SDK do .NET para construir o projeto
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia os arquivos do projeto e restaura as dependências
COPY ["LojaBackEnd.csproj", "./"]
RUN dotnet restore "LojaBackEnd.csproj"

# Copia o resto do código e compila a aplicação
COPY . .
RUN dotnet publish "LojaBackEnd.csproj" -c Release -o /app/publish

# Usa uma imagem mais leve apenas para rodar a aplicação
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Diz ao Render qual porta a API vai usar
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# O comando que faz a API ligar
ENTRYPOINT ["dotnet", "LojaBackEnd.dll"]