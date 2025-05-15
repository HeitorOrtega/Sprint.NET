# Sprint 1 - .NET API

API desenvolvida em ASP.NET Core para gerenciamento de motocicletas, funcionários e pátios.  
Parte do projeto acadêmico para a **SmartConnectCar (Mottu)**.

---

## 📌 Descrição do Projeto

Esta API fornece endpoints RESTful para **cadastrar, consultar, atualizar e excluir** informações sobre:

- 🏍️ Motos  
- 👷 Funcionários  
- 🏢 Pátios  

Ela é parte do sistema de atendimento automotivo **SmartConnectCar**, promovendo uma solução eficiente para gestão de oficinas e veículos.

---

## 🔗 Rotas da API

### 📍 Motos

| Método | Rota             | Descrição                          |
|--------|------------------|------------------------------------|
| GET    | /api/Motos       | Lista todas as motos               |
| GET    | /api/Motos/{id}  | Retorna uma moto específica        |
| POST   | /api/Motos       | Cadastra uma nova moto             |
| PUT    | /api/Motos/{id}  | Atualiza os dados de uma moto      |
| DELETE | /api/Motos/{id}  | Remove uma moto do sistema         |

### 📍 Funcionários

| Método | Rota                                 | Descrição                            |
|--------|--------------------------------------|--------------------------------------|
| GET    | /api/Funcionario                     | Lista todos os funcionários          |
| GET    | /api/Funcionario/{id}                | Retorna um funcionário específico    |
| GET    | /api/Funcionario/busca?nome={nome}   | Busca um funcionário por nome        |
| POST   | /api/Funcionario                     | Cadastra um novo funcionário         |
| PUT    | /api/Funcionario/{id}                | Atualiza os dados de um funcionário  |
| DELETE | /api/Funcionario/{id}                | Remove um funcionário do sistema     |

### 📍 Pátio

| Método | Rota             | Descrição                            |
|--------|------------------|----------------------------------------|
| GET    | /api/Patio       | Lista todos os pátios                  |
| GET    | /api/Patio/{id}  | Retorna um pátio específico            |
| POST   | /api/Patio       | Cadastra um novo pátio                 |
| PUT    | /api/Patio/{id}  | Atualiza os dados de um pátio          |
| DELETE | /api/Patio/{id}  | Remove um pátio do sistema             |

---

## ⚙️ Instalação e Execução

### ✅ Pré-requisitos

- [.NET SDK 9.0 ou superior](https://dotnet.microsoft.com/en-us/download)
- [Oracle SQL Developer](https://www.oracle.com/database/sqldeveloper/)
- [Oracle Data Access Components (ODAC)](https://www.oracle.com/database/technologies/dotnet-odacdeploy-downloads.html)
- [Oracle Instant Client](https://www.oracle.com/database/technologies/instant-client/downloads.html)
- Rider ou Visual Studio (ou outro editor compatível com .NET)

---

### 🚀 Passos para Executar


# 1. Clone o repositório
git clone https://github.com/HeitorOrtega/Sprint1.NET.git

# 2. Acesse a pasta do projeto
cd Sprint1.NET

# 3. Restaure os pacotes e atualize o banco
dotnet restore
dotnet ef database update

# 4. Execute a API
dotnet run

# Acesso ao Swagger 
http://localhost:5051/swagger


📫 Contato
Desenvolvido por Heitor Ortega Silva
📚 Curso: Análise e Desenvolvimento de Sistemas – FIAP

