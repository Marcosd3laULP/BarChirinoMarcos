
namespace Bar.Models;
public class UsuarioDTO
{
    public int IdUsuario { get; set; }
    public required string Nombre { get; set; }
    public required string Apellido { get; set; }

    public required string Nick { get; set; }
    public required string Email { get; set; }
    public RolUsuario Rol { get; set; }
}