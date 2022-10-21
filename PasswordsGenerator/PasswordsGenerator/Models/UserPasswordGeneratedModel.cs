namespace PasswordsGenerator.Models
{
    public class UserPasswordGeneratedModel
    {
        public string? userID { get; set; }
        public DateTime? passwordGenerationDatetime { get; set; }
        public string? generatedPassword { get; set; }
    }
}
