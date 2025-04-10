# TaskManager - Gerenciador de Tarefas

Projeto para fins de estudo e explora o uso de **autenticação JWT** e **gerenciamento de tarefas** utilizando **.NET 9** e ferramentas modernas para garantir desempenho e segurança.

---

## Tecnologias
- **.NET 9** – Desenvolvimento da API.  
- **Entity Framework Core** – ORM para manipulação de dados.
- **SQL Server** (LocalDB) -  Banco para persistência de dados
- **Swagger e Scalar** - Documentação da API	
- **FluentValidation** – Para validação de dados de entrada.  
- **Mapster** – Para mapeamento eficiente de objetos.  
- **JWT (JSON Web Tokens)** – Para autenticação e autorização.
- **Serilog** – Para logging estruturado e rastreamento de atividades.  
- **Hangfire** – Para gerenciamento de tarefas em segundo plano.  
- **Cancellation Token** - Para cancelar requisições assíncronas de maneira controlada
- **X-unit** - Para testes unitários

---

## Funcionalidades
-  Criação, edição e exclusão de tarefas.  
-  Criação de subtarefas vinculadas.  
-  Atribuição de status às tarefas.  
-  Autenticação e autorização com JWT.  
-  Registro e gerenciamento de usuários e permissões.  
-  Logging avançado com Serilog.  
-  Execução de tarefas automáticas com Hangfire. 
