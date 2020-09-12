using System;
using System.ComponentModel.DataAnnotations;

namespace app.api.DTOs
{
    public class UserForRegister
    {
        [Required()]
        public string Username { get; set; }
        [Required()]
        public string Gender { get; set; }
        [Required()]
        public string KnownAs { get; set; }
        [Required()]
        public DateTime DateOfBirth { get; set; }
        [Required()]
        public string City { get; set; }
        [Required()]
        public string Country { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }

        [Required(), StringLength(40, MinimumLength = 4, ErrorMessage = "You must specify a password between 4 and 40 characters")]
        public string Password { get; set; }

        public UserForRegister()
        {
            var dt = DateTime.Now;

            this.Created = dt;
            this.LastActive = dt;
        }
    }
}
