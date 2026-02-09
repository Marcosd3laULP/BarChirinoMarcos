namespace Bar.Models;

public class PedidoDetalleVM
{
    public int IdPedido { get; set; }
    public int IdPlato { get; set; }

    public Plato? Plato { get; set; }

    public List<Bebida>? Bebidas { get; set; }
    public List<Guarnicion>? Guarniciones { get; set; }
    public List<Aderezo>? Aderezos { get; set; }

    // selección del usuario
    public int? IdBebida { get; set; }
    public int? IdGuarnicion { get; set; }
    public int? IdAderezo { get; set; }

    public int SubTotal { get; set; }
}
