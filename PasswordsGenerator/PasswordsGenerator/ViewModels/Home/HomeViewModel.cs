using PasswordsGenerator.Helpers;
using System.ComponentModel.DataAnnotations;

namespace PasswordsGenerator.ViewModels.Home
{
    public class HomeViewModel
    {
        [Required(ErrorMessage = "The User Identifier is required!")]
        public string? userID { get; set; }
        [Required(ErrorMessage = "The Date and Time are required!")]
        [DateTimeCustom]
        public DateTime? passwordGenerationDatetime { get; set; }
    }
}
