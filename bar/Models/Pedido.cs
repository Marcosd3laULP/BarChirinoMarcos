namespace Bar.Models;
public class Pedido
{
    public int IdPedido { get; set; }
    public int IdUsuario { get; set; }
    public int Monto { get; set; }
    public DateTime Fecha { get; set; }
    public string Estado { get; set; }
}
