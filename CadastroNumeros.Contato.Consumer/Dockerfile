# Etapa 1: Construção
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar arquivos de projeto e restaurar dependências
COPY CadastroNumeros.Contato.Consumer/*.csproj ./CadastroNumeros.Contato.Consumer/
RUN dotnet restore CadastroNumeros.Contato.Consumer/CadastroNumeros.Contato.Consumer.csproj

# Copiar todo o código do projeto e compilar
COPY . .
RUN dotnet publish CadastroNumeros.Contato.Consumer/CadastroNumeros.Contato.Consumer.csproj -c Release -o /app/publish

# Etapa 2: Execução
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS runtime
WORKDIR /app

# Copiar o resultado da publicação
COPY --from=build /app/publish .

# Copiar o script wait-for-it para o container
COPY CadastroNumeros.Contato.Consumer/wait-for-it.sh /app/wait-for-it.sh
RUN chmod +x /app/wait-for-it.sh

RUN apt-get update && apt-get install -y iputils-ping

# Ponto de entrada do container
#ENTRYPOINT ["/app/wait-for-it.sh", "rabbitmq", "--", "dotnet", "CadastroNumeros.Contato.Consumer.dll"]

# Configurar o ponto de entrada para o container
ENTRYPOINT ["dotnet", "CadastroNumeros.Contato.Consumer.dll"]
