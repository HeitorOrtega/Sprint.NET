# SmartConnectCar API

API desenvolvida em ASP.NET Core para gerenciamento de motocicletas e seus proprietários.  
Parte do projeto acadêmico da startup SmartConnectCar 🚀.

---

## 📌 Descrição do Projeto

Essa API fornece endpoints RESTful para cadastrar, editar, excluir e listar **Motos** , **Pátios** **Funcinarios**, permitindo que a aplicação gerencie com facilidade os dados necessários ao funcionamento do sistema de atendimento automotivo SmartConnectCar.

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

### 📍 Funcionarios

| Método | Rota                   | Descrição                            |
|--------|------------------------|--------------------------------------|
| GET    | /api/funcionario    | Lista todos os proprietários            |
| GET    | /api/funcionario/{id}| Retorna um funcionario específico      |
| GET    |/api/funcionario/{buscaPorNome}| Retorna um funcionario        |
| POST   | /api/funcionario     | Cadastra um novo funcionario           |
| PUT    | /api/funcionario/{id}| Atualiza os dados de um funcionario    |
| DELETE | /api/funcionario/{id}| Remove um funcionario do sistema       |


### 📍 Pátio

| Método | Rota                   | Descrição                            |
|--------|------------------------|--------------------------------------|
| GET    | /api/patio     | Lista todos os pátios                        |
| GET    | /api/patio/{id}| Retorna um pátio específico                  |
| POST   | /api/patio     | Cadastra um novo pátio                       |
| PUT    | /api/patio/{id}| Atualiza os dados de um pátio                |
| DELETE | /api/patio/{id}| Remove um pátio do sistema                   |
---

## ⚙️ Instalação e Execução

### Pré-requisitos:
- .NET SDK 9 ou superior
- Oracle SQL Developer
- Preferencia Rider

### Passos:

1. Clone o repositório:
bash
git clone https://github.com/seu-usuario/](https://github.com/HeitorOrtega/Sprint1.NET.git

2.
cd Sprint_1

3.
dotnet restore
dotnet ef database update

4.
Execute a API

5. Acesse o Swagger
http://localhost:5051/swagger


📫 Contato
Desenvolvido por Heitor Ortega Silva
Curso: Análise e Desenvolvimento de Sistemas - FIAP



