# Sistema de Gerenciamento de Produtos Hypesoft

Sistema completo de gerenciamento de inventário desenvolvido com arquitetura moderna, autenticação via Keycloak e interface responsiva.

##  Índice

- [Sobre o Projeto](#sobre-o-projeto)
- [Funcionalidades Implementadas](#funcionalidades-implementadas)
- [Tecnologias Utilizadas](#tecnologias-utilizadas)
- [Arquitetura](#arquitetura)
- [Configuração e Instalação](#configuração-e-instalação)
- [Dificuldades Encontradas](#dificuldades-encontradas)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [API Endpoints](#api-endpoints)
- [Autenticação e Autorização](#autenticação-e-autorização)
- [Testes](#testes)

##  Sobre o Projeto

Sistema de gerenciamento de produtos desenvolvido como parte de um desafio técnico. O projeto implementa um CRUD completo de produtos e categorias, com sistema de autenticação robusto usando Keycloak, controle de acesso baseado em roles e interface moderna com Next.js.

##  Funcionalidades Implementadas

### Backend (.NET 9.0)

-  **CRUD Completo de Produtos**
  - Criação, leitura, atualização e exclusão de produtos
  - Paginação de resultados
  - Busca por nome
  - Filtro por categoria
  - Identificação de produtos com estoque baixo (<10 unidades)

-  **CRUD Completo de Categorias**
  - Gerenciamento de categorias de produtos
  - Validação de exclusão (não permite excluir categorias com produtos)
  - Listagem ordenada

-  **Dashboard com Estatísticas**
  - Total de produtos no inventário
  - Valor total do inventário
  - Contagem de itens com estoque baixo
  - Distribuição de produtos por categoria

-  **Autenticação e Autorização**
  - Integração com Keycloak
  - Autenticação via JWT
  - Controle de acesso baseado em roles (Admin, Manager, User)
  - Refresh token

-  **Arquitetura Clean**
  - Separação em camadas (API, Application, Domain, Infrastructure)
  - CQRS com MediatR
  - Repository Pattern
  - Dependency Injection

-  **Validações**
  - FluentValidation para validação de comandos
  - Validações de domínio
  - Middleware de tratamento de exceções global

-  **Logging**
  - Serilog configurado
  - Logs estruturados em arquivo e console
  - Logs detalhados de autenticação

-  **Rate Limiting**
  - Proteção contra abuso de API
  - Limitação por IP

-  **MongoDB**
  - Persistência de dados
  - Índices otimizados
  - Soft delete

-  **Health Checks**
  - Endpoint de verificação de saúde da aplicação

-  **Testes Unitários**
  - Testes do Domain Layer
  - Testes do Application Layer
  - Cobertura de validadores

### Frontend (Next.js 16 + React 19)

-  **Dashboard Interativo**
  - Estatísticas em tempo real
  - Gráficos com Recharts
  - Cards informativos
  - Tabela de produtos com estoque baixo

-  **Gerenciamento de Produtos**
  - Listagem paginada com busca e filtros
  - Formulário de criação/edição
  - Confirmação de exclusão
  - Indicador visual de estoque baixo
  - Validação com Zod

-  **Gerenciamento de Categorias**
  - Grid responsivo de categorias
  - CRUD completo
  - Validação de dependências

-  **Autenticação**
  - Login com Keycloak
  - Gestão de tokens (access + refresh)
  - Proteção de rotas
  - Logout seguro
  - Interceptor axios para injeção automática de token

-  **UI/UX Moderna**
  - Design system com shadcn/ui
  - Tailwind CSS
  - Tema dark/light
  - Componentes reutilizáveis
  - Responsivo (mobile-first)
  - Loading states
  - Toast notifications (sonner)

-  **Gestão de Estado**
  - React Query para cache e sincronização
  - Zustand para estado global de autenticação
  - Persistência de sessão

-  **TypeScript**
  - Tipagem forte em todo o código
  - Interfaces compartilhadas
  - Type safety

## 🛠️ Tecnologias Utilizadas

### Backend

- **.NET 9.0** - Framework principal
- **ASP.NET Core** - API REST
- **MongoDB** - Banco de dados NoSQL
- **Keycloak** - Servidor de autenticação
- **MediatR** - CQRS pattern
- **FluentValidation** - Validações
- **Serilog** - Logging
- **xUnit** - Testes unitários
- **Moq** - Mocking para testes
- **FluentAssertions** - Assertions nos testes
- **Swashbuckle** - Documentação OpenAPI/Swagger

### Frontend

- **Next.js 16** - Framework React
- **React 19** - Biblioteca UI
- **TypeScript** - Tipagem estática
- **Tailwind CSS** - Estilização
- **shadcn/ui** - Componentes UI
- **React Query (TanStack Query)** - Data fetching
- **Zustand** - State management
- **React Hook Form** - Gerenciamento de formulários
- **Zod** - Validação de schemas
- **Axios** - Cliente HTTP
- **Recharts** - Gráficos
- **Lucide React** - Ícones
- **Sonner** - Toast notifications

### DevOps

- **Docker** - Containerização
- **Docker Compose** - Orquestração
- **PostgreSQL** - Banco do Keycloak

##  Arquitetura

### Backend - Clean Architecture

```
backend/
├── src/
│   ├── Hypesoft.API/          # Controllers, Middlewares
│   ├── Hypesoft.Application/   # Use Cases, DTOs, Validators
│   ├── Hypesoft.Domain/        # Entities, Interfaces
│   └── Hypesoft.Infrastructure/# Repositories, Data Context
└── tests/
    ├── Hypesoft.Domain.Tests/
    ├── Hypesoft.Application.Tests/
    └── Hypesoft.API.Tests/
```

**Fluxo de Dados:**
```
Request → Controller → MediatR → Handler → Repository → MongoDB
                                    ↓
                              Validator
```

### Frontend - Feature-based

```
frontend/
├── app/                    # Pages e layouts
│   ├── auth/login/        # Página de login
│   ├── dashboard/         # Dashboard e sub-páginas
│   ├── hooks/             # Custom hooks
│   └── types/             # TypeScript types
├── components/            # Componentes reutilizáveis
│   ├── ui/               # Componentes base (shadcn)
│   ├── dashboard/        # Componentes do dashboard
│   ├── products/         # Componentes de produtos
│   ├── categories/       # Componentes de categorias
│   └── layout/           # Layout components
├── lib/                  # Utilitários e configurações
│   ├── api/             # Clientes API
│   ├── auth.ts          # Configuração NextAuth
│   ├── keycloak.ts      # Integração Keycloak
│   └── utils.ts         # Funções auxiliares
└── store/               # Zustand stores
```

## 🚀 Configuração e Instalação

### Pré-requisitos

- Docker e Docker Compose
- .NET 9.0 SDK (para desenvolvimento local)
- Node.js 20+ (para desenvolvimento local)
- Git

### 1. Clone o Repositório

```bash
git clone <repository-url>
cd Desafio-Tecnico-Hypesoft
```

### 2. Configuração com Docker (Recomendado)

```bash
# Inicie todos os serviços
docker-compose up -d

# Aguarde os serviços iniciarem (pode levar 1-2 minutos)
docker-compose ps
```

Serviços disponíveis:
- **API Backend**: http://localhost:5000
- **Keycloak**: http://localhost:8080
- **MongoDB**: localhost:27017
- **MongoDB Express**: http://localhost:8081
- **PostgreSQL**: localhost:5432

### 3. Configuração do Keycloak

Acesse http://localhost:8080 e faça login:
- **Usuário**: admin
- **Senha**: admin

O Keycloak deve ser configurado manualmente ou via script. Credenciais de teste:
- **Admin**: admin / admin123 (roles: Admin, Manager, User)
- **Manager**: manager / manager123 (roles: Manager, User)
- **User**: user / user123 (role: User)

### 4. Acesse a Aplicação

Frontend: Acesse http://localhost:3000 (se configurado)

Backend Swagger: http://localhost:5000/swagger

## ⚠️ Dificuldades Encontradas

### 1. Validação de JWT com Keycloak

**Problema Principal**: Durante a implementação da autenticação JWT com Keycloak, enfrentei diversos desafios relacionados à validação do token:

#### a) Mismatch de Audience (aud)

**Sintoma**: Token rejeitado com erro `InvalidAudience`

**Causa**: O Keycloak, por padrão, emite tokens com `aud: "account"`, mas a API esperava `aud: "hypesoft-api"`.

**Solução Implementada**:
```csharp
// Program.cs - Configuração inicial (temporária)
options.TokenValidationParameters = new TokenValidationParameters
{
    ValidateAudience = false, // Desabilitado temporariamente
    // ... outras configurações
};
```

**Solução Recomendada**: Configurar um Audience Mapper no Keycloak:
1. No cliente `hypesoft-api`: Adicionar mapper do tipo "Audience"
2. Incluir `hypesoft-api` como audience
3. Reativar validação: `ValidateAudience = true`

#### b) Issuer Configuration

**Sintoma**: Token rejeitado com erro `InvalidIssuer`

**Causa**: URL do issuer incorreta ou não incluindo o caminho completo do realm.

**Solução**:
```csharp
// Authority deve incluir o realm
Authority = "http://localhost:8080/realms/hypesoft" // ✅ Correto
// NÃO: "http://localhost:8080" // ❌ Incorreto
```

#### c) HTTPS Requirement em Desenvolvimento

**Sintoma**: Falha na validação por usar HTTP em vez de HTTPS

**Solução**:
```csharp
options.RequireHttpsMetadata = !isDevelopment; // false em dev, true em prod
```

#### d) Clock Skew

**Sintoma**: Tokens expirados rejeitados imediatamente ou aceitos após expiração

**Solução**:
```csharp
options.TokenValidationParameters = new TokenValidationParameters
{
    ClockSkew = TimeSpan.Zero // Sem margem de tolerância
};
```

### 2. Integração Frontend com Keycloak

**Problema**: Sincronização entre Zustand, localStorage e o interceptor do Axios

**Solução**: Implementação de hidratação adequada do store e sincronização manual:
```typescript
// auth-store.ts
onRehydrateStorage: () => (state) => {
  if (state?.accessToken) {
    localStorage.setItem('access_token', state.accessToken);
  }
  state.setHasHydrated(true);
}
```

### 3. Logging de Autenticação

Para facilitar o debug, implementei logging detalhado:

```csharp
options.Events = new JwtBearerEvents
{
    OnAuthenticationFailed = context =>
    {
        Log.Error("Authentication failed: {ErrorType} - {ErrorMessage}", 
            context.Exception.GetType().Name, 
            context.Exception.Message);
        return Task.CompletedTask;
    },
    OnTokenValidated = context =>
    {
        Log.Information("Token validated successfully. User: {User}", 
            context.Principal?.Identity?.Name);
        return Task.CompletedTask;
    }
};
```

### 4. CORS Issues

**Problema**: Requisições bloqueadas por CORS em desenvolvimento

**Solução**:
```csharp
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});
```

##  Estrutura do Projeto

### Backend - Camadas

**Domain Layer** (`Hypesoft.Domain`)
- Entidades de domínio (`Product`, `Category`)
- Interfaces de repositórios
- Exceções de domínio
- Lógica de negócio

**Application Layer** (`Hypesoft.Application`)
- Commands e Queries (CQRS)
- Handlers (MediatR)
- DTOs
- Validators (FluentValidation)
- Interfaces de serviços

**Infrastructure Layer** (`Hypesoft.Infrastructure`)
- Implementação de repositórios
- Contexto do MongoDB
- Configurações de infraestrutura
- Repositories In-Memory (fallback)

**API Layer** (`Hypesoft.API`)
- Controllers
- Middlewares (ExceptionHandling, CorrelationId)
- Configuração de autenticação
- Startup configuration

##  API Endpoints

### Authentication
```
POST /api/auth/login - Login (Keycloak)
```

### Products
```
GET    /api/products              - Listar produtos (paginado)
GET    /api/products/{id}         - Obter produto por ID
GET    /api/products/low-stock    - Produtos com estoque baixo
POST   /api/products              - Criar produto [Admin, Manager]
PUT    /api/products/{id}         - Atualizar produto [Admin, Manager]
DELETE /api/products/{id}         - Excluir produto [Admin]
```

### Categories
```
GET    /api/categories            - Listar categorias
GET    /api/categories/{id}       - Obter categoria por ID
POST   /api/categories            - Criar categoria [Admin, Manager]
PUT    /api/categories/{id}       - Atualizar categoria [Admin, Manager]
DELETE /api/categories/{id}       - Excluir categoria [Admin]
```

### Dashboard
```
GET    /api/dashboard/stats       - Estatísticas do dashboard
```

### Health
```
GET    /api/health                - Health check
```

##  Autenticação e Autorização

### Roles Implementadas

| Role    | Permissões                                      |
|---------|------------------------------------------------|
| Admin   | Todas as operações (CRUD completo)            |
| Manager | Criar e editar produtos/categorias            |
| User    | Apenas visualização                            |

### Fluxo de Autenticação

1. **Login**: Frontend envia credenciais para Keycloak
2. **Token**: Keycloak retorna access_token e refresh_token
3. **Armazenamento**: Tokens salvos no Zustand + localStorage
4. **Requisições**: Axios interceptor injeta token no header
5. **Validação**: Backend valida JWT e extrai roles
6. **Autorização**: Atributos `[Authorize(Roles = "...")]` protegem endpoints


Este projeto foi desenvolvido como parte de um desafio técnico


