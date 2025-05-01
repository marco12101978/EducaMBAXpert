
# EducaMBAXpert - Plataforma de Gestão Educacional

## 1. Apresentação

O projeto **EducaMBAXpert** é uma plataforma modular voltada para a gestão educacional, contemplando cadastro de usuários, matrículas, catálogo de cursos, avaliações e pagamentos.
Projetado com arquitetura distribuída, utilizando princípios de DDD (Domain-Driven Design) e CQRS (Command Query Responsibility Segregation).

---

## 2. Proposta do Projeto

Este projeto é dividido em múltiplos módulos:

- **Usuários**: Gerenciamento de cadastro, autenticação e perfis de usuário.
- **Matrículas**: Controle de inscrições em cursos e gestão de status.
- **Catálogo de Cursos**: Administração de cursos disponíveis na plataforma.
- **Avaliações**: Avaliação de desempenho e feedback de usuários em cursos.
- **Pagamentos**: Processamento de transações financeiras e controle fiscal.
- **Core**: Compartilhamento de componentes essenciais entre os módulos.
- **Web API**: Exposição dos serviços através de APIs RESTful.

---

## 3. Tecnologias Utilizadas

- **Linguagem**: C#
- **Frameworks**:
  - ASP.NET Core Web API
  - Entity Framework Core
- **Arquitetura**:
  -CQRS (Command Query Responsibility Segregation)
  -DDD (Domain-Driven Design)
- **Banco de Dados**: SQL Server
- **Autenticação**:
  - JWT (JSON Web Token)
- **Documentação**:
  - Swagger para a API
- **IDE recomendada**: Visual Studio 2022 ou superior

---

## 4. Estrutura do Projeto

```plaintext
src/
├── EducaMBAXpert.Api/                 # API principal
├── EducaMBAXpert.Core/                # Biblioteca de utilitários e base
├── EducaMBAXpert.Usuarios/            # Aplicação, Domínio e Dados de Usuários
├── EducaMBAXpert.CatalogoCursos/      # Aplicação, Domínio e Dados de Cursos
├── EducaMBAXpert.Pagamentos/          # Aplicação, Anti-corruption layer, Business e Dados de Pagamentos
└── README.md                          # Documento de apoio
```

---

## 5. Funcionalidades Implementadas

- Cadastro e autenticação de usuários
- Gerenciamento de cursos e matrículas
- Avaliações de cursos
- Processamento de pagamentos
- Exposição de APIs RESTful
- Documentação automática via Swagger

---

## 6. Como Executar o Projeto

### Pré-requisitos

- .NET SDK 8.0 ou superior
- SQL Server
- Visual Studio 2022 (ou IDE compatível)
- Git

### Passos para Executar

1. **Clone o repositório:**
   ```bash
   git clone https://github.com/marco12101978/EducaMBAXpert.git
   cd EducaMBAXpert
   ```

2. **Configure o Banco de Dados:**
   - Atualize a `ConnectionString` no arquivo `appsettings.json` da API.
   - Rode as migrações do EF Core (se necessário).

3. **Execute o Projeto:**
   - Defina o `EducaMBAXpert.Api` como projeto de inicialização.
   - Execute a solução no Visual Studio.

4. **Acesse:**
   - API: `http://localhost:5000/swagger`

---

## 7. Documentação da API

Após a execução da aplicação, acesse:  
➡️ `http://localhost:5000/swagger`

---

## 8. Observações

- Este projeto faz parte de um estudo acadêmico e pode receber contribuições futuras.
- Para dúvidas e feedback, utilize o recurso de **Issues** no repositório.
