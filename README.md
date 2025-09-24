# Sprint 1 - .NET API

API desenvolvida em ASP.NET Core para gerenciamento de motocicletas, funcionários e pátios.
Parte do projeto acadêmico para a **Mottu**.

---

# 💡 Sobre o Projeto
Este projeto é uma API RESTful em ASP.NET Core desenvolvida para gerenciar motos, funcionários e pátios de uma empresa de entregas.

- A solução foi projetada para:

- Organizar a frota de motocicletas

- Controlar a alocação em pátios

- Cadastrar e gerenciar funcionários

👉 A API se conecta a um banco de dados Oracle e permite realizar operações completas de CRUD para cada entidade.

---

# Requisito inicial:
- Entrar na **appsettings.json** e colocar Id:rm557825 e Password:fiap25
<img width="847" height="286" alt="image" src="https://github.com/user-attachments/assets/02a139b1-3692-4c13-b49d-0cdb9638cc81" />

---

📌 Funcionalidades

- 🏍️ Motos: cadastro, consulta, atualização e exclusão

- 👷 Funcionários: gerenciamento completo, incluindo busca por nome

- 🏢 Pátios: controle e manutenção de registros

---

## 🔗 Rotas da API

### 📍 Motos

| Método | Rota             | Descrição                     |
| ------ | ---------------- | ----------------------------- |
| GET    | `/v1/motos`      | Lista todas as motos          |
| GET    | `/v1/motos/{id}` | Retorna uma moto específica   |
| POST   | `/v1/motos`      | Cadastra uma nova moto        |
| PUT    | `/v1/motos/{id}` | Atualiza os dados de uma moto |
| DELETE | `/v1/motos/{id}` | Remove uma moto do sistema    |


![image](https://github.com/user-attachments/assets/8c907b04-c2e7-4154-a79e-00d80cf123f5)


### 📍 Funcionários

| Método | Rota                                 | Descrição                           |
| ------ | ------------------------------------ | ----------------------------------- |
| GET    | `/v1/funcionarios`                   | Lista todos os funcionários         |
| GET    | `/v1/funcionarios/{id}`              | Retorna um funcionário específico   |
| GET    | `/v1/funcionarios/busca?nome={nome}` | Busca um funcionário por nome       |
| POST   | `/v1/funcionarios`                   | Cadastra um novo funcionário        |
| PUT    | `/v1/funcionarios/{id}`              | Atualiza os dados de um funcionário |
| DELETE | `/v1/funcionarios/{id}`              | Remove um funcionário do sistema    |


![image](https://github.com/user-attachments/assets/882c795e-5d4b-4c7e-9728-d6d94685c043)


### 📍 Pátios

| Método | Rota              | Descrição                     |
| ------ | ----------------- | ----------------------------- |
| GET    | `/v1/patios`      | Lista todos os pátios         |
| GET    | `/v1/patios/{id}` | Retorna um pátio específico   |
| POST   | `/v1/patios`      | Cadastra um novo pátio        |
| PUT    | `/v1/patios/{id}` | Atualiza os dados de um pátio |
| DELETE | `/v1/patios/{id}` | Remove um pátio do sistema    |

![image](https://github.com/user-attachments/assets/a46272e3-5165-4737-a50f-1da361677a25)


## ⚙️ Instalação e Execução

### ✅ Pré-requisitos

- [.NET SDK 9.0 ou superior](https://dotnet.microsoft.com/en-us/download)
- [Oracle SQL Developer](https://www.oracle.com/database/sqldeveloper/)
- [Oracle Data Access Components (ODAC)](https://www.oracle.com/database/technologies/dotnet-odacdeploy-downloads.html)
- [Oracle Instant Client](https://www.oracle.com/database/technologies/instant-client/downloads.html)
- Visual Studio, Rider ou outro editor compatível com .NET

---

### 🚀 Passos para Executar

```bash
# 1. Clone o repositório
git clone https://github.com/HeitorOrtega/Sprint1.NET.git

# 2. Acesse a pasta do projeto
cd Sprint1.NET

# 3. Restaure os pacotes e atualize o banco
dotnet restore
dotnet ef database update

# 4. Execute a API
dotnet run
```

🌐 Acesso ao Swagger
Acesse no navegador:

http://localhost:5051/swagger



👥 Equipe

Projeto desenvolvido para a disciplina de Análise e Desenvolvimento de Sistemas – FIAP.

- Heitor Ortega Silva – RM 55782
- Marcos Lourenço – RM 556496
- Pedro Saraiva – RM 555160

⚡ Projeto acadêmico – FIAP | Mottu API Sprint 3
