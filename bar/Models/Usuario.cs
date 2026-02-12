using System.ComponentModel.DataAnnotations;

namespace Bar.Models;
public class Usuario
{
    public int IdUsuario { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(50)]
    public string Nombre { get; set; }

    [Required(ErrorMessage = "El apellido es obligatorio")]
    [StringLength(50)]
    public string Apellido { get; set; }

    [Required(ErrorMessage = "El nick es obligatorio")]
    [StringLength(30)]
    public string Nick { get; set; }

    [Required(ErrorMessage = "El email es obligatorio")]
    [EmailAddress(ErrorMessage = "Formato de email inválido")]
    public string Email { get; set; }

    [Required(ErrorMessage = "El teléfono es obligatorio")]
    public string Telefono { get; set; }

    [Required(ErrorMessage = "El domicilio es obligatorio")]
    public string Domicilio { get; set; }

    public string? Avatar { get; set; }

    public RolUsuario Rol { get; set; }

    public string? PasswordHash { get; set; }

    public bool Estado { get; set; } = true;
}
