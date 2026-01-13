<div align="center">

![Version](https://img.shields.io/badge/version-0.1.0-blue.svg?cacheSeconds=2592000)
![.NET](https://img.shields.io/badge/.NET-9.0-purple.svg)
![Entity Framework](https://img.shields.io/badge/Entity%20Framework-Core-orange.svg)
![Docker](https://img.shields.io/badge/Docker-Supported-blue.svg)
![Status](https://img.shields.io/badge/status-active-success.svg)

**Sistema de gerenciamento de Pedidos, Produtos e Clientes**

*Clean Architecture • DDD • CQRS/MediatR • Unit of Work • Generic Repository*

</div>

---

## 📋 Índice

- [📊 Visão Geral](#-visão-geral)
- [🐳 Docker – Como Rodar o Projeto](#-docker--como-rodar-o-projeto)
- [🏪 ProdutoService](#-produtoservice)
- [👥 ClienteService](#-clienteservice)
- [🧾 PedidoService](#-pedidoservice)
- [🚀 Tecnologias](#-tecnologias)

---

## 📊 Visão Geral

A **ApiPedidos** é uma API robusta em .NET 9 para gestão de clientes, produtos e pedidos.  
O projeto segue **Clean Architecture** com **DDD**, utiliza **CQRS com MediatR**, **AutoMapper** e **Entity Framework Core** com **Unit of Work** e **Repositórios Genéricos**.

### ✨ Principais Features

- 🛡️ **Transações Seguras** – Escritas via Unit of Work
- 🧭 **CQRS com MediatR** – Commands e Queries desacoplados
- 🔗 **Mapeamentos Limpos** – AutoMapper
- 📦 **Pedidos com Itens** – Cálculo automático de total
- 🔒 **Validações no Domínio** – Regras encapsuladas
- 🐳 **Docker Ready** – API e banco sobem juntos

---

## 🐳 Docker – Como Rodar o Projeto

O projeto está preparado para rodar **100% via Docker**, sem necessidade de instalar SQL Server ou configurar ambiente manualmente.

### 📦 Pré-requisitos

- Docker
- Docker Compose

Verifique se estão instalados:
```bash
docker --version
docker-compose --version

```

### ▶️ Subindo o ambiente (API + SQL Server)

````aiignore
docker-compose up --build
````

### 🔄 Recriando o ambiente SEM usar cache

Recomendado quando houver mudanças no Dockerfile, dependências ou migrations

````
docker-compose up -d --build --force-recreate
````

### 🛑 Parar os containers

````
docker-compose down
````

### ⚠️ Para remover containers e apagar o banco:

````
docker-compose down -v
````