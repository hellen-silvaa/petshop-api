# PetShopApi

API RESTful para gestão de um pet shop, desenvolvida em **C# / .NET 10** com **Entity Framework Core**, como parte do CP5 da disciplina de C# Software Development.

## Integrantes do grupo

| Nome              | RM     |
|-------------------|--------|
| Alexia Ramalho    | 558385 |
| Enzo Real         | 557943 |
| Gustavo Pasquini  | 555454 |
| Hellen Silva      | 559008 |
| Lorenzo Acquesta  | 557397 |


## Contexto do projeto

O **PetShopApi** resolve a gestão operacional de um pet shop: cadastro de tutores (clientes) e seus pets, agendamento de serviços (banho, tosa, consulta, vacina, hospedagem), catálogo de produtos e pedidos de compra com baixa automática de estoque.

É destinado à equipe interna do pet shop (atendentes/administradores) que precisa, em um único sistema:
- Cadastrar clientes e seus animais;
- Agendar e acompanhar o status de serviços prestados;
- Manter o catálogo de produtos e o nível de estoque;
- Registrar pedidos de produtos, com desconto automático do estoque e possibilidade de cancelamento (que devolve o estoque).

## Banco de dados

**SQLite**, via `Microsoft.EntityFrameworkCore.Sqlite`. O banco é um arquivo local (`petshop.db`), criado e migrado automaticamente na inicialização da aplicação — não é necessário instalar nenhum servidor de banco de dados.

## Arquitetura

Estrutura em camadas dentro de `src/PetShopApi`:

```
Controllers/   -> endpoints da API (camada de apresentação)
Services/      -> regras de negócio (interfaces + Services/Impl)
Data/          -> AppDbContext (EF Core)
Entities/      -> entidades mapeadas (+ Entities/Enums)
Dtos/          -> objetos de requisição/resposta
Middleware/    -> tratamento global de exceções
Exceptions/    -> exceções de domínio (NotFound, BusinessRule)
Migrations/    -> migrations do EF Core
```

## Como rodar o projeto localmente

