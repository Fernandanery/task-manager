namespace TaskManager.SharedKernel.Constants
{
    public static class ValidationMessages
    {
        public const string RequiredField = "O campo '{PropertyName}' é obrigatório.";
        public const string MaxLength = "O campo '{PropertyName}' deve ter no máximo {MaxLength} caracteres.";
        public const string InvalidEmail = "O campo '{PropertyName}' não é um e-mail válido.";
        public const string StatusRequired = "O status da tarefa é obrigatório.";

    }
}
