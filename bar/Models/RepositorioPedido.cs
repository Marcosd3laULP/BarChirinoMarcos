using MySql.Data.MySqlClient;
using Bar.Models;
using Bar.Data;

namespace Bar.Repositorios;
public class RepositorioPedido
{
    private readonly Database _database;

    public RepositorioPedido(Database database)
    {
        _database = database;
    }

    public int Crear(Pedido p)
    {
        using var conn = _database.GetConnection();
        conn.Open();

        var sql = @"INSERT INTO pedido (IdUsuario, Monto, Fecha, Estado)
                    VALUES (@IdUsuario, @Monto, @Fecha, @Estado);
                    SELECT LAST_INSERT_ID();";

        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@IdUsuario", p.IdUsuario);
        cmd.Parameters.AddWithValue("@Monto", p.Monto);
        cmd.Parameters.AddWithValue("@Fecha", p.Fecha);
        cmd.Parameters.AddWithValue("@Estado", p.Estado);

        return Convert.ToInt32(cmd.ExecuteScalar());
    }

    public List<Pedido> ObtenerPorUsuario(int idUsuario)
    {
        var lista = new List<Pedido>();

        using var conn = _database.GetConnection();
        conn.Open();

        var sql = "SELECT * FROM pedido WHERE IdUsuario = @id";
        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@id", idUsuario);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            lista.Add(new Pedido
            {
                IdPedido = reader.GetInt32("IdPedido"),
                IdUsuario = reader.GetInt32("IdUsuario"),
                Monto = reader.GetInt32("Monto"),
                Fecha = reader.GetDateTime("Fecha"),
                Estado = reader.GetString("Estado")
            });
        }

        return lista;
    }

    public Pedido? ObtenerPorId(int id)
{
    using var conn = _database.GetConnection();
    conn.Open();

    var sql = "SELECT * FROM pedido WHERE IdPedido = @id";
    using var cmd = new MySqlCommand(sql, conn);
    cmd.Parameters.AddWithValue("@id", id);

    using var reader = cmd.ExecuteReader();
    if (!reader.Read()) return null;

    return new Pedido
    {
        IdPedido = reader.GetInt32("IdPedido"),
        IdUsuario = reader.GetInt32("IdUsuario"),
        Monto = reader.GetInt32("Monto"),
        Fecha = reader.GetDateTime("Fecha"),
        Estado = reader.GetString("Estado")
    };
}

public void ActualizarMonto(int idPedido, int monto)
{
    using var conn = _database.GetConnection();
    conn.Open();

    var sql = "UPDATE pedido SET Monto = @monto WHERE IdPedido = @id";
    using var cmd = new MySqlCommand(sql, conn);
    cmd.Parameters.AddWithValue("@monto", monto);
    cmd.Parameters.AddWithValue("@id", idPedido);

    cmd.ExecuteNonQuery();
}

public void CambiarEstado(int idPedido, string estado)
{
    using var conn = _database.GetConnection();
    conn.Open();

    var sql = "UPDATE pedido SET Estado = @estado WHERE IdPedido = @id";
    using var cmd = new MySqlCommand(sql, conn);
    cmd.Parameters.AddWithValue("@estado", estado);
    cmd.Parameters.AddWithValue("@id", idPedido);

    cmd.ExecuteNonQuery();
}

public int ContarPedidosCliente(
    int idUsuario,
    DateTime? desde,
    DateTime? hasta
)
{
    using var conn = _database.GetConnection();
    conn.Open();

    var query = @"SELECT COUNT(*) 
                  FROM pedido 
                  WHERE IdUsuario = @idUsuario";

    using var cmd = new MySqlCommand(query, conn);
    cmd.Parameters.AddWithValue("@idUsuario", idUsuario);

    if (desde.HasValue)
    {
        query += " AND Fecha >= @desde";
        cmd.Parameters.AddWithValue("@desde", desde.Value);
    }

    if (hasta.HasValue)
    {
        query += " AND Fecha <= @hasta";
        cmd.Parameters.AddWithValue("@hasta", hasta.Value);
    }

    cmd.CommandText = query;
    return Convert.ToInt32(cmd.ExecuteScalar());
}

