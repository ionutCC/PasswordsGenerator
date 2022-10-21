using PasswordsGenerator.Helpers;
using System.ComponentModel.DataAnnotations;

namespace PasswordsGenerator.ViewModels.Home
{
    public class GeneratedPasswordViewModel
    {
        public string? userID { get; set; }
        public DateTime? passwordGenerationDatetime { get; set; }
        public string? generatedPassword { get; set; }  
        public string? messageAlert { get; set; }
    }
}
