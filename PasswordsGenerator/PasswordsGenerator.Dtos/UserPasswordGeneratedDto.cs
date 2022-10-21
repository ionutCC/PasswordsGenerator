using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordsGenerator.Dtos
{
    public class UserPasswordGeneratedDto
    {
        public string? userID { get; set; }
        public DateTime? passwordGenerationDatetime { get; set; }
        public string? generatedPassword { get; set; }
    }
}