public List<Pedido> ObtenerPedidosClientePaginado(
    int idUsuario,
    int page,
    int pageSize,
    DateTime? desde,
    DateTime? hasta
)
{
    var lista = new List<Pedido>();
    int offset = (page - 1) * pageSize;

    using var conn = _database.GetConnection();
    conn.Open();

    var query = @"SELECT IdPedido, Monto, Estado, Fecha
                  FROM pedido
                  WHERE IdUsuario = @idUsuario";

    using var cmd = new MySqlCommand(query, conn);
    cmd.Parameters.AddWithValue("@idUsuario", idUsuario);

    if (desde.HasValue)
    {
        query += " AND Fecha >= @desde";
        cmd.Parameters.AddWithValue("@desde", desde.Value);
    }

    if (hasta.HasValue)
    {
        query += " AND Fecha <= @hasta";
        cmd.Parameters.AddWithValue("@hasta", hasta.Value);
    }

    query += @" ORDER BY Fecha DESC
                LIMIT @limit OFFSET @offset";

    cmd.Parameters.AddWithValue("@limit", pageSize);
    cmd.Parameters.AddWithValue("@offset", offset);

    cmd.CommandText = query;

    using var reader = cmd.ExecuteReader();
    while (reader.Read())
    {
        lista.Add(new Pedido
        {
            IdPedido = reader.GetInt32("IdPedido"),
            Monto = reader.GetInt32("Monto"),
            Estado = reader.GetString("Estado"),
            Fecha = reader.GetDateTime("Fecha")
        });
    }

    return lista;
}

public int ContarPedidosRestaurante(
    int idRestaurante,
    DateTime? desde,
    DateTime? hasta
)
{
    using var conn = _database.GetConnection();
    conn.Open();

    var query = @"
        SELECT COUNT(DISTINCT p.IdPedido)
        FROM pedido p
        INNER JOIN pedidodetalle pd ON p.IdPedido = pd.IdPedido
        INNER JOIN plato pl ON pd.IdPlato = pl.IdPlato
        WHERE pl.IdRes = @idRestaurante
    ";

    using var cmd = new MySqlCommand(query, conn);
    cmd.Parameters.AddWithValue("@idRestaurante", idRestaurante);

    if (desde.HasValue)
    {
        query += " AND p.Fecha >= @desde";
        cmd.Parameters.AddWithValue("@desde", desde.Value);
    }

    if (hasta.HasValue)
    {
        query += " AND p.Fecha <= @hasta";
        cmd.Parameters.AddWithValue("@hasta", hasta.Value);
    }

    cmd.CommandText = query;
    return Convert.ToInt32(cmd.ExecuteScalar());
}

public List<Pedido> ObtenerPedidosRestaurantePaginado(
    int idRestaurante,
    int page,
    int pageSize,
    DateTime? desde,
    DateTime? hasta
)
{
    var lista = new List<Pedido>();
    int offset = (page - 1) * pageSize;

    using var conn = _database.GetConnection();
    conn.Open();

    var query = @"
        SELECT DISTINCT p.IdPedido, p.Monto, p.Estado, p.Fecha
        FROM pedido p
        INNER JOIN pedidodetalle pd ON p.IdPedido = pd.IdPedido
        INNER JOIN plato pl ON pd.IdPlato = pl.IdPlato
        WHERE pl.IdRes = @idRestaurante
    ";

    using var cmd = new MySqlCommand(query, conn);
    cmd.Parameters.AddWithValue("@idRestaurante", idRestaurante);

    if (desde.HasValue)
    {
        query += " AND p.Fecha >= @desde";
        cmd.Parameters.AddWithValue("@desde", desde.Value);
    }

    if (hasta.HasValue)
    {
        query += " AND p.Fecha <= @hasta";
        cmd.Parameters.AddWithValue("@hasta", hasta.Value);
    }

    query += @"
        ORDER BY p.Fecha DESC
        LIMIT @limit OFFSET @offset
    ";

    cmd.Parameters.AddWithValue("@limit", pageSize);
    cmd.Parameters.AddWithValue("@offset", offset);

    cmd.CommandText = query;

    using var reader = cmd.ExecuteReader();
    while (reader.Read())
    {
        lista.Add(new Pedido
        {
            IdPedido = reader.GetInt32("IdPedido"),
            Monto = reader.GetInt32("Monto"),
            Estado = reader.GetString("Estado"),
            Fecha = reader.GetDateTime("Fecha")
        });
    }

    return lista;
}


}
