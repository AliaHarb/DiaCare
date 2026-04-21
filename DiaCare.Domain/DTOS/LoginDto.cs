using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiaCare.Domain.DTOS
{
    public class LoginDto
    {
        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required, MinLength(8)]
        public string Password { get; set; } = null!;


    }
}
