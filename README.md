# Guia de Configuração e Execução

## Pré-requisitos

Antes de começar, certifique-se de ter instalado:

- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (apenas se quiser executar os testes localmente)

## Executando com Docker Compose (Recomendado)

### 1. Clone o Repositório

```bash
git clone [URL_DO_REPOSITÓRIO]
cd Ambev-Teste
```

### 2. Execute o Docker Compose

```bash
docker-compose up -d
```

Este comando irá:

- Construir e iniciar a API
- Iniciar o PostgreSQL na porta 5432
- Iniciar o Redis na porta 6379
- Configurar a rede entre os serviços

### 3. Acesse a Aplicação

A aplicação estará disponível em:

- API: `http://localhost:5000` ou `https://localhost:5001`
- PostgreSQL: `localhost:5432`
- Redis: `localhost:6379`

### 4. Verifique os Logs

```bash
docker-compose logs -f api
```

### 5. Parar os Serviços

```bash
docker-compose down
```

## Executando Localmente (Sem Docker)

### 1. Configuração do PostgreSQL

```bash
docker run --name postgres -e POSTGRES_USER=postgres -e POSTGRES_PASSWORD=postgres -e POSTGRES_DB=ambev -p 5432:5432 -d postgres:latest
```

### 2. Configuração do Redis

```bash
docker run --name redis -p 6379:6379 -d redis:7
```

### 3. Restaure as Dependências

```bash
dotnet restore
```

### 4. Aplique as Migrações do Banco de Dados

```bash
cd src/Ambev.Infrastructure
dotnet ef database update --startup-project ../Ambev.Api/Ambev.Api.csproj --context ApplicationDbContext
```

### 5. Execute a Aplicação

```bash
cd ../Ambev.Api
dotnet run
```

## Executando os Testes

### 1. Testes Unitários

```bash
cd ../Ambev.Tests
dotnet test
```

### 2. Testes de Integração

```bash
dotnet test --filter "Category=Integration"
```

## Configurações Adicionais

### Variáveis de Ambiente

O Docker Compose já configura automaticamente as seguintes variáveis:

- `ConnectionStrings__DefaultConnection`: Host=postgres;Database=ambev;Username=postgres;Password=postgres
- `Redis__ConnectionString`: redis:6379
- `ASPNETCORE_ENVIRONMENT`: Development

### Configuração do CORS

Por padrão, a aplicação está configurada para aceitar requisições dos seguintes origens:

- http://localhost:4200
- https://localhost:4200
- http://localhost:3000
- https://localhost:3000
