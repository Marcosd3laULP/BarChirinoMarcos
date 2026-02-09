namespace Bar.Models;

public class PedidoDetalle
{
    public int IdDetalle { get; set; }
    public int IdPlato { get; set; }
    public int IdPedido { get; set; }

    public int? IdGuarnicion { get; set; }
    public int? IdBebida { get; set; }
    public int? IdAderezo { get; set; }

    public int SubTotal { get; set; }
}
