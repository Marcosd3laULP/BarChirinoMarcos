
namespace Bar.Models;
public class Usuario
{
    public int IdUsuario { get; set; }
    public required string Nombre { get; set; }
    public required string Apellido { get; set; }

    public required string Nick { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public required string Telefono { get; set; }
    public required string Domicilio { get; set; }
    
    public RolUsuario Rol { get; set; }
    public string? Avatar { get; set;}
    public bool Estado { get; set; }
}