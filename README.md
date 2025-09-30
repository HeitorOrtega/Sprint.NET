# Sprint 1 - .NET API

API desenvolvida em ASP.NET Core para gerenciamento de motocicletas, funcionários e pátios.
Parte do projeto acadêmico para a Mottu.

---

# 💡 Sobre o Projeto

- Este projeto é uma API RESTful em ASP.NET Core desenvolvida para gerenciar motos, funcionários e pátios de uma empresa de entregas.

- Organiza a frota de motocicletas.

- Controla a alocação em pátios.

- Cadastra e gerencia funcionários.

- Conecta-se a um banco de dados Oracle, permitindo operações completas de CRUD para cada entidade.

---

# 🏗️ Justificativa da Arquitetura

ASP.NET Core: escolhida por sua performance, facilidade para criar APIs RESTful e integração nativa com bancos de dados como Oracle.

Separação de entidades (Motos, Funcionários, Pátios): permite modularidade e manutenção mais fácil, cada módulo gerencia seus próprios dados.

Camadas do sistema (Controller → Service → Repository/DAO): garante organização, facilita testes unitários e manutenção futura.

Banco Oracle: utilizado para simular um ambiente corporativo real, integrando operações CRUD de forma confiável.

---

## ⚙️ Instalação e Execução

### ✅ Pré-requisitos

- [.NET SDK 9.0 ou superior](https://dotnet.microsoft.com/en-us/download)
- [Oracle SQL Developer](https://www.oracle.com/database/sqldeveloper/)
- [Oracle Data Access Components (ODAC)](https://www.oracle.com/database/technologies/dotnet-odacdeploy-downloads.html)
- [Oracle Instant Client](https://www.oracle.com/database/technologies/instant-client/downloads.html)
- Visual Studio, Rider ou outro editor compatível com .NET

---


# 🚀 Passos para Executar
```bash
# 1. Clone o repositório
git clone https://github.com/HeitorOrtega/Sprint1.NET.git

# 2. Acesse a pasta do projeto
cd sprint_1

# 3. Restaure os pacotes e atualize o banco
dotnet restore
dotnet ef database update

# 4. Execute a API
dotnet run
```

---

# 🔧 Configuração do Banco
```json
"ConnectionStrings": {
  "OracleConnection": "User Id=rm557825;Password=fiap25;Data Source=SEU_DATASOURCE;"
}
```

---

# 🔗 Endpoints da API

### 🏍️ Motos

| Método | Rota             | Descrição                     |
| ------ | ---------------- | ----------------------------- |
| GET    | `/v1/motos`      | Lista todas as motos          |
| GET    | `/v1/motos/{id}` | Retorna uma moto específica   |
| POST   | `/v1/motos`      | Cadastra uma nova moto        |
| PUT    | `/v1/motos/{id}` | Atualiza os dados de uma moto |
| DELETE | `/v1/motos/{id}` | Remove uma moto do sistema    |

Exemplo de POST /v1/motos
```json
 {
  "cor": "azul",
  "placa": "123teste",
  "dataFabricacao": "2025-09-12"
}

```
Exemplo de PUT /v1/motos
```json
 {
  "cor": "preto",
  "placa": "ABCD123"
  "dataFabricacao": "2023-08-12"
}
```

---


### 👷 Funcionários

| Método | Rota                                 | Descrição                                                    |
| ------ | ------------------------------------ | -------------------------------------------------------------|
| GET    | `/v1/funcionarios`                   | Lista todos os funcionários                                  |
| GET    | `/v1/funcionarios/{id}`              | Retorna um funcionário específico                            |
| GET    | `/v1/funcionarios/busca?nome={nome}` | Busca um funcionário por nome                                |
| POST   | `/v1/funcionarios`                   | Cadastra um novo funcionário (ID de pátio obrigatório)       |
| PUT    | `/v1/funcionarios/{id}`              | Atualiza os dados de um funcionário (ID de pátio obrigatório)|
| DELETE | `/v1/funcionarios/{id}`              | Remove um funcionário do sistema                             |


Exemplo de POST /v1/funcionarios
```json
{
  "nome": "Cleber",
  "cpf": "43873308776",
  "email": "cleber@gmail.com",
  "rg": "565009277",
  "telefone": "11947438811",
  "patioId": 22
}
```

---

### 🏢 Pátios

| Método | Rota              | Descrição                     |
| ------ | ----------------- | ----------------------------- |
| GET    | `/v1/patios`      | Lista todos os pátios         |
| GET    | `/v1/patios/{id}` | Retorna um pátio específico   |
| POST   | `/v1/patios`      | Cadastra um novo pátio        |
| PUT    | `/v1/patios/{id}` | Atualiza os dados de um pátio |
| DELETE | `/v1/patios/{id}` | Remove um pátio do sistema    |

Exemplo de POST /v1/patios
```json
{
  "localizacao": "patio norte"
}
```
---

## 🌐 Acesso ao Swagger

Após executar a API, acesse:

```bash
http://localhost:5051/swagger
```

## 🧪 Testes
Para rodar os testes unitários da API:

```bash
dotnet test
```

---

## 👥 Equipe

Projeto desenvolvido para a disciplina de Análise e Desenvolvimento de Sistemas – FIAP.

Heitor Ortega Silva – RM 557825

Marcos Lourenço – RM 556496

Pedro Saraiva – RM 555160

⚡ Projeto acadêmico – FIAP | Mottu API Sprint 3
