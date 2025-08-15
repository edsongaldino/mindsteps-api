using System.ComponentModel.DataAnnotations;

namespace MindSteps.Domain.Entities
{
    public class Usuario
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "O nome � obrigat�rio")]
        [MaxLength(100)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O e-mail � obrigat�rio")]
        [MaxLength(150)]
        [EmailAddress(ErrorMessage = "E-mail inv�lido")]
        public string Email { get; set; }

        [MaxLength(20)]
        public string? Telefone { get; set; }

        [Required(ErrorMessage = "A senha � obrigat�ria")]
        [MaxLength(255)]
        public string Senha { get; set; }
    }
}