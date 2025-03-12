# TaskManager - Gerenciador de Tarefas

Este projeto foi desenvolvido para fins de estudo e explora o uso de **autenticação JWT** e **gerenciamento de tarefas** utilizando **.NET 9** e ferramentas modernas para garantir desempenho e segurança.

---

## Tecnologias
- **.NET 9** – Desenvolvimento da API.  
- **Entity Framework Core** – ORM para manipulação de dados.  
- **JWT (JSON Web Tokens)** – Para autenticação e autorização.  
- **Serilog** – Para logging estruturado e rastreamento de atividades.  
- **Mapster** – Para mapeamento eficiente de objetos.  
- **FluentValidation** – Para validação de dados de entrada.  
- **Hangfire** – Para gerenciamento de tarefas em segundo plano.  
- **API Versioning** – Para versionamento de endpoints.  

---

## Objetivo do Projeto
- Criar um sistema de gerenciamento de tarefas onde os usuários possam criar, editar, excluir e organizar tarefas.  
- Implementar autenticação e autorização com **JWT** para controle de acesso.  
- Utilizar **Hangfire** para executar tarefas em segundo plano (exemplo: lembretes).  
- Melhorar o desempenho com **cache distribuído** e **rate limiting**.  
- Garantir validação e integridade dos dados usando **FluentValidation**.  

---

## Funcionalidades
-  Criação, edição e exclusão de tarefas.  
-  Criação de subtarefas vinculadas.  
-  Atribuição de status e etiquetas personalizadas às tarefas.  
-  Autenticação e autorização com JWT.  
-  Registro e gerenciamento de usuários e permissões.  
-  Logging avançado com Serilog.  
-  Configuração de API Versioning para compatibilidade futura.  
-  Execução de tarefas automáticas com Hangfire (notificações de tarefas pendentes). 
