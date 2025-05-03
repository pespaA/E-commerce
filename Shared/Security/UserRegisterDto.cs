using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Security
{
    public record UserRegisterDto
    {
        [Required(ErrorMessage ="DisplayName Is Requird")]
        public string DisplayName { get; init; }
        [Required(ErrorMessage = "Email Is Requird")]
        [EmailAddress]
        public string Email { get; init; }
        [Required(ErrorMessage = "Password Is Requird")]
        public string Password { get; set; }
        [Required(ErrorMessage = "UserName Is Requird")]
        public string UserName { get; set; }
        public string? phoneNumber { get; set; }
    }
}
