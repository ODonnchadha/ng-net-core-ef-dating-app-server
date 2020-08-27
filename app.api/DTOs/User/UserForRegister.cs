using System.ComponentModel.DataAnnotations;

namespace app.api.DTOs
{
    public class UserForRegister
    {
        [Required()]
        public string Username { get; set; }

        [Required()]
        [StringLength(40, MinimumLength = 4, ErrorMessage = "You must specify a password between 4 and 40 characters")]
        public string Password { get; set; }
    }
}
