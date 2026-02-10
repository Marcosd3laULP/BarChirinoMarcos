namespace Bar.Models;

public class PedidoClienteVM
{
    public int IdPedido { get; set; }
    public DateTime Fecha { get; set; }
    public string Estado { get; set; }
    public decimal Monto { get; set; }

    public string NombrePlato { get; set; }
    public string Bebida { get; set; }
    public string Guarnicion { get; set; }
    public string Aderezo { get; set; }
}
