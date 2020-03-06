using System.ComponentModel.DataAnnotations;

namespace API_Projeto_Casa.Models {
    public class Usuario {

        public int Id { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength (100, ErrorMessage = "O nome não pode conter mais de 100 caracteres")]
        [MinLength (2, ErrorMessage = "Nome curto demais")]

        public string Email { get; set; }

        [Required]
        [MaxLength (100, ErrorMessage = "A senha não pode conter mais de 100 caracteres")]
        [MinLength (8, ErrorMessage = "Senha curta demais")]
        public string Senha { get; set; }
    }
}