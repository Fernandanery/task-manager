namespace TaskManager.SharedKernel.Constants
{
    public static class Miscellaneous
    {
        public const string StatusCode404 = "O recurso solicitado não foi encontrado.";
        public const string StatusCode400 = "teste 400 Bad request";
        public const string StatusCode500 = "Erro inesperado no servidor.";

        // Logs - Tarefas
        public const string GetAllTasks = "Recebida a requisição para buscar todas as tarefas";
        public const string GetTaskById = "Recebida requisição GET /api/Task/{Id}";
        public const string TaskByIdNotFound = "Tarefa com ID {Id} não encontrada";

        public const string CreateTaskRequest = "Recebida requisição POST para criar uma nova tarefa: {@TaskDto}";
        public const string TaskCreatedSuccessfully = "Tarefa criada com sucesso. ID: {Id}";

        public const string UpdateTaskRequest = "Recebida requisição PUT para atualizar tarefa ID {Id}: {@TaskDto}";
        public const string TaskUpdateNotFound = "Tentativa de atualizar tarefa ID {Id}, mas não foi encontrada";
        public const string TaskUpdatedSuccessfully = "Tarefa ID {Id} atualizada com sucesso";

        public const string DeleteTaskRequest = "Recebida requisição DELETE para tarefa ID {Id}";
        public const string TaskDeleteNotFound = "Tentativa de deletar tarefa ID {Id}, mas não foi encontrada";
        public const string TaskDeletedSuccessfully = "Tarefa ID {Id} deletada com sucesso";

        // Logs - User
        public const string GetAllUsers = "Recebida a requisição para buscar todos os usuários";
        public const string GetUserById = "Recebida requisição GET /api/User/{Id}";
        public const string UserByIdNotFound = "Usuário com ID {Id} não encontrado";
        public const string CreateUserRequest = "Recebida requisição POST /api/User com body {@userDto}";
        public const string UserCreatedSuccessfully = "Usuário criado com sucesso. ID: {Id}";
        public const string UpdateUserRequest = "Recebida requisição PUT /api/User/{Id} com body {@userDto}";
        public const string UserUpdateNotFound = "Usuário para atualização não encontrado. ID: {Id}";
        public const string UserUpdatedSuccessfully = "Usuário atualizado com sucesso. ID: {Id}";
        public const string DeleteUserRequest = "Recebida requisição DELETE /api/User/{Id}";
        public const string UserDeleteNotFound = "Usuário para exclusão não encontrado. ID: {Id}";
        public const string UserDeletedSuccessfully = "Usuário excluído com sucesso. ID: {Id}";

        // Validations
        public const string NameRequired = "O nome é obrigatório.";
        public const string NameTooShort = "O nome deve ter pelo menos 3 caracteres.";
        public const string NameTooLong = "O nome não pode ter mais de 50 caracteres.";

        public const string EmailRequired = "O e-mail é obrigatório.";
        public const string InvalidEmail = "O formato do e-mail é inválido.";

        public const string TitleRequired = "O título é obrigatório.";
        public const string TitleTooLong = "O título não pode ter mais de 100 caracteres.";

        public const string DescriptionRequired = "A descrição é obrigatória.";
        public const string DescriptionTooLong = "A descrição não pode ter mais de 500 caracteres.";

        public const string CreatedAtRequired = "A data de criação é obrigatória.";
        public const string InvalidCreatedAt = "A data de criação não pode ser futura.";
    }
}
