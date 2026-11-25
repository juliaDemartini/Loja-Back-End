
# 🛍️ Loja DNA — Backend API

API RESTful desenvolvida em **C# com .NET 8** para gerenciamento de produtos de uma loja de roupas. A aplicação utiliza PostgreSQL por meio do Supabase e está hospedada no Render.

O projeto também implementa um fluxo de **CI/CD com GitHub Actions**, incluindo build, testes automatizados e deploy da aplicação.

---

## 🛠️ Tecnologias utilizadas

- **C# / .NET 8:** desenvolvimento da API utilizando Minimal APIs.
- **ASP.NET Core:** estrutura para construção dos endpoints REST.
- **Entity Framework Core:** ORM para acesso e manipulação dos dados.
- **PostgreSQL / Supabase:** banco de dados relacional em nuvem.
- **xUnit:** framework para testes automatizados.
- **EF Core InMemory:** banco em memória utilizado nos testes.
- **Git e GitHub:** versionamento e colaboração.
- **GitHub Actions:** automação do pipeline CI/CD.
- **Render:** hospedagem e deploy da API.
- **Swagger / OpenAPI:** documentação e teste dos endpoints.

---

## 📁 Estrutura do projeto

```text
Loja-Back-End/
├── .github/
│   └── workflows/
│       └── cicd.yml
├── LojaBackEnd/
├── LojaBackEnd.Tests/
│   ├── Infrastructure/
│   │   └── CustomWebApplication.cs
│   ├── UnitTest1.cs
│   └── LojaBackEnd.Tests.csproj
├── Data/
├── Models/
├── Program.cs
└── LojaBackEnd.csproj
```

---

## ⚙️ Configuração do ambiente

### Pré-requisitos

- .NET SDK 8.0 ou superior.
- Git instalado.
- Conta e banco PostgreSQL configurados no Supabase.

### Configuração da conexão com o banco

A aplicação utiliza a variável de configuração:

`ConnectionStrings:DefaultConnection`

Para configurar a conexão localmente sem armazenar credenciais no repositório, utilize User Secrets:

```bash
dotnet user-secrets init
```

Em seguida:

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "SUA_CONNECTION_STRING"
```

Também é possível configurar a variável de ambiente:

```text
ConnectionStrings__DefaultConnection
```

**Importante:** nunca publique senhas, tokens ou connection strings reais no GitHub.

---

## 🚀 Executando a aplicação localmente

1. Clone o repositório:

```bash
git clone https://github.com/juliaDemartini/Loja-Back-End.git
```

2. Acesse a pasta:

```bash
cd Loja-Back-End
```

3. Restaure as dependências:

```bash
dotnet restore
```

4. Execute a aplicação:

```bash
dotnet run
```

5. Acesse a documentação Swagger pelo endereço informado no terminal, quando disponível.

---

## 🧪 Testes automatizados

O projeto utiliza **xUnit** para executar testes automatizados.

Foram implementados quatro testes, divididos entre as integrantes da equipe:

| Integrante | Teste | Objetivo |
|---|---|---|
| Cindyane | Validação de status do pedido | Verificar a aceitação de um status permitido. |
| Cindyane | Validação do nome do usuário | Verificar a rejeição de um nome acima do limite definido. |
| Julia | Listagem de produtos | Verificar se a API retorna uma lista de produtos. |
| Julia | Cadastro de produtos | Verificar se um produto pode ser cadastrado com sucesso. |

Os testes de integração utilizam `WebApplicationFactory` e banco de dados em memória, evitando a necessidade de utilizar o banco de produção durante sua execução.

### Como executar os testes

Na raiz do repositório, execute:

```bash
dotnet test LojaBackEnd.Tests/LojaBackEnd.Tests.csproj
```

Para visualizar os detalhes da execução:

```bash
dotnet test LojaBackEnd.Tests/LojaBackEnd.Tests.csproj --logger "console;verbosity=normal"
```

---

## 🔄 Pipeline CI/CD — GitHub Actions

O projeto possui um pipeline automatizado configurado em:

`.github/workflows/cicd.yml`

O fluxo utiliza dois eventos principais:

### 1. Pull Request para a branch main

Quando um Pull Request é aberto ou atualizado direcionado à branch `main`, o GitHub Actions executa:

- Restauração das dependências.
- Build da aplicação.
- Execução dos testes automatizados.

O merge só pode acontecer quando as verificações obrigatórias forem aprovadas.

### 2. Push na branch main

Após a integração das alterações na branch `main`, o pipeline executa:

- Build da aplicação.
- Execução dos testes automatizados.
- Deploy da API no Render, quando habilitado na configuração do workflow.

### Fluxo de integração

```text
Desenvolvimento em branch
          |
          v
     Pull Request
          |
          v
   GitHub Actions
          |
          v
    Build e Testes
          |
     +----+----+
     |         |
   Falha     Sucesso
     |         |
     v         v
 Corrigir   Aprovação
               |
               v
           Merge main
               |
               v
        GitHub Actions
               |
               v
          Build e Testes
               |
               v
          Deploy Render
```

### Proteção da branch main

A branch principal possui regras de proteção para garantir a qualidade das alterações:

- Integração somente por Pull Request.
- Aprovação de outro integrante.
- Verificações obrigatórias do GitHub Actions.
- Bloqueio do merge quando o pipeline falha.

---

## 🌐 Deploy em produção

A API está hospedada na plataforma Render:

https://api-loja-roupas-1ugj.onrender.com

O deploy é integrado ao GitHub Actions, permitindo automatizar a publicação após alterações aprovadas e integradas à branch principal.

---

## 👩‍💻 Equipe

Projeto desenvolvido em colaboração por:

- **Julia Demartini**
- **Cindyane Batista**

Repositório: https://github.com/juliaDemartini/Loja-Back-End

---

## 📄 Licença

Projeto desenvolvido para fins acadêmicos.
