
![Logo](https://sunsetti.com.br/wp-content/uploads/2020/08/06-parceiros-logo-fiap.png) 
# Contatos

[![Build and Test](https://github.com/BrSTurner/Contacts/actions/workflows/dotnet.yml/badge.svg)](https://github.com/BrSTurner/Contacts/actions/workflows/dotnet.yml)

Projeto desenvolvido em .NET 8 - Clean Architecture
-------------------------
- Repository Pattern
- Clean Code
- SOLID
- IoC
- Minimal API
- Unit Tests
-------------------------

Requisitos para rodar:
- .NET 8
- Rodar o comando
```bash
Update-Database
```
- SQL Server Local DB 
-------------------------




## Endpoints

#### Obter todos os contatos

```http
  GET /api/contacts
```

#### Obter contatos por DDD

```http
  GET /api/contacts/${phoneCode}
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `phoneCode`      | `int` | **Required**. Codigo de area da regiao |

#### Criar contato

```http
  POST /api/contacts
```

#### Atualizar contato

```http
  PUT /api/contacts/${contactId}
```
| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `contactId`      | `int` | **Required**. Id do Contato |

#### Deletar contato

```http
  DELETE /api/contacts/${contactId}
```
| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `contactId`      | `int` | **Required**. Id do Contato |


## Camadas do Projeto:

| Camada             | Descricao                                                                |
| ----------------- | ------------------------------------------------------------------ |
| FIAP.Contacts.WebAPI | Contém a interface de apresentação (Minimal API) e configurações da aplicação. |
| FIAP.Contacts.Application | Contém a lógica de aplicação, como serviços de aplicação, modelos, perfis e consultas. Esta camada orquestra a comunicação entre a camada de apresentação e a camada de domínio. |
| FIAP.Contacts.SharedKernel | Contém elementos comuns que podem ser compartilhados entre diferentes domínios, como constantes, objetos de domínio, enums, exceções e o padrão Unit of Work (UoW). |
| FIAP.Contacts.Domain | Contém a lógica de negócio central do sistema, incluindo entidades de domínio, repositórios e serviços de domínio.|
| FIAP.Contacts.Infrastructure | Contém a implementação da infraestrutura, como contexto de banco de dados, migrações, repositórios e extensões. Esta camada depende do domínio, mas o domínio não depende da infraestrutura. |
| FIAP.Contacts.UnitTests | Contém os testes unitários para garantir que cada componente do sistema funcione conforme esperado. |
| FIAP.Contacts.IntegrationTests | Contém os testes de integração para garantir que os componentes do sistema estão interagindo entre si como o esperado. |

Essa estrutura promove a separação de preocupações, facilitando a manutenção, testes e evolução do sistema.
