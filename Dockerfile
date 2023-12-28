# Fase de build: Use a imagem base do Alpine Linux com o SDK do .NET Core
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /app

# Copie todos os arquivos do projeto para o diretório de trabalho do contêiner de build
COPY . .

# Restaure as dependências e compile o aplicativo
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Fase de runtime: Imagem leve do .NET Core
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS runtime

# Defina as variáveis de ambiente para suportar internacionalização
ENV \
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false \
    LC_ALL=en_US.UTF-8 \
    LANG=en_US.UTF-8

# Instale os pacotes necessários
RUN apk add --no-cache \
    icu-data-full \
    icu-libs \
    libgdiplus

# Configure o diretório de trabalho no contêiner
WORKDIR /app

# Copie os arquivos publicados da fase de build para o diretório de trabalho no contêiner
COPY --from=build /app/out ./

# Comando para iniciar o aplicativo quando o contêiner for iniciado
ENTRYPOINT ["dotnet", "DanfeGenAPI.dll"]
