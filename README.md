# Controle de Gastos Residenciais

Sistema para controle de gastos residenciais, com cadastro de pessoas, cadastro
de transações (receitas/despesas) e consulta de totais.

- **Back-end:** .NET 8 (ASP.NET Core Web API) + Entity Framework Core + SQLite
- **Front-end:** React + TypeScript (Vite)
- **Persistência:** banco de dados SQLite em arquivo (`backend/GastosResidenciais.Api/gastos.db`), criado automaticamente na primeira execução. Os dados permanecem salvos após fechar a aplicação.

## Estrutura do repositório

```
gastos-residenciais/
├── backend/
│   └── GastosResidenciais.Api/   # API .NET
│       ├── Controllers/          # Endpoints HTTP
│       ├── Services/             # Regras de negócio
│       ├── Models/                # Entidades (Person, Transaction)
│       ├── DTOs/                  # Objetos de entrada/saída da API
│       ├── Data/                  # DbContext (EF Core)
│       ├── Middleware/            # Tratamento global de exceções
│       └── Exceptions/            # Exceções de domínio
└── frontend/
    └── src/
        ├── api/                   # Cliente HTTP (fetch) para a API
        ├── components/            # Componentes React (formulários, listas, totais)
        └── App.tsx                # Composição das telas (Pessoas / Transações / Totais)
```

## Regras de negócio implementadas

- **Pessoas:** criação, listagem e remoção. Identificador (`Guid`) gerado automaticamente pelo servidor.
- **Exclusão em cascata:** ao remover uma pessoa, todas as suas transações são apagadas automaticamente (configurado via `DeleteBehavior.Cascade` no EF Core).
- **Transações:** criação e listagem. Identificador (`Guid`) gerado automaticamente. A transação deve referenciar uma pessoa já existente (validado no backend).
- **Menores de idade:** pessoas com menos de 18 anos só podem ter **despesas** cadastradas; qualquer tentativa de cadastrar uma **receita** para um menor é rejeitada pela API (HTTP 400), e a opção também é desabilitada no formulário do front-end como reforço de UX.
- **Consulta de totais:** lista cada pessoa com total de receitas, total de despesas e saldo (receita − despesa), seguida do total geral (soma de todas as pessoas).

## Como executar

### Pré-requisitos
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Node.js 18+](https://nodejs.org/)

### Back-end (API)

```bash
cd backend/GastosResidenciais.Api
dotnet restore
dotnet run
```

A API sobe em `http://localhost:5000` e o Swagger fica disponível em
`http://localhost:5000/swagger`. Na primeira execução, o arquivo `gastos.db`
é criado automaticamente na pasta do projeto (persistência local).

### Front-end (React)

Em outro terminal:

```bash
cd frontend
npm install
npm run dev
```

A aplicação abre em `http://localhost:5173` e consome a API em
`http://localhost:5000/api` (configurado em `frontend/src/api/api.ts`).

## Endpoints da API

| Método | Rota                | Descrição                                   |
|--------|---------------------|----------------------------------------------|
| GET    | `/api/people`        | Lista todas as pessoas                        |
| POST   | `/api/people`         | Cadastra uma nova pessoa                     |
| DELETE | `/api/people/{id}`    | Remove uma pessoa (e suas transações)        |
| GET    | `/api/transactions`   | Lista todas as transações                     |
| POST   | `/api/transactions`   | Cadastra uma nova transação                  |
| GET    | `/api/totals`         | Retorna totais por pessoa + total geral      |
