# 🛍️ Loja Roupas API - Backend

API RESTful desenvolvida em **.NET (C#)** para gerenciamento e catálogo de peças de vestuário, integrada ao banco de dados relacional **PostgreSQL (Supabase)** e hospedada no **Render**.

---

## 🛠️ Tecnologias Utilizadas

* **[.NET](https://dotnet.microsoft.com/)** - Framework principal para construção de APIs Minimal APIs de alta performance.
* **[Entity Framework Core](https://learn.microsoft.com/ef/core/)** - ORM para modelagem, mapeamento e manipulação de entidades no banco de dados.
* **[PostgreSQL (Supabase)](https://supabase.com/)** - Banco de dados relacional em nuvem.
* **[Swagger / OpenAPI](https://swagger.io/)** - Documentação e interface interativa para testes de endpoints.
* **[Render](https://render.com/)** - Plataforma de hospedagem e deploy contínuo (CI/CD).

---

## ⚙️ Configuração e Variáveis de Ambiente

Para rodar a API localmente, certifique-se de configurar a Connection String com o PostgreSQL do Supabase no arquivo `appsettings.json` (ou `appsettings.Development.json`):

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Host=SEU_HOST_SUPABASE;Port=5432;Database=postgres;Username=postgres;Password=SUA_SENHA;"
  }
}

```

---

## 🚀 Como Executar Localmente

### Pré-requisitos

* [.NET SDK 8.0+](https://dotnet.microsoft.com/download) instalado.
* Instância do PostgreSQL configurada no Supabase.

### Passo a Passo

1. Clone o repositório:

```bash
git clone [https://github.com/juliaDemartini/Loja-Back-End.git](https://github.com/juliaDemartini/Loja-Back-End.git)
cd Loja-Back-End

```

2. Restaure as dependências do projeto:

```bash
dotnet restore

```

3. Execute a aplicação:

```bash
dotnet run

```

4. Acesse a documentação Swagger interativa no navegador:

```
http://localhost:8080/swagger

```

---

## 📍 Endpoints da API

| Método | Rota | Descrição | Corpo da Requisição (Payload) | Respostas HTTP |
| --- | --- | --- | --- | --- |
| **`GET`** | `/produtos` | Retorna a lista de todas as peças cadastradas | *Nenhum* | `200 OK` |
| **`POST`** | `/produtos` | Cadastra uma nova peça no catálogo | `{"nome": "Vestido", "preco": 129.90, "imagemUrl": "https://..."}` | `201 Created` / `500 Error` |
| **`PUT`** | `/produtos/{id}` | Atualiza nome, preço ou foto de uma peça existente | `{"id": 1, "nome": "Novo Nome", "preco": 149.90, "imagemUrl": "https://..."}` | `200 OK` / `404 Not Found` |
| **`DELETE`** | `/produtos/{id}` | Remove uma peça do catálogo pelo seu ID | *Nenhum* | `204 NoContent` / `404 Not Found` |

---

## 🌐 Deploy em Produção

A API está publicada e operando em ambiente de produção no Render através do link base:

```
[https://api-loja-roupas-1ugj.onrender.com](https://api-loja-roupas-1ugj.onrender.com)

```

```

---
