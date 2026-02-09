using MySql.Data.MySqlClient;
using Bar.Models;
using Bar.Data;

namespace Bar.Repositorios;

public class RepositorioPedidoDetalle
{
    private readonly Database _database;

    public RepositorioPedidoDetalle(Database database)
    {
        _database = database;
    }

    public void Crear(PedidoDetalle d)
    {
        using var conn = _database.GetConnection();
        conn.Open();

        var sql = @"INSERT INTO pedidodetalle
        (IdPlato, IdPedido, IdGuarnicion, IdBebida, IdAderezo, SubTotal)
        VALUES (@IdPlato, @IdPedido, @IdGuarnicion, @IdBebida, @IdAderezo, @SubTotal)";

        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@IdPlato", d.IdPlato);
        cmd.Parameters.AddWithValue("@IdPedido", d.IdPedido);
        cmd.Parameters.AddWithValue("@IdGuarnicion", d.IdGuarnicion);
        cmd.Parameters.AddWithValue("@IdBebida", d.IdBebida);
        cmd.Parameters.AddWithValue("@IdAderezo", d.IdAderezo);
        cmd.Parameters.AddWithValue("@SubTotal", d.SubTotal);

        cmd.ExecuteNonQuery();
    }

    public List<PedidoDetalle> ObtenerPorPedido(int idPedido)
    {
        var lista = new List<PedidoDetalle>();

        using var conn = _database.GetConnection();
        conn.Open();

        var sql = "SELECT * FROM pedidodetalle WHERE IdPedido = @id";
        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@id", idPedido);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            lista.Add(new PedidoDetalle
            {
                IdDetalle = reader.GetInt32("IdDetalle"),
                IdPlato = reader.GetInt32("IdPlato"),
                IdPedido = reader.GetInt32("IdPedido"),
                IdGuarnicion = reader.IsDBNull(reader.GetOrdinal("IdGuarnicion")) ? null : reader.GetInt32("IdGuarnicion"),
                IdBebida = reader.IsDBNull(reader.GetOrdinal("IdBebida")) ? null : reader.GetInt32("IdBebida"),
                IdAderezo = reader.IsDBNull(reader.GetOrdinal("IdAderezo")) ? null : reader.GetInt32("IdAderezo"),
                SubTotal = reader.GetInt32("SubTotal")
            });
        }

        return lista;
    }
}
