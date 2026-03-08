# PerfumeStore API — Documentação de Endpoints

> **Base URL (desenvolvimento):** `http://localhost:<porta>`  
> **Formato:** JSON  
> **Autenticação:** JWT Bearer Token — inclua o header em todas as rotas protegidas:
> ```
> Authorization: Bearer <token>
> ```

---

## Índice
1. [Autenticação](#1-autenticação)
2. [Clientes](#2-clientes)
3. [Produtos](#3-produtos)
4. [Estoque](#4-estoque)
5. [Vendas](#5-vendas)
6. [Enums de Referência](#6-enums-de-referência)
7. [Formato de Resposta Padrão](#7-formato-de-resposta-padrão)

---

## 1. Autenticação

Base: `/api/auth`  
🔓 Rotas públicas (sem token), exceto logout.

---

### `POST /api/auth/register`
Cria um novo usuário no sistema.

**Body (JSON):**
```json
{
  "nome": "João Silva",
  "email": "joao@email.com",
  "senha": "minimo6caracteres"
}
```

**Respostas:**
| Status | Descrição |
|--------|-----------|
| `201 Created` | Usuário criado. Retorna token JWT. |
| `400 Bad Request` | Validação falhou ou e-mail já cadastrado. |

**Resposta de sucesso (`201`):**
```json
{
  "success": true,
  "data": "<jwt_token>",
  "message": "Usuário registrado com sucesso.",
  "errors": []
}
```

---

### `POST /api/auth/login`
Autentica um usuário e retorna o token JWT.

**Body (JSON):**
```json
{
  "email": "joao@email.com",
  "senha": "minimo6caracteres"
}
```

**Respostas:**
| Status | Descrição |
|--------|-----------|
| `200 OK` | Login realizado. Retorna token JWT. |
| `401 Unauthorized` | Credenciais inválidas. |
| `400 Bad Request` | Dados ausentes ou mal formatados. |

**Resposta de sucesso (`200`):**
```json
{
  "success": true,
  "data": "<jwt_token>",
  "message": "Login realizado com sucesso.",
  "errors": []
}
```

> **Uso do token:** armazene `data` e envie como `Authorization: Bearer <data>` em todas as requisições autenticadas.

---

### `POST /api/auth/logout` 🔒
Invalida o token JWT atual. Após o logout, o token é rejeitado mesmo que ainda não tenha expirado.

**Headers:** `Authorization: Bearer <token>`  
**Body:** nenhum

**Respostas:**
| Status | Descrição |
|--------|-----------|
| `200 OK` | Logout realizado com sucesso. |
| `401 Unauthorized` | Token ausente ou inválido. |

**Resposta de sucesso (`200`):**
```json
{
  "message": "Logout realizado com sucesso."
}
```

---

## 2. Clientes

Base: `/api/cliente`  
🔒 Todas as rotas exigem autenticação.

---

### `GET /api/cliente`
Lista clientes com paginação e filtros opcionais via query string.

**Query params (todos opcionais):**
| Parâmetro | Tipo | Descrição |
|-----------|------|-----------|
| `nome` | string | Filtra por nome (contém) |
| `cpf` | string | Filtra por CPF exato (11 dígitos) |
| `email` | string | Filtra por e-mail (contém) |
| `isAtivo` | boolean | `true` ou `false` |
| `dataCadastroInicio` | datetime | Ex: `2024-01-01` |
| `dataCadastroFim` | datetime | Ex: `2024-12-31` |
| `pageNumber` | int | Padrão: `1` |
| `pageSize` | int | Padrão: `10`, máx: `100` |

**Resposta de sucesso (`200`):**
```json
{
  "items": [
    {
      "id": 1,
      "nome": "Maria Souza",
      "cpf": "12345678901",
      "telefone": "(11) 99999-9999",
      "email": "maria@email.com",
      "isAtivo": true,
      "dataCadastro": "2024-03-01T10:00:00"
    }
  ],
  "totalCount": 50,
  "pageNumber": 1,
  "pageSize": 10
}
```

---

### `GET /api/cliente/{id}`
Retorna um cliente específico por ID.

**Respostas:**
| Status | Descrição |
|--------|-----------|
| `200 OK` | Cliente encontrado. |
| `404 Not Found` | Cliente não existe. |

**Resposta de sucesso (`200`):**
```json
{
  "id": 1,
  "nome": "Maria Souza",
  "cpf": "12345678901",
  "telefone": "(11) 99999-9999",
  "email": "maria@email.com",
  "isAtivo": true,
  "dataCadastro": "2024-03-01T10:00:00"
}
```

---

### `GET /api/cliente/{id}/detalhes`
Retorna os dados completos do cliente incluindo histórico de vendas.

**Resposta de sucesso (`200`):**
```json
{
  "id": 1,
  "nome": "Maria Souza",
  "cpf": "12345678901",
  "email": "maria@email.com",
  "telefone": "(11) 99999-9999",
  "isAtivo": true,
  "dataCadastro": "2024-03-01T10:00:00",
  "vendas": [
    {
      "id": 10,
      "dataVenda": "2024-05-10T14:30:00",
      "valorTotal": 350.00
    }
  ]
}
```

---

### `POST /api/cliente`
Cria um novo cliente.

**Body (JSON):**
```json
{
  "nome": "Carlos Lima",
  "cpf": "98765432100",
  "telefone": "(21) 98888-7777",
  "email": "carlos@email.com",
  "isAtivo": true
}
```

> `cpf`: opcional, mas se informado deve ter exatamente 11 dígitos numéricos.  
> `email`: obrigatório.

**Respostas:**
| Status | Descrição |
|--------|-----------|
| `201 Created` | Cliente criado com sucesso. |
| `400 Bad Request` | Dados inválidos. |
| `409 Conflict` | CPF já cadastrado. |

---

### `PUT /api/cliente/{id}`
Atualiza os dados de um cliente existente.

**Body (JSON):** mesmo formato do `POST /api/cliente`.

**Respostas:**
| Status | Descrição |
|--------|-----------|
| `200 OK` | Cliente atualizado. Retorna o objeto atualizado. |
| `400 Bad Request` | Dados inválidos. |
| `404 Not Found` | Cliente não encontrado. |
| `409 Conflict` | CPF já em uso por outro cliente. |

---

### `PATCH /api/cliente/{id}/desativar`
Desativa um cliente (soft delete — não exclui do banco).

**Respostas:**
| Status | Descrição |
|--------|-----------|
| `200 OK` | `{ "message": "Cliente desativado com sucesso" }` |
| `400 Bad Request` | Cliente já está desativado. |
| `404 Not Found` | Cliente não encontrado. |

---

### `DELETE /api/cliente/{id}`
Remove permanentemente um cliente. **Só funciona se o cliente não tiver vendas associadas.**

**Respostas:**
| Status | Descrição |
|--------|-----------|
| `200 OK` | `{ "message": "Cliente excluído com sucesso" }` |
| `400 Bad Request` | Cliente possui vendas — use desativar. |
| `404 Not Found` | Cliente não encontrado. |

---

## 3. Produtos

Base: `/api/produto`  
🔒 Todas as rotas exigem autenticação.

---

### `GET /api/produto`
Lista todos os produtos.

**Resposta de sucesso (`200`):**
```json
[
  {
    "id": 1,
    "nome": "Perfume X",
    "marca": "Marca Y",
    "precoCompra": 80.00,
    "precoVenda": 150.00,
    "descricao": "Fragrância floral",
    "isAtivo": true,
    "dataCadastro": "2024-01-15T00:00:00",
    "estoqueId": 2
  }
]
```

---

### `GET /api/produto/{id}`
Retorna um produto pelo ID.

**Respostas:** `200 OK` | `404 Not Found`

---

### `POST /api/produto`
Cria um novo produto.

**Body (JSON):**
```json
{
  "nome": "Perfume Z",
  "marca": "Marca W",
  "precoCompra": 60.00,
  "precoVenda": 120.00,
  "descricao": "Fragrância amadeirada",
  "isAtivo": true
}
```

**Respostas:** `201 Created` com o produto criado.

---

### `PUT /api/produto/{id}`
Atualiza um produto existente.

**Body (JSON):** mesmo formato do `POST`.

**Respostas:** `200 OK` com o produto atualizado | `404 Not Found`

---

### `DELETE /api/produto/{id}`
Remove um produto.

**Respostas:** `200 OK` com os dados do produto removido | `404 Not Found`

---

## 4. Estoque

Base: `/api/estoque`  
🔒 Todas as rotas exigem autenticação.

---

### `POST /api/estoque`
Cria um novo estoque.

**Body (JSON):**
```json
{
  "nome": "Estoque Principal",
  "descricao": "Estoque central da loja",
  "usuarioResponsavel": "admin"
}
```

**Respostas:** `201 Created` com o estoque criado.

**Resposta de sucesso:**
```json
{
  "id": 1,
  "nome": "Estoque Principal",
  "descricao": "Estoque central da loja",
  "dataCriacao": "2024-03-01T10:00:00",
  "totalItens": 0,
  "totalProdutos": 0
}
```

---

### `GET /api/estoque`
Lista todos os estoques.

**Resposta:** array de objetos `EstoqueResponse` (mesmo formato acima).

---

### `GET /api/estoque/{id}`
Retorna um estoque por ID.

**Respostas:** `200 OK` | `404 Not Found`

---

### `GET /api/estoque/produto/{produtoId}/ObterEstoquePorProduto`
Lista todos os registros de estoque de um produto específico.

**Resposta (`200`):**
```json
[
  {
    "id": 5,
    "produtoId": 3,
    "estoqueId": 1,
    "quantidade": 20,
    "quantidadeMinima": 5,
    "quantidadeMaxima": 100,
    "dataUltimaMovimentacao": "2024-03-10T08:00:00"
  }
]
```

---

### `GET /api/estoque/produto/{produtoId}/TotalProdutos`
Retorna a quantidade total disponível de um produto somando todos os estoques.

**Resposta (`200`):** `42` *(número inteiro)*

---

### `GET /api/estoque/estoque/{estoqueId}/ObterItensEstoque`
Lista todos os itens (produtos) de um estoque específico.

**Resposta:** array de `ItemEstoqueResponse` (mesmo formato de `/produto/{id}/ObterEstoquePorProduto`).

---

### `GET /api/estoque/estoque-baixo`
Lista produtos com quantidade abaixo do mínimo definido.

**Resposta:** array de `ItemEstoqueResponse`.

---

### `POST /api/estoque/MovimentarEstoque`
Registra uma movimentação (entrada, saída, ajuste etc.) de um produto num estoque.

**Body (JSON):**
```json
{
  "produtoId": 3,
  "estoqueId": 1,
  "quantidade": 10,
  "tipo": "Entrada",
  "observacoes": "Reposição semanal",
  "usuarioResponsavel": "admin"
}
```

> **`tipo`** — valores possíveis (string): `"Entrada"`, `"Saida"`, `"Transferencia"`, `"Ajuste"`, `"Perda"`, `"Devolucao"`, `"Criacao"`

**Respostas:** `200 OK` | `400 Bad Request` (ex: estoque insuficiente)

---

### `POST /api/estoque/TransferirEstoque`
Transfere quantidade de um produto entre dois estoques.

**Body (JSON):**
```json
{
  "produtoId": 3,
  "estoqueOrigemId": 1,
  "estoqueDestinoId": 2,
  "quantidade": 5,
  "observacoes": "Transferência para filial",
  "usuarioResponsavel": "admin"
}
```

**Respostas:** `200 OK` | `400 Bad Request`

---

### `GET /api/estoque/historico/produto/{produtoId}/ObterHistoricoProduto`
Retorna o histórico de movimentações de um produto.

**Query params (opcionais):**
| Parâmetro | Tipo | Descrição |
|-----------|------|-----------|
| `estoqueId` | int | Filtra por estoque específico |
| `dataInicio` | datetime | Ex: `2024-01-01` |
| `dataFim` | datetime | Ex: `2024-12-31` |

**Resposta (`200`):**
```json
[
  {
    "id": 10,
    "itemEstoqueId": 5,
    "quantidade": 10,
    "tipo": "Entrada",
    "observacoes": "Reposição semanal",
    "usuarioResponsavel": "admin"
  }
]
```

---

## 5. Vendas

Base: `/api/venda`  
🔒 Todas as rotas exigem autenticação.

---

### `POST /api/venda/CadastrarVenda`
Cria uma nova venda com status **Pendente**. O pagamento é registrado depois via `FinalizarVenda`.

**Body (JSON):**
```json
{
  "clienteId": 1,
  "estoqueId": 1,
  "itens": [
    {
      "produtoId": 3,
      "quantidade": 2,
      "precoUnitario": 150.00
    }
  ],
  "desconto": 10.00,
  "observacoes": "Cliente VIP",
  "usuarioVendedor": "vendedor01"
}
```

> `precoUnitario`: opcional — se omitido, usa o preço cadastrado no produto.

**Respostas:**
| Status | Descrição |
|--------|-----------|
| `201 Created` | Venda criada. Retorna os detalhes da venda. |
| `400 Bad Request` | Estoque insuficiente ou dados inválidos. |

**Resposta de sucesso (`201`):**
```json
{
  "id": 15,
  "clienteId": 1,
  "dataVenda": "2024-03-08T10:30:00",
  "valorTotal": 290.00,
  "desconto": 10.00,
  "status": "Pendente",
  "observacoes": "Cliente VIP",
  "usuarioVendedor": "vendedor01",
  "itensVenda": [
    {
      "id": 20,
      "produtoId": 3,
      "nomeProduto": "Perfume X",
      "quantidade": 2,
      "precoUnitario": 150.00,
      "subtotal": 300.00
    }
  ]
}
```

---

### `GET /api/venda/ObterVendaPorId/{id}`
Retorna os detalhes completos de uma venda.

**Respostas:** `200 OK` | `404 Not Found`

**Resposta:** mesmo formato do `POST CadastrarVenda`.

---

### `GET /api/venda/ObterVendas`
Lista todas as vendas.

**Resposta (`200`):**
```json
[
  {
    "id": 15,
    "dataVenda": "2024-03-08T10:30:00",
    "valorTotal": 290.00,
    "desconto": 10.00,
    "status": "Pendente",
    "observacoes": "Cliente VIP",
    "usuarioVendedor": "vendedor01"
  }
]
```

---

### `PUT /api/venda/FinalizarVenda?vendaId={id}`
Finaliza uma venda pendente registrando os pagamentos.  
O total dos pagamentos deve cobrir o valor da venda.

**Query param:** `vendaId` (int, obrigatório)  
**Body (JSON):** array de pagamentos

```json
[
  {
    "valorPago": 290.00,
    "formaPagamento": "PIX",
    "dataPagamento": "2024-03-08T10:35:00",
    "observacoes": "Pago via app"
  }
]
```

> **`formaPagamento`** — valores possíveis (string): `"Dinheiro"`, `"CartaoCredito"`, `"CartaoDebito"`, `"PIX"`, `"Crediario"`  
> `dataPagamento`: opcional — usa `DateTime.Now` se omitido.

**Respostas:** `200 OK` com os dados da venda finalizada | `400 Bad Request`

---

### `PUT /api/venda/CancelarVenda?id={id}&motivo={motivo}`
Cancela uma venda.

**Query params:**
| Parâmetro | Tipo | Obrigatório | Descrição |
|-----------|------|-------------|-----------|
| `id` | int | ✅ | ID da venda |
| `motivo` | string | ✅ | Motivo do cancelamento |
| `usuarioResponsavel` | string | ❌ | Usuário que cancelou |

**Respostas:** `200 OK` | `400 Bad Request`

---

### `POST /api/venda/ValidarEstoque`
Valida se o estoque possui todos os produtos necessários antes de criar a venda.

**Body (JSON):**
```json
{
  "itens": [
    {
      "produtoId": 3,
      "quantidade": 2
    }
  ],
  "estoqueId": 1
}
```

**Respostas:**
| Status | Descrição |
|--------|-----------|
| `200 OK` | `{ "mensagem": "Estoque validado com sucesso. Todos os produtos estão disponíveis." }` |
| `400 Bad Request` | `{ "erro": "Produto X sem estoque suficiente." }` |

---

## 6. Enums de Referência

### `StatusVenda`
| Valor (string) | Valor (int) | Descrição |
|---|---|---|
| `"Pendente"` | `1` | Venda criada, aguardando pagamento |
| `"Finalizada"` | `2` | Pagamento registrado |
| `"Cancelada"` | `3` | Venda cancelada |

### `TipoFormaPagamento`
| Valor (string) | Valor (int) |
|---|---|
| `"Dinheiro"` | `1` |
| `"CartaoCredito"` | `2` |
| `"CartaoDebito"` | `3` |
| `"PIX"` | `4` |
| `"Crediario"` | `5` |

### `TipoMovimentacao`
| Valor (string) | Valor (int) |
|---|---|
| `"Entrada"` | `0` |
| `"Saida"` | `1` |
| `"Transferencia"` | `2` |
| `"Ajuste"` | `3` |
| `"Perda"` | `4` |
| `"Devolucao"` | `5` |
| `"Criacao"` | `6` |

### `RoleUsuario`
| Valor (string) | Valor (int) |
|---|---|
| `"ADMIN"` | `1` |
| `"USER"` | `2` |

> **Nota:** A API serializa enums como **strings** (ex: `"PIX"`, `"Pendente"`). Não envie o valor numérico.

---

## 7. Formato de Resposta Padrão

A maioria dos endpoints retorna um envelope `OperationResult<T>`:

```json
{
  "success": true,
  "data": { ... },
  "message": "Mensagem descritiva",
  "errors": []
}
```

Em caso de falha:
```json
{
  "success": false,
  "data": null,
  "message": "",
  "errors": ["Mensagem de erro detalhada"]
}
```

### Códigos HTTP utilizados
| Código | Quando é retornado |
|--------|--------------------|
| `200 OK` | Consulta ou operação realizada com sucesso |
| `201 Created` | Recurso criado (POST de cadastro) |
| `400 Bad Request` | Dados inválidos, validação falhou |
| `401 Unauthorized` | Token ausente, expirado ou revogado |
| `404 Not Found` | Recurso não encontrado |
| `409 Conflict` | Conflito de dados (ex: CPF duplicado) |
| `500 Internal Server Error` | Erro inesperado no servidor |

---

## Fluxo de Uso Sugerido

### Fluxo de Venda Completo
```
1. POST /api/auth/login              → obtém token
2. POST /api/venda/ValidarEstoque    → confirma disponibilidade
3. POST /api/venda/CadastrarVenda    → cria venda (status: Pendente), salva o id retornado
4. PUT  /api/venda/FinalizarVenda    → registra pagamento (status: Finalizada)
```

### Fluxo de Cancelamento
```
1. PUT /api/venda/CancelarVenda?id={id}&motivo=Desistência
```
