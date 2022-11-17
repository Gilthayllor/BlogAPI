using System.ComponentModel.DataAnnotations;

namespace BlogAPI.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "O e-mail é inválido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "A senha deve conter entre 6 e 20 caracteres.")]
        public string Password { get; set; }

        public string About { get; set; }

        public string Image { get; set; }

    }
}
