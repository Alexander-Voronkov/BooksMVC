using System.ComponentModel.DataAnnotations;

namespace BooksMVC.Models
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Not specified by Email")]
        public string Email { get; set; } = default!;
        [Required(ErrorMessage = "Not specified by Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = default!;
    }
}