### Pré-requisitos
- [.NET SDK 10](https://dotnet.microsoft.com/download)

### Passos

```bash
# 1. Entrar na pasta do projeto
cd src/PetShopApi

# 2. Restaurar dependências
dotnet restore

# 3. (Opcional) aplicar as migrations manualmente
#    A aplicação já aplica as migrations automaticamente ao iniciar,
#    mas é possível rodar manualmente com o EF Core CLI:
dotnet tool install --global dotnet-ef   # necessário apenas na primeira vez
dotnet ef database update

# 4. Rodar a aplicação
dotnet run
```

A API sobe por padrão em `http://localhost:5000` (ou na porta definida em `Properties/launchSettings.json` / variável `ASPNETCORE_URLS`).

### Documentação interativa (Swagger)

Com a aplicação rodando, acesse:

```
http://localhost:<porta>/swagger
```

Todos os endpoints podem ser testados diretamente por ali.

### Migration documentada

A migration inicial `InitialCreate` (em `src/PetShopApi/Migrations/`) cria as tabelas `Tutores`, `Pets`, `Agendamentos`, `Produtos`, `Pedidos` e `PedidoItens`, com chaves estrangeiras e índices (ex.: e-mail único de tutor). Para gerar uma nova migration após alterar uma entidade:

```bash
dotnet ef migrations add NomeDaMigration
```

## Endpoints disponíveis

Todas as rotas são versionadas sob `/api/v1`.

### Tutores (`/api/v1/tutores`)

| Método | Rota                  | Descrição                          |
|--------|------------------------|-------------------------------------|
| GET    | `/api/v1/tutores`      | Lista todos os tutores              |
| GET    | `/api/v1/tutores/{id}` | Busca um tutor pelo id              |
| POST   | `/api/v1/tutores`      | Cadastra um novo tutor              |
| PUT    | `/api/v1/tutores/{id}` | Atualiza um tutor existente         |
| DELETE | `/api/v1/tutores/{id}` | Remove um tutor                     |

### Pets (`/api/v1/pets`)

| Método | Rota                                | Descrição                                   |
|--------|--------------------------------------|-----------------------------------------------|
| GET    | `/api/v1/pets`                       | Lista pets (filtro opcional `?tutorId=`)      |
| GET    | `/api/v1/pets/{id}`                  | Busca um pet pelo id                          |
| POST   | `/api/v1/pets`                       | Cadastra um novo pet                          |
| PUT    | `/api/v1/pets/{id}`                  | Atualiza um pet existente                     |
| DELETE | `/api/v1/pets/{id}`                  | Remove um pet                                 |

### Agendamentos (`/api/v1/agendamentos`)

| Método | Rota                                        | Descrição                                        |
|--------|-----------------------------------------------|----------------------------------------------------|
| GET    | `/api/v1/agendamentos`                        | Lista agendamentos (filtro opcional `?petId=`)     |
| GET    | `/api/v1/agendamentos/{id}`                   | Busca um agendamento pelo id                       |
| POST   | `/api/v1/agendamentos`                        | Cria um novo agendamento                           |
| PUT    | `/api/v1/agendamentos/{id}`                   | Atualiza um agendamento existente                  |
| PATCH  | `/api/v1/agendamentos/{id}/status`            | Atualiza apenas o status do agendamento            |
| DELETE | `/api/v1/agendamentos/{id}`                   | Remove um agendamento                              |

### Produtos (`/api/v1/produtos`)

| Método | Rota                     | Descrição                        |
|--------|---------------------------|------------------------------------|
| GET    | `/api/v1/produtos`        | Lista todos os produtos            |
| GET    | `/api/v1/produtos/{id}`   | Busca um produto pelo id           |
| POST   | `/api/v1/produtos`        | Cadastra um novo produto           |
| PUT    | `/api/v1/produtos/{id}`   | Atualiza um produto existente      |
| DELETE | `/api/v1/produtos/{id}`   | Remove um produto                  |

### Pedidos (`/api/v1/pedidos`)

| Método | Rota                                | Descrição                                                              |
|--------|--------------------------------------|--------------------------------------------------------------------------|
| GET    | `/api/v1/pedidos`                    | Lista pedidos (filtro opcional `?tutorId=`)                              |
| GET    | `/api/v1/pedidos/{id}`               | Busca um pedido pelo id                                                   |
| POST   | `/api/v1/pedidos`                    | Cria um pedido; dá baixa no estoque dos produtos                         |
| PATCH  | `/api/v1/pedidos/{id}/status`        | Atualiza o status do pedido (cancelar devolve o estoque)                 |
| DELETE | `/api/v1/pedidos/{id}`               | Remove um pedido                                                          |

### Códigos de status utilizados
- `200 OK` — consultas bem-sucedidas
- `201 Created` — criação bem-sucedida (com header `Location` apontando para o recurso)
- `204 No Content` — atualização/remoção bem-sucedida
- `400 Bad Request` — dados inválidos ou violação de regra de negócio (ex.: estoque insuficiente)
- `404 Not Found` — recurso inexistente

## Evidências de teste

As capturas de tela demonstrando cada endpoint em funcionamento (via Swagger) estão na pasta [`docs/evidencias`](docs/evidencias).

### Visão geral dos endpoints

![Visão geral 1](docs/evidencias/GERAL1.png)

![Visão geral 2](docs/evidencias/GERAL2.png)

### Tutores

**GET** `/api/v1/tutores`

![GET tutores](docs/evidencias/TUTORESGET.png)

**GET** `/api/v1/tutores/{id}`

![GET tutor por id](docs/evidencias/TUTORESGET1.png)

**POST** `/api/v1/tutores`

![POST tutor](docs/evidencias/TUTORESPOST.png)

**PUT** `/api/v1/tutores/{id}`

![PUT tutor](docs/evidencias/TUTORESPUT.png)

**DELETE** `/api/v1/tutores/{id}`

![DELETE tutor](docs/evidencias/TUTORESDELETE.png)

### Produtos

**GET** `/api/v1/produtos`

![GET produtos](docs/evidencias/PRODUTOSGET.png)

**GET** `/api/v1/produtos/{id}`

![GET produto por id](docs/evidencias/PRODUTOSGET1.png)

**POST** `/api/v1/produtos`

![POST produto](docs/evidencias/PRODUTOSPOST.png)

**PUT** `/api/v1/produtos/{id}`

![PUT produto](docs/evidencias/PRODUTOSPUT.png)

**DELETE** `/api/v1/produtos/{id}`

![DELETE produto](docs/evidencias/PRODUTOSDELETE.png)

### Pets

**GET** `/api/v1/pets`

![GET pets](docs/evidencias/PETSGET.png)

**GET** `/api/v1/pets/{id}`

![GET pet por id](docs/evidencias/PETSGET1.png)

**POST** `/api/v1/pets`

![POST pet](docs/evidencias/PETSPOST.png)

**PUT** `/api/v1/pets/{id}`

![PUT pet](docs/evidencias/PETSPUT.png)

**DELETE** `/api/v1/pets/{id}`

![DELETE pet](docs/evidencias/PETSDELETE.png)

### Agendamentos

**GET** `/api/v1/agendamentos`

![GET agendamentos](docs/evidencias/GET.png)

**GET** `/api/v1/agendamentos/{id}`

![GET agendamento por id](docs/evidencias/GET1.png)

**POST** `/api/v1/agendamentos`

![POST agendamento](docs/evidencias/POST.png)

**PUT** `/api/v1/agendamentos/{id}`

![PUT agendamento](docs/evidencias/PUT.png)

**PATCH** `/api/v1/agendamentos/{id}/status`

![PATCH status do agendamento](docs/evidencias/PATCH.png)

**DELETE** `/api/v1/agendamentos/{id}`

![DELETE agendamento](docs/evidencias/DELETE.png)

### Pedidos

**GET** `/api/v1/pedidos`

![GET pedidos](docs/evidencias/PEDIDOSGET.png)

**GET** `/api/v1/pedidos/{id}`

![GET pedido por id](docs/evidencias/PEDIDOSGET1.png)

**POST** `/api/v1/pedidos`

![POST pedido](docs/evidencias/PEDIDOSPOST.png)

**PATCH** `/api/v1/pedidos/{id}/status`

![PATCH status do pedido](docs/evidencias/PEDIDOSPATCH.png)

**DELETE** `/api/v1/pedidos/{id}`

![DELETE pedido](docs/evidencias/PEDIDOSDELETE.png)
