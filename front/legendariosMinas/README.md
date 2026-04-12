# Legendários Minas

Plataforma web para gestão do grupo **Legendários Minas** — cadastro de senderistas, inscrições em eventos, check-ins, pagamentos (MercadoPago), área administrativa e ações sociais.

## Tech Stack

| Camada | Tecnologia |
|--------|------------|
| **Frontend** | Angular 14 · Angular Material · PO-UI · Bootstrap 5 |
| **Backend** | ASP.NET Core 5 · Dapper · AutoMapper · Serilog |
| **Banco** | MySQL (MySqlConnector) |
| **Auth** | JWT Bearer Token (emissão + guard no front) |
| **Pagamentos** | MercadoPago SDK |
| **Infra** | Docker · Nginx (front) · Kestrel (back) · Let's Encrypt |

## Estrutura do Projeto

```
legendarios/
├── front/legendariosMinas/   # Angular SPA
│   ├── src/app/
│   │   ├── pages/            # Módulos de página (Home, Admin, Cadastro, Eventos…)
│   │   ├── services/         # Serviços HTTP (auth, eventos, inscrições, checkins…)
│   │   ├── guards/           # AuthGuard (rotas protegidas)
│   │   ├── interceptors/     # JWT interceptor
│   │   ├── header/ footer/   # Layout compartilhado
│   │   └── app-routing.module.ts
│   ├── Dockerfile
│   └── nginx.conf
│
└── back/legendarios_API/
    └── legendarios_API/      # ASP.NET Core API
        ├── Controllers/      # Anuncios, Auth, Checkins, Dashboard, Eventos,
        │                     # Inscricoes, Legendarios, Login, Pagamentos,
        │                     # Relatorios, Voluntarios
        ├── Entity/           # Modelos de domínio
        ├── DTO/              # Data Transfer Objects
        ├── Repository/       # Acesso a dados (Dapper)
        ├── Service/          # Regras de negócio
        ├── Middleware/       # GlobalExceptionMiddleware
        ├── Migrations/       # SQL de schema
        ├── Dockerfile
        └── Startup.cs
```

## Rotas Principais (Frontend)

| Rota | Componente | Protegida |
|------|-----------|-----------|
| `/home` | Home | Não |
| `/cadastrar-senderista` | Cadastro de Senderistas | Não |
| `/pre-cadastro` | Pré-Cadastro | Não |
| `/acoes-sociais` | Ações Sociais | Não |
| `/login-adm` | Login Admin | Não |
| `/home-adm` | Painel Administrativo | Sim |
| `/editar-legendario` | Editar Legendário | Sim |
| `/status-pagamento` | Status de Pagamento | Não |

## Pré-requisitos

- **Node.js** (LTS) e **npm**
- **Angular CLI** 14.x (`npm i -g @angular/cli@14`)
- **.NET 5 SDK** (para o backend)
- **MySQL** acessível

## Desenvolvimento Local

### Frontend

```bash
cd front/legendariosMinas
npm install
ng serve
# → http://localhost:4200
```

### Backend

```bash
cd back/legendarios_API/legendarios_API
dotnet restore
dotnet run
# → https://localhost:5001  /  http://localhost:5000
# Swagger: /swagger
```

> Configure a connection string em `appsettings.Development.json`.

## Build & Deploy (Docker)

### Frontend

```bash
docker build -t legendarios-front ./front/legendariosMinas
docker run -p 80:80 -p 443:443 legendarios-front
```

### Backend

```bash
docker build -t legendarios-api ./back/legendarios_API/legendarios_API
docker run -p 80:80 -p 443:443 legendarios-api
```

## Testes

```bash
# Frontend (Karma + Jasmine)
cd front/legendariosMinas
ng test
```

## API (Swagger)

Com o backend rodando, acesse `/swagger` para explorar e testar os endpoints.
