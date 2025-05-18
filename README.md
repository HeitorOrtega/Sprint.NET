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
|--------|------------------|----------------------------------- |
| GET    | /v1/motos       | Lista todas as motos                |
| GET    | /v1/motos/{id}  | Retorna uma moto específica         |
| POST   | /v1/motos       | Cadastra uma nova moto              |
| PUT    | /v1/motos/{id}  | Atualiza os dados de uma moto       |
| DELETE | /v1/motos/{id}  | Remove uma moto do sistema          |

### 📍 Funcionários

| Método | Rota                                 | Descrição                            |
|--------|--------------------------------------|--------------------------------------|
| GET    | /v1/funcionarios                    | Lista todos os funcionários           |
| GET    | /v1/funcionarios/{id}                | Retorna um funcionário específico    |
| GET    | /v1/funcionarios/busca?nome={nome}   | Busca um funcionário por nome        |
| POST   | /v1/funcionarios                     | Cadastra um novo funcionário         |
| PUT    | /v1/funcionarios/{id}                | Atualiza os dados de um funcionário  |
| DELETE | /v1/funcionarios/{id}                | Remove um funcionário do sistema     |

### 📍 Pátio

| Método | Rota             | Descrição                              |
|--------|------------------|----------------------------------------|
| GET    | /v1/patios       | Lista todos os pátios                  |
| GET    | /v1/patios/{id}  | Retorna um pátio específico            |
| POST   | /v1/patios       | Cadastra um novo pátio                 |
| PUT    | /v1/patios/{id}  | Atualiza os dados de um pátio          |
| DELETE | /v1/patios/{id}  | Remove um pátio do sistema             |

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

