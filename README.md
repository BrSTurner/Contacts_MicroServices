
![Logo](https://sunsetti.com.br/wp-content/uploads/2020/08/06-parceiros-logo-fiap.png) 
# Contatos

[![Build and Test](https://github.com/BrSTurner/Contacts/actions/workflows/dotnet.yml/badge.svg)](https://github.com/BrSTurner/Contacts/actions/workflows/dotnet.yml)

Projeto desenvolvido em .NET 8 - Micro Services Architecture
-------------------------
- Repository Pattern
- Clean Code
- SOLID
- IoC
- Minimal API
- Unit Tests
- Micro Services
- RabbitMQ
- Mass Transit
- Grafana & Prometheus 
- Integration Tests
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


Essa estrutura promove a separação de preocupações, facilitando a manutenção, testes e evolução do sistema.
