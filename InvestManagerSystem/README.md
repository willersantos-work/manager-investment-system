# Sistema de Gest�o de Portf�lio de Investimentos - Backend

## Sum�rio

1. [Introdu��o](#introdu��o)
2. [Pr�-requisitos](#pr�-requisitos)
3. [Primeiros passos](#primeiros-passos)
   - [Vari�veis de ambiente](#vari�veis-de-ambiente)
   - [Rodando o projeto](#rodando-o-projeto)
4. [Estrutura](#estrutura)
5. [Planejamento do projeto](#planejamento-do-projeto)
6. [M�todos da API](#m�todos-da-api)
7. [Depend�ncias do projeto](#depend�ncias-do-projeto)
8. [Autor](#autor)


## Introdu��o

Esse projeto visa o desenvolvimento de um sistema para a gest�o de investimentos, logo, fazer com que os administradores possam gerenciar os produtos financeiros dispon�veis e que os clientes comprem, vendam e acompanhem seus investimentos.


## Pr�-requisitos

Para rodar e dar build do projeto localmente voc� precisar� de algumas ferramentas:

-   Instale [Visual Studio](https://visualstudio.microsoft.com/pt-br/downloads/)
-   Instale [Docker](https://www.docker.com/products/docker-desktop/)


## Primeiros passos

### Vari�veis de ambiente

```json
  "ConnectionStrings": {
    "DefaultConnection": "String de conex�o com banco" # (ex.: Host=localhost;Port=5433;Database=postgres;Username=admin;Password=admin)
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
    "SecretToken": "Segredo para gera��o do token",
    "TokenValidityDays": 1 # Quantidade de dias at� expirar o token
  },
  "EmailSettings": {
    "From": "Email outlook",
    "Password": "Senha do email",
    "Host": "Host de envio de email", # (smtp-mail.outlook.com)
    "Port": 587 # Porta default para envio pelo outlook
  }
```

### Rodando o projeto

Antes de rodar o projeto, configure as vari�veis de ambiente no arquivo `appsettings` na root do Back.

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

Aguarde um tempo at� finalizar a compila��o, ap�s isso abrir� o servidor [http://localhost:5221/](http://localhost:5221/) apresentando o Swagger.


## Estrutura
-   **Auth**: Configura��o de autentica��o, como middleware de autentica��o, contexto do usu�rio e decorator de autoriza��o;
-   **Controllers**: Camada de controllers da aplica��o;
-   **Enums**: Enums utilizados;
-   **Global**: Configura��o e utilities utilizadas globalmente no projeto:
  -   **Configs**: Estrutura para configs que s�o utilizadas no projeto, das quais saiem do appsettings;
  -   **Database**: Configura��o para conex�o com banco e defini��o de entidades;
  -   **Helpers**: Utilit�rios globais do projeto;
  -   **Injection**: Configura��o de principais inje��es do projeto (reposit�rio, servi�o, etc.);
  -   **Mapper**: Mapeador utilizando autoMapper entre DTO e entidades;
  -   **Startup.cs**: Centralizador para inicializa��o e inje��o de depend�ncias no projeto.
-   **Interfaces**: DTO's, para transfer�ncia de conte�do entre diferentes camadas e fun��es;
-   **Models**: Defini��o das entidades;
-   **Repositories**: Camada de reposit�rio, camada mais pr�xima ao banco de dados;
-   **Services**: Camada de servi�o, onde estar�o as regras de neg�cio, tendo no m�ximo 1 reposit�rio para cada servi�o;
-   **TaskScheduler**: Tasks a serem executadas no projetos (tarefas di�rias).


## Planejamento do projeto

O planejamento inicial do projeto e o DER podem ser acessados pelos seguintes link:
-   Acesse o [planejamento do projeto](https://www.figma.com/board/de7v8XNWsZxyzbMcaqwHxI/Sistema-de-Gest%C3%A3o-de-Portf%C3%B3lio-de-Investimentos?node-id=2-13&t=58IncFDMJIRA0ljr-0)
-   Acesse o [DER (Diagram-Entity-Relation)](https://dbdiagram.io/d/Sistema-de-Gestao-de-Portfolio-de-Investimentos-666c8cfca179551be6ecbc89)


## M�todos da API

Os m�todos que temos para a API s�o os seguintes:
-   _/ Admin
   -    _/ AdminAuth
      - Login Admin - POST /api/admin/auth/login
      - Registro de Admin - POST /api/admin/auth/register
   -    _/ FinancerProduct
      - Cria��o de produto - POST /api/admin/financer-product
      - Listagem de produtos - GET /api/admin/financer-product
      - Visualiza��o de produto espec�fico - GET /api/admin/financer-product/{id}
      - Atualiza��o de produto - PUT /api/admin/financer-product/{id}
      - Remo��o de produto - DELETE /api/admin/financer-product/{id}
-   _/ Client
   -    _/ ClientAuth
      - Login Cliente: POST /api/client/auth/login
      - Registro de Cliente: POST /api/client/auth/register
   -    _/ Investment
      - Transa��o de compra - POST /api/client/investments/buy
      - Transa��o de venda - POST /api/client/investments/sell
      - Visualiza��o de investimento do cliente - GET /api/client/investments
      - Visualiza��o de investimento - GET /api/client/investments/{id}
      - Visualiza��o de extrato de transa��es - GET /api/client/investments/{id}/statement

---

**AdminAuthController**

---
**Login Admin**
POST /api/admin/auth/login

Body
```json
{
    "Email": "email.exemplo@dominio.com",
    "Password": "abc@123"
}
```
---
**Registro de Admin**
POST /api/admin/auth/register

Body
```json
{
    "Email": "email.exemplo@dominio.com",
    "Password": "abc@123",
    "FullName": "First Last"
}
```

---

**FinancerProductController**

---
**Cria��o de produto**
POST /api/admin/financer-product

Body
```json
{
    "Name": "A��es",
    "Description": "A��es na bolsa",
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
---
**Listagem de produtos**
GET /api/admin/financer-product

Header
```json
{
    "Authorization": "Bearer {{token}}"
}
```
---
**Visualiza��o de produto espec�fico**
GET /api/admin/financer-product/{id}

Header
```json
{
    "Authorization": "Bearer {{token}}"
}
```
---
**Atualiza��o de produto**
PUT /api/admin/financer-product/{id}

Body
```json
{
    "Description": "A��es na bolsa",
}
```

Header
```json
{
    "Authorization": "Bearer {{token}}"
}
```
---
**Remo��o de produto**
DELETE /api/admin/financer-product/{id}

Header
```json
{
    "Authorization": "Bearer {{token}}"
}
```
---

**ClientAuthController**

---
**Login Cliente**
POST /api/client/auth/login

Body
```json
{
    "Email": "email.exemplo@dominio.com",
    "Password": "abc@123"
}
```
---
**Registro de Cliente**
POST /api/client/auth/register

Body
```json
{
    "Email": "email.exemplo@dominio.com",
    "Password": "abc@123",
    "FullName": "First Last"
}
```
---

**InvestmentController**

---
**Transa��o de compra**
POST /api/client/investments/buy

Body
```json
{
    "FinancerProductName": "A��es",
    "Amount": 1000
}
```

Header
```json
{
    "Authorization": "Bearer {{token}}"
}
```
---
**Transa��o de venda**
POST /api/client/investments/sell

Body
```json
{
    "FinancerProductName": "A��es",
    "Amount": 1000
}
```

Header
```json
{
    "Authorization": "Bearer {{token}}"
}
```
---
**Visualiza��o de investimento do cliente**
GET /api/client/investments

Header
```json
{
    "Authorization": "Bearer {{token}}"
}
```
---
**Visualiza��o de investimento**
GET /api/client/investments/{id}

Header
```json
{
    "Authorization": "Bearer {{token}}"
}
```
---
**Visualiza��o de extrato de transa��es**
GET /api/client/investments/{id}/statement

Header
```json
{
    "Authorization": "Bearer {{token}}"
}
```
---


## Depend�ncias do projeto

| Pacote                                        | Descri��o                                                          |
| --------------------------------------------- | ------------------------------------------------------------------ |
| **AutoMapper**                                | Mapeamento de objetos (Entidade->Dto).                             |
| **BCrypt.Net-Next**                           | Hashing de senhas com BCrypt.                                      |
| **Coravel**                                   | Task scheduler.                                                    |
| **Microsoft.EntityFrameworkCore**             | ORM para trabalhar com bancos de dados relacionais.                |
| **Microsoft.EntityFrameworkCore.Design**      | Auxiliar de design para EF Core.                                   |
| **Microsoft.EntityFrameworkCore.Tools**       | CLI para EF Core.                                                  |
| **Microsoft.Extensions.Logging**              | Extens�es de logs                                                  |
| **Npgsql**                                    | Provider para conex�o PostgreSQL com database.                     |
| **Npgsql.EntityFrameworkCore.PostgreSQL**     | Provider PostgreSQL para EF Core.                                  |
| **Swashbuckle.AspNetCore**                    | Ferramenta para gerar documenta��o Swagger para APIs.              |
| **Swashbuckle.AspNetCore.Swagger**            | Componente Swagger para Swashbuckle.                               |
| **Swashbuckle.AspNetCore.SwaggerGen**         | Componente SwaggerGen para Swashbuckle.                            |
| **System.IdentityModel.Tokens.Jwt**           | Biblioteca para Manipula��o de JWT.                                |


## Autor

-   Nome: Willer Santos
-   Local: S�o Paulo, SP
-   Formado: Engenheiro Qu�mico, Escola Polit�cnica da USP
