namespace Bar.Models;

public class Plato
{
    public int IdPlato { get; set; }
    public string Nombre { get; set; }
    public string Ingredientes { get; set; }
    public string Descripcion { get; set; }
    public int Costo { get; set; }
    public string? Imagen { get; set; }
    public bool Estado { get; set; } = true;

    public int IdRes { get; set; }
}