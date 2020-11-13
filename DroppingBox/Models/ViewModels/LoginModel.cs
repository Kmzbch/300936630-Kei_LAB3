using System.ComponentModel.DataAnnotations;

namespace DroppingBox.Models
{
    public class LoginModel
    {

        [Required]
        public string Email { get; set; }

        [Required]
        [UIHint("password")]
        public string Password { get; set; }

        public string ReturnUrl { get; set; } = "/";
    }

}
