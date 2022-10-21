using System;
using System.Collections.Generic;

namespace DataLayer.Entities
{
    public partial class UserPasswordGenerated
    {
        public int UserCountor { get; set; }
        public string? UserID { get; set; }
        public DateTime? PasswordGenerationDatetime { get; set; }
        public string? GeneratedPassword { get; set; }
    }
}
