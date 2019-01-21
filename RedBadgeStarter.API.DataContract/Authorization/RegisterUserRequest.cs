using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RedBadgeStarter.API.DataContract.Authorization
{
    public class RegisterUserRequest
    {
        [Required]
        public string UserName { get; set; }

        public string Email { get; set; }

        [Required]
        [StringLength(12, MinimumLength = 4, ErrorMessage = "Password must be between 4 and 12 characters.")]
        public string Password { get; set; }
    }
}
