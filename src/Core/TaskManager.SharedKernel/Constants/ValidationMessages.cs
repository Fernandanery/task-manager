namespace TaskManager.SharedKernel.Constants
{
    public static class ValidationMessages
    {
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
