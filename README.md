# Sistema de Gestão de Estoque de Veículos

Sistema desenvolvido para gerenciamento de estoque de veículos e seus respectivos proprietários.

A aplicação permite cadastrar, consultar, editar e excluir veículos, além de manter o histórico de proprietários e controlar o processo de venda de um veículo.

## Tecnologias utilizadas

### Backend

- C#
- .NET 8
- ASP.NET Core Web API
- ADO.NET
- Oracle.ManagedDataAccess.Core
- Swagger

### Frontend

- React
- Vite
- Material UI
- Axios

### Banco de dados

- Oracle Database
- Acesso aos dados utilizando ADO.NET puro
- Queries SQL escritas manualmente
- Sem utilização de ORM ou micro-ORM

---

## Versão do .NET

O projeto foi desenvolvido utilizando:

```text
.NET 8
```

SDK utilizado durante o desenvolvimento:

```text
8.0.425
```

Para verificar a versão instalada:

```bash
dotnet --version
```

---

## Estrutura do projeto

```text
gestao-estoque-veiculos/
│
├── EstoqueVeiculos.Api/     # API ASP.NET Core
├── frontend/                # Aplicação React
├── database/
│   └── create_tables.sql    # Script de criação das tabelas
└── README.md
```

---

## Banco de dados

O banco de dados utilizado no projeto é o **Oracle**.

O script para criação das tabelas está disponível em:

```text
database/create_tables.sql
```

Foram utilizadas as tabelas:

- `VEICULO`
- `PROPRIETARIO`

O relacionamento entre elas é de **um veículo para muitos proprietários (1:N)**.

A tabela `PROPRIETARIO` possui a chave estrangeira `VEICULOID`, que referencia `VEICULO.ID`.

---

## Acesso ao banco de dados

O banco Oracle utilizado durante o desenvolvimento está disponível através de uma rede privada.

O acesso à rede é realizado utilizando o **Tailscale**.

Para utilizar o banco disponibilizado juntamente com o projeto:

1. Instale o Tailscale.
2. Aceite o convite de acesso enviado juntamente com a entrega do projeto.
3. Conecte-se à rede pelo Tailscale.
4. Configure a connection string do Oracle.
5. Execute a API normalmente.

É necessário estar conectado ao Tailscale para que a API consiga acessar o banco Oracle disponibilizado para o teste.

As informações de acesso ao banco serão fornecidas separadamente junto com a entrega do projeto.

---

## Configuração da conexão com o Oracle

Por segurança, as credenciais do banco de dados não são armazenadas diretamente no repositório.

A connection string pode ser configurada utilizando o **.NET User Secrets**.

Acesse a pasta da API:

```bash
cd EstoqueVeiculos.Api
```

Configure a conexão:

```bash
dotnet user-secrets set "ConnectionStrings:Oracle" "User Id=SEU_USUARIO;Password=SUA_SENHA;Data Source=HOST:PORT/SERVICE_NAME"
```

Exemplo do formato:

```text
User Id=usuario;Password=senha;Data Source=host:1521/XEPDB1
```

---

## Como executar o backend

### Pré-requisitos

- .NET 8 SDK
- Acesso ao banco Oracle
- Tailscale conectado, caso seja utilizado o banco disponibilizado para o teste

### 1. Acessar a pasta da API

```bash
cd EstoqueVeiculos.Api
```

### 2. Restaurar as dependências

```bash
dotnet restore
```

### 3. Executar a API

```bash
dotnet run
```

Durante o desenvolvimento, a API foi executada em:

```text
http://localhost:5124
```

A documentação da API pode ser consultada através do Swagger em:

```text
http://localhost:5124/swagger
```
---

## Como executar o frontend

### Pré-requisitos

- Node.js
- npm
- Backend em execução

### 1. Acessar a pasta do frontend

A partir da raiz do projeto:

```bash
cd frontend
```

### 2. Instalar as dependências

```bash
npm install
```

### 3. Executar a aplicação

```bash
npm run dev
```

Durante o desenvolvimento, o frontend foi executado em:

```text
http://localhost:5173
```

---

## Decisões tomadas

### Acesso a dados com ADO.NET

O acesso ao banco Oracle foi implementado utilizando **ADO.NET puro**, através do pacote `Oracle.ManagedDataAccess.Core`.

Foram utilizados principalmente:

- `OracleConnection`
- `OracleCommand`
- `OracleDataReader`

### Organização do backend

O backend foi organizado utilizando separação de responsabilidades:

```text
Controller
    ↓
Service
    ↓
Repository
    ↓
ADO.NET
    ↓
Oracle
```

**Controllers**

Responsáveis por receber as requisições HTTP e disponibilizar os endpoints da API.

**Services**

Responsáveis pelas regras de negócio, validações e coordenação das operações.

**Repositories**

Responsáveis pelo acesso ao banco de dados e execução das queries SQL utilizando ADO.NET.

**DTOs**

Utilizados para representar os dados esperados nas diferentes operações da API, evitando expor diretamente todos os campos dos modelos.

---

### Tratamento de erros

A API possui um middleware global para tratamento de exceções.

São tratados cenários como:

- dados inválidos;
- recurso não encontrado;
- erros de acesso ao Oracle;
- erros inesperados.

As regras de negócio ficam centralizadas no backend e o frontend apresenta ao usuário as mensagens retornadas pela API.

-