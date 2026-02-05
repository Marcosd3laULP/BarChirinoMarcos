namespace Bar.Models;

public class Restaurante
{
    public int IdRes { get; set; }
    public string Nombre { get; set; }
    public string Ubicacion { get; set; }
    public string Especialidad { get; set; }
    public string Imagen { get; set; }

    public bool Estado { get; set; } = true;

    //Clave foranea:
    public int IdUsuario { get; set; }
}