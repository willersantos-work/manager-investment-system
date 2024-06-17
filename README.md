# Sistema de Gestão de Portfólio de Investimentos - Backend

## Sumário

1. [Introdução](#introdução)
2. [Pré-requisitos](#pré-requisitos)
3. [Primeiros passos](#primeiros-passos)
   - [Variáveis de ambiente](#variáveis-de-ambiente)
   - [Rodando o projeto](#rodando-o-projeto)
4. [Estrutura](#estrutura)
5. [Planejamento do projeto](#planejamento-do-projeto)
6. [Métodos da API](#métodos-da-api)
7. [Dependências do projeto](#dependências-do-projeto)
8. [Autor](#autor)


## Introdução

Esse projeto visa o desenvolvimento de um sistema para a gestão de investimentos, logo, fazer com que os administradores possam gerenciar os produtos financeiros disponíveis e que os clientes comprem, vendam e acompanhem seus investimentos.


## Pré-requisitos

Para rodar e dar build do projeto localmente você precisará de algumas ferramentas:

-   Instale [Visual Studio](https://visualstudio.microsoft.com/pt-br/downloads/)
-   Instale [Docker](https://www.docker.com/products/docker-desktop/)


## Primeiros passos

### Variáveis de ambiente

```json
  "ConnectionStrings": {
    "DefaultConnection": "String de conexão com banco" # (ex.: Host=localhost;Port=5433;Database=postgres;Username=admin;Password=admin)
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ProjectName": "Nome do projeto", # (InvestManagerSysten),
  "TokenConfig": {
    "SecretToken": "Segredo para geração do token",
    "TokenValidityDays": 1 # Quantidade de dias até expirar o token
  },
  "EmailSettings": {
    "From": "Email outlook",
    "Password": "Senha do email",
    "Host": "Host de envio de email", # (smtp-mail.outlook.com)
    "Port": 587 # Porta default para envio pelo outlook
  }
```

### Rodando o projeto

Antes de rodar o projeto, configure as variáveis de ambiente no arquivo `appsettings` na root do Back.

Para rodar o projeto em modo de desenvolvimento:

1. Crie um container de banco de dados:
   ```bash
   docker run --name investManagerSystemDB -e POSTGRES_USER=$USER -e POSTGRES_PASSWORD=$PASSWORD -e POSTGRES_DB=$DATABASE_NAME -p 5433:5432 -d postgres:15.7
   ```

2. No `Console do gerenciador de pacotes`, rode as migrations:
   ```bash
   Database-Update
   ```

3. Aperte f5 ou clique em `http` na barra de ferramentas superior.

Aguarde um tempo até finalizar a compilação, após isso abrirá o servidor [http://localhost:5221/](http://localhost:5221/) apresentando o Swagger.


## Estrutura
-   **Auth**: Configuração de autenticação, como middleware de autenticação, contexto do usuário e decorator de autorização;
-   **Controllers**: Camada de controllers da aplicação;
-   **Enums**: Enums utilizados;
-   **Global**: Configuração e utilities utilizadas globalmente no projeto:
  -   **Configs**: Estrutura para configs que são utilizadas no projeto, das quais saiem do appsettings;
  -   **Database**: Configuração para conexão com banco e definição de entidades;
  -   **Helpers**: Utilitários globais do projeto;
  -   **Injection**: Configuração de principais injeções do projeto (repositório, serviço, etc.);
  -   **Mapper**: Mapeador utilizando autoMapper entre DTO e entidades;
  -   **Startup.cs**: Centralizador para inicialização e injeção de dependências no projeto.
-   **Interfaces**: DTO's, para transferência de conteúdo entre diferentes camadas e funções;
-   **Models**: Definição das entidades;
-   **Repositories**: Camada de repositório, camada mais próxima ao banco de dados;
-   **Services**: Camada de serviço, onde estarão as regras de negócio, tendo no máximo 1 repositório para cada serviço;
-   **TaskScheduler**: Tasks a serem executadas no projetos (tarefas diárias).


## Planejamento do projeto

O planejamento inicial do projeto e o DER podem ser acessados pelos seguintes link:
-   Acesse o [planejamento do projeto](https://www.figma.com/board/de7v8XNWsZxyzbMcaqwHxI/Sistema-de-Gest%C3%A3o-de-Portf%C3%B3lio-de-Investimentos?node-id=2-13&t=58IncFDMJIRA0ljr-0)
-   Acesse o [DER (Diagram-Entity-Relation)](https://dbdiagram.io/d/Sistema-de-Gestao-de-Portfolio-de-Investimentos-666c8cfca179551be6ecbc89)


## Métodos da API

Os métodos que temos para a API são os seguintes:
-   _/ Admin
   -    _/ AdminAuth
      - Login Admin - POST /api/admin/auth/login
      - Registro de Admin - POST /api/admin/auth/register
   -    _/ FinancerProduct
      - Criação de produto - POST /api/admin/financer-product
      - Listagem de produtos - GET /api/admin/financer-product
      - Visualização de produto específico - GET /api/admin/financer-product/{id}
      - Atualização de produto - PUT /api/admin/financer-product/{id}
      - Remoção de produto - DELETE /api/admin/financer-product/{id}
-   _/ Client
   -    _/ ClientAuth
      - Login Cliente: POST /api/client/auth/login
      - Registro de Cliente: POST /api/client/auth/register
   -    _/ Investment
      - Transa��o de compra - POST /api/client/investment/buy
      - Transa��o de venda - POST /api/client/investment/sell
      - Visualiza��o de investimento do cliente - GET /api/client/investment
      - Visualiza��o de investimento - GET /api/client/investment/{id}
      - Visualiza��o de extrato de transa��es - GET /api/client/investment/{id}/statement

---

#### **AdminAuthController**

---
##### **Login Admin**
POST /api/admin/auth/login

Body
```json
{
    "Email": "email.exemplo@dominio.com",
    "Password": "abc@123"
}
```

Response 200 OK
```json
{
    "user": {
        "id": 1,
        "fullName": "First Last",
        "email": "email.exemplo@dominio.com",
        "type": "ADMIN"
    },
    "accessToken": "TOKEN"
}
```
---
##### **Registro de Admin**
POST /api/admin/auth/register

Body
```json
{
    "Email": "email.exemplo@dominio.com",
    "Password": "abc@123",
    "FullName": "First Last"
}
```

Response 201 CREATED
```json
{
    "id": 1,
    "fullName": "First Last",
    "email": "email.exemplo@dominio.com",
    "type": "ADMIN"
}
```
---

#### **FinancerProductController**

---
##### **Cria��o de produto**
POST /api/admin/financer-product

Body
```json
{
    "Name": "Ações",
    "Description": "Ações na bolsa",
    "Type": "Stocks",
    "MaturityDate": "2024-06-16",
    "InterestRate": 0.50,
    "Price": 10.50,
    "Quantity": 1000,
    "QuantityBought": 0
}
```

Header
```json
{
    "Authorization": "Bearer {{token}}"
}
```

Response 201 CREATED
```json
{
    "id": 1,
    "name": "Ações",
    "description": "Ações na bolsa",
    "type": "Stocks",
    "maturityDate": "2024-06-16",
    "interestRate": 0.50,
    "price": 10.50,
    "quantity": 1000,
    "quantityBought": 0,
    "createdDate": "2024-06-17T21:30:28.8931715Z",
    "updatedDate": "2024-06-17T21:30:28.8931715Z"
}
```
---
##### **Listagem de produtos**
GET /api/admin/financer-product

Header
```json
{
    "Authorization": "Bearer {{token}}"
}
```

Response 200 OK
```json
[
    {
        "id": 2,
        "name": "Example Product",
        "maturityDate": "2024-12-31T03:00:00Z",
        "quantity": 100,
        "quantityBought": 50,
        "price": 150.75
    },
    {
        "id": 3,
        "name": "Ações",
        "maturityDate": "2024-12-31T03:00:00Z",
        "quantity": 10000,
        "quantityBought": 950,
        "price": 150.75
    }
]
```
---
##### **Visualiza��o de produto espec�fico**
GET /api/admin/financer-product/{id}

Header
```json
{
    "Authorization": "Bearer {{token}}"
}
```

Response 200 OK
```json
{
    "id": 1,
    "name": "Ações",
    "description": "Ações na bolsa",
    "type": "Stocks",
    "maturityDate": "2024-06-16",
    "interestRate": 0.50,
    "price": 10.50,
    "quantity": 1000,
    "quantityBought": 0,
    "createdDate": "2024-06-17T21:30:28.8931715Z",
    "updatedDate": "2024-06-17T21:30:28.8931715Z"
}
```
---
##### **Atualiza��o de produto**
PUT /api/admin/financer-product/{id}

Body
```json
{
    "Description": "Ações na bolsa",
}
```

Header
```json
{
    "Authorization": "Bearer {{token}}"
}
```

Response 200 OK

---
##### **Remo��o de produto**
DELETE /api/admin/financer-product/{id}

Header
```json
{
    "Authorization": "Bearer {{token}}"
}
```

Response 204 NO-CONTENT

---

#### **ClientAuthController**

---
##### **Login Cliente**
POST /api/client/auth/login

Body
```json
{
    "Email": "email.exemplo@dominio.com",
    "Password": "abc@123"
}
```

Response 200 OK
```json
{
    "user": {
        "id": 1,
        "fullName": "First Last",
        "email": "email.exemplo@dominio.com",
        "type": "CLIENT"
    },
    "accessToken": "TOKEN"
}
```
---
##### **Registro de Cliente**
POST /api/client/auth/register

Body
```json
{
    "Email": "email.exemplo@dominio.com",
    "Password": "abc@123",
    "FullName": "First Last"
}
```

Response 201 CREATED
```json
{
    "id": 1,
    "fullName": "First Last",
    "email": "email.exemplo@dominio.com",
    "type": "CLIENT"
}
```
---

#### **InvestmentController**

---
##### **Transa��o de compra**
POST /api/client/investment/buy

Body
```json
{
    "FinancerProductName": "Ações",
    "Amount": 1000
}
```

Header
```json
{
    "Authorization": "Bearer {{token}}"
}
```

Response 200 OK

---
##### **Transa��o de venda**
POST /api/client/investment/sell

Body
```json
{
    "FinancerProductName": "Ações",
    "Amount": 1000
}
```

Header
```json
{
    "Authorization": "Bearer {{token}}"
}
```

Response 200 OK

---
##### **Visualiza��o de investimento do cliente**
GET /api/client/investment

Header
```json
{
    "Authorization": "Bearer {{token}}"
}
```

Response 200 OK
```json
[
    {
        "financerProductId": 3,
        "financerProduct": null,
        "quantity": 900,
        "purchaseDate": "2024-06-17T21:31:03.870886Z",
        "purchasePrice": 150.75,
        "salesDate": "2024-06-17T21:31:52.891063Z",
        "salesPrice": 150.75
    }
]
```
---
##### **Visualiza��o de investimento**
GET /api/client/investment/{id}

Header
```json
{
    "Authorization": "Bearer {{token}}"
}
```

Response 200 OK
```json
{
    "financerProductId": 3,
    "financerProduct": null,
    "quantity": 900,
    "purchaseDate": "2024-06-17T21:31:03.870886Z",
    "purchasePrice": 150.75,
    "salesDate": "2024-06-17T21:31:52.891063Z",
    "salesPrice": 150.75
}
```
---
##### **Visualiza��o de extrato de transa��es**
GET /api/client/investment/{id}/statement

Header
```json
{
    "Authorization": "Bearer {{token}}"
}
```

Response 200 OK
```json
[
    {
        "financerProductName": null,
        "type": 1,
        "amount": 1000,
        "price": 150.75,
        "total": 150750.00
    },
    {
        "financerProductName": null,
        "type": 0,
        "amount": 100,
        "price": 150.75,
        "total": 15075.00
    }
]
```
---


## Dependências do projeto

| Pacote                                        | Descrição                                                          |
| --------------------------------------------- | ------------------------------------------------------------------ |
| **AutoMapper**                                | Mapeamento de objetos (Entidade->Dto).                             |
| **BCrypt.Net-Next**                           | Hashing de senhas com BCrypt.                                      |
| **Coravel**                                   | Task scheduler.                                                    |
| **Microsoft.EntityFrameworkCore**             | ORM para trabalhar com bancos de dados relacionais.                |
| **Microsoft.EntityFrameworkCore.Design**      | Auxiliar de design para EF Core.                                   |
| **Microsoft.EntityFrameworkCore.Tools**       | CLI para EF Core.                                                  |
| **Microsoft.Extensions.Logging**              | Extensões de logs                                                  |
| **Npgsql**                                    | Provider para conexão PostgreSQL com database.                     |
| **Npgsql.EntityFrameworkCore.PostgreSQL**     | Provider PostgreSQL para EF Core.                                  |
| **Swashbuckle.AspNetCore**                    | Ferramenta para gerar documentação Swagger para APIs.              |
| **Swashbuckle.AspNetCore.Swagger**            | Componente Swagger para Swashbuckle.                               |
| **Swashbuckle.AspNetCore.SwaggerGen**         | Componente SwaggerGen para Swashbuckle.                            |
| **System.IdentityModel.Tokens.Jwt**           | Biblioteca para Manipulação de JWT.                                |


## Autor

-   Nome: Willer Santos
-   Local: São Paulo, SP
-   Formado: Engenheiro Químico, Escola Politécnica da USP
