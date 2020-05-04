using System.ComponentModel.DataAnnotations;
using System;

namespace Dating.API.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "You Must Specify the password between 4 and 8 charcters")]
        public string Password { get; set; }
            
        [Required]
        public string Gender { get; set; }
          
        [Required]
        public string KnownAS { get; set; }
            
        [Required]
        public DateTime DateOfBirth { get; set; }
            
        [Required]
        public string City { get; set; }
          
        [Required]
        public string Country { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastActive { get; set; }

        public UserForRegisterDto()
        {
            Created = DateTime.Now;
            LastActive = DateTime.Now;
        }
    }
}