using System.ComponentModel.DataAnnotations;

namespace BlogAPI.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "O valor digitado não é um e-mail válido.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "A senha é obrigatória.")]
        public string Password { get; set; }
    }
}
