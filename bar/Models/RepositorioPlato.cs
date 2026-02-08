using MySql.Data.MySqlClient;
using Bar.Models;
using Bar.Data;

namespace Bar.Repositorios;

public class RepositorioPlato
{
    private readonly Database _database;

    public RepositorioPlato(Database database)
    {
        _database = database;
    }

    public void Crear(Plato p)
{
    using var conn = _database.GetConnection();
    conn.Open();

    var query = @"INSERT INTO Plato
    (Nombre, Ingredientes, Descripcion, Costo, Imagen, Estado, IdRes)
    VALUES (@Nombre, @Ingredientes, @Descripcion, @Costo, @Imagen, @Estado, @IdRes)";

    using var cmd = new MySqlCommand(query, conn);

    cmd.Parameters.AddWithValue("@Nombre", p.Nombre);
    cmd.Parameters.AddWithValue("@Ingredientes", p.Ingredientes);
    cmd.Parameters.AddWithValue("@Descripcion", p.Descripcion);
    cmd.Parameters.AddWithValue("@Costo", p.Costo);
    cmd.Parameters.AddWithValue("@Imagen", p.Imagen);
    cmd.Parameters.AddWithValue("@Estado", p.Estado);
    cmd.Parameters.AddWithValue("@IdRes", p.IdRes);

    cmd.ExecuteNonQuery();
}


     public List<Plato> ListarTodos()
    {
        var lista = new List<Plato>();

        using var conn = _database.GetConnection();
        conn.Open();

        var query = "SELECT * FROM Plato WHERE Estado = 1";
        using var cmd = new MySqlCommand(query, conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            lista.Add(Mapear(reader));
        }
        return lista;
    }

    public Plato? ObtenerPorId(int id)
    {
        using var conn = _database.GetConnection();
        conn.Open();

        var query = "SELECT * FROM Plato WHERE IdPlato = @id";
        using var cmd = new MySqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@id", id);

        using var reader = cmd.ExecuteReader();
        if(!reader.Read())
        return null;


    

    
    return Mapear(reader);
    }

    public List<Plato> ObtenerPorRestaurante(int idRestaurante)
    {
        var lista = new List<Plato>();

        using var conn = _database.GetConnection();
        conn.Open();

        var sql = @"SELECT * FROM Plato
                    WHERE IdRes = @id
                    AND Estado = 1";

        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@id", idRestaurante);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            lista.Add(Mapear(reader));
        }

        return lista;
    }


    public void Editar(Plato p)
    {
        using var conn = _database.GetConnection();
        conn.Open();

        var query = @"UPDATE Plato
                    SET Nombre=@nombre, Ingredientes=@ingredientes, Descripcion=@descripcion, Costo=@costo, Imagen=@imagen
                    WHERE IdPlato = @IdPlato";
        
        using var cmd = new MySqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@nombre", p.Nombre);
        cmd.Parameters.AddWithValue("@ingredientes", p.Ingredientes);
        cmd.Parameters.AddWithValue("@descripcion", p.Descripcion);
        cmd.Parameters.AddWithValue("@costo", p.Costo);
        cmd.Parameters.AddWithValue("@imagen", p.Imagen);
        cmd.Parameters.AddWithValue("@IdPlato", p.IdPlato);

        cmd.ExecuteNonQuery();
    }

    public void Baja(int id)
    {
        using var conn = _database.GetConnection();
        conn.Open();

        var query = "UPDATE Plato SET Estado = 0 WHERE IdPlato = @id";
        using var cmd = new MySqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@id", id);

        cmd.ExecuteNonQuery();
    }

    private Plato Mapear(MySqlDataReader reader)
    {
        return new Plato
        {
            IdPlato = reader.GetInt32("IdPlato"),
            Nombre = reader.GetString("Nombre"),
            Ingredientes = reader.GetString("Ingredientes"),
            Descripcion = reader.GetString("Descripcion"),
            Costo = reader.GetInt32("Costo"),
            Estado = reader.GetBoolean("Estado"),
            IdRes = reader.GetInt32("IdRes"),
            Imagen = reader.IsDBNull(reader.GetOrdinal("Imagen"))
            ? "/img/default-plato.png" : reader.GetString("Imagen")
        };
    }

    
    public int ContarPorRestaurante(int IdRes)
    {
        using var conn = _database.GetConnection();
        conn.Open();

        var query = "SELECT COUNT(*) FROM plato WHERE IdRes = @IdRes AND plato.Estado = true";
        using var cmd = new MySqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@IdRes", IdRes);

        return Convert.ToInt32(cmd.ExecuteScalar());
    }

    public List<Plato> ObtenerPorRestaurantePaginado(int IdRes, int page, int pageSize)
    {
        var lista = new List<Plato>();
        int offset = (page - 1) * pageSize;

        using var conn = _database.GetConnection();
        conn.Open();

        var query= @"SELECT IdPlato, Nombre, Costo, Imagen
        FROM plato
        WHERE IdRes = @IdRes AND plato.Estado = true
        ORDER BY IdPlato
        LIMIT @limit OFFSET @offset";

        using var cmd = new MySqlCommand(query, conn);

        cmd.Parameters.AddWithValue("@IdRes", IdRes);
        cmd.Parameters.AddWithValue("@limit", pageSize);
        cmd.Parameters.AddWithValue("@offset", offset);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            int colImagen = reader.GetOrdinal("Imagen");

            lista.Add(new Plato
            {
                IdPlato = reader.GetInt32("IdPlato"),
                Nombre = reader.GetString("Nombre"),
                Costo = reader.GetInt32("Costo"),
                Imagen = reader.IsDBNull(colImagen) ? null : reader.GetString("Imagen")
            });
        }
        return lista;
    }

    public int ContarFiltrados(
    int IdRes,
    string? nombre,
    string? ingredientes,
    int? costoMax)
{
    using var conn = _database.GetConnection();
    conn.Open();

    var sql = @"SELECT COUNT(*) FROM plato
                WHERE IdRes = @IdRes AND Estado = true";

    var cmd = new MySqlCommand();
    cmd.Connection = conn;
    cmd.Parameters.AddWithValue("@IdRes", IdRes);

    if (!string.IsNullOrWhiteSpace(nombre))
    {
        sql += " AND Nombre LIKE @nombre";
        cmd.Parameters.AddWithValue("@nombre", $"%{nombre}%");
    }

    if (!string.IsNullOrWhiteSpace(ingredientes))
    {
        var palabras = ingredientes.Split(',', StringSplitOptions.RemoveEmptyEntries);
        int i = 0;
        foreach (var ing in palabras)
        {
            sql += $" AND Ingredientes LIKE @ing{i}";
            cmd.Parameters.AddWithValue($"@ing{i}", $"%{ing.Trim()}%");
            i++;
        }
    }

    if (costoMax.HasValue)
    {
        sql += " AND Costo <= @costoMax";
        cmd.Parameters.AddWithValue("@costoMax", costoMax.Value);
    }

    cmd.CommandText = sql;
    return Convert.ToInt32(cmd.ExecuteScalar());
}

public List<Plato> ObtenerFiltradosPaginado(
    int IdRes,
    int page,
    int pageSize,
    string? nombre,
    string? ingredientes,
    int? costoMax)
{
    var lista = new List<Plato>();
    int offset = (page - 1) * pageSize;

    using var conn = _database.GetConnection();
    conn.Open();

    var sql = @"SELECT IdPlato, Nombre, Costo, Imagen
                FROM plato
                WHERE IdRes = @IdRes AND Estado = true";

    var cmd = new MySqlCommand();
    cmd.Connection = conn;
    cmd.Parameters.AddWithValue("@IdRes", IdRes);

    if (!string.IsNullOrWhiteSpace(nombre))
    {
        sql += " AND Nombre LIKE @nombre";
        cmd.Parameters.AddWithValue("@nombre", $"%{nombre}%");
    }

    if (!string.IsNullOrWhiteSpace(ingredientes))
    {
        var palabras = ingredientes.Split(',', StringSplitOptions.RemoveEmptyEntries);
        int i = 0;
        foreach (var ing in palabras)
        {
            sql += $" AND Ingredientes LIKE @ing{i}";
            cmd.Parameters.AddWithValue($"@ing{i}", $"%{ing.Trim()}%");
            i++;
        }
    }

    if (costoMax.HasValue)
    {
        sql += " AND Costo <= @costoMax";
        cmd.Parameters.AddWithValue("@costoMax", costoMax.Value);
    }

    sql += @" ORDER BY IdPlato
              LIMIT @limit OFFSET @offset";

    cmd.Parameters.AddWithValue("@limit", pageSize);
    cmd.Parameters.AddWithValue("@offset", offset);
    cmd.CommandText = sql;

    using var reader = cmd.ExecuteReader();
    while (reader.Read())
    {
        int colImagen = reader.GetOrdinal("Imagen");
        lista.Add(new Plato
        {
            IdPlato = reader.GetInt32("IdPlato"),
            Nombre = reader.GetString("Nombre"),
            Costo = reader.GetInt32("Costo"),
            Imagen = reader.IsDBNull(colImagen) ? null : reader.GetString("Imagen")
        });
    }

    return lista;
}

/////////////   PARTE DEL CLIENTE   ///////////////

public int ContarFiltradosPublicos(
    string? nombre,
    string? ingredientes,
    int? costoMax
)
    {
        using var conn = _database.GetConnection();
        conn.Open();
        
        var query = @"SELECT COUNT(*) FROM plato 
                    WHERE Estado = true";
        using var cmd = new MySqlCommand(query, conn);

        if (!string.IsNullOrWhiteSpace(nombre))
        {
            query += " AND Nombre LIKE @nombre";
            cmd.Parameters.AddWithValue("@nombre", $"%{nombre}%");

        }

        if (!string.IsNullOrWhiteSpace(ingredientes))
        {
            var palabras = ingredientes.Split(',', StringSplitOptions.RemoveEmptyEntries);
            int i = 0;

            foreach (var ing in palabras)
            {
                query += $" AND Ingredientes LIKE @ing{i}";
                cmd.Parameters.AddWithValue($"@ing{i}", $"%{ing.Trim()}%");
                i++;         
            }
        }

        if (costoMax.HasValue)
        {
            query += " AND Costo <= @costoMax";
            cmd.Parameters.AddWithValue("@costoMax", costoMax.Value);

        }

        cmd.CommandText = query;
        return Convert.ToInt32(cmd.ExecuteScalar());
    }

    public List<Plato> ObtenerFiltradosPublicosPaginado(
        int page,
        int pageSize,
        string? nombre,
        string? ingredientes,
        int? costoMax
    )
    {
       var lista = new List<Plato>();
       int offset = (page -1) * pageSize;

       using var conn = _database.GetConnection();
       conn.Open();

       var query = @"SELECT IdPlato, Nombre, Costo, Imagen, IdRes
       FROM plato
       WHERE Estado = true"; 

       using var cmd = new MySqlCommand(query, conn);
       
       if (!string.IsNullOrWhiteSpace(nombre))
    {
        query += " AND Nombre LIKE @nombre";
        cmd.Parameters.AddWithValue("@nombre", $"%{nombre}%");
    }

    if (!string.IsNullOrWhiteSpace(ingredientes))
    {
        var palabras = ingredientes.Split(',', StringSplitOptions.RemoveEmptyEntries);
        int i = 0;
        foreach (var ing in palabras)
        {
            query += $" AND Ingredientes LIKE @ing{i}";
            cmd.Parameters.AddWithValue($"@ing{i}", $"%{ing.Trim()}%");
            i++;
        }
    }

    if (costoMax.HasValue)
    {
        query += " AND Costo <= @costoMax";
        cmd.Parameters.AddWithValue("@costoMax", costoMax.Value);
    }

    query += @" ORDER BY IdPlato
                LIMIT @limit OFFSET @offset";
    cmd.Parameters.AddWithValue("@limit", pageSize);
    cmd.Parameters.AddWithValue("@offset", offset);

    cmd.CommandText = query;

    using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            int colImagen = reader.GetOrdinal("Imagen");
            lista.Add(new Plato
            {
                IdPlato = reader.GetInt32("IdPlato"),
            Nombre = reader.GetString("Nombre"),
            Costo = reader.GetInt32("Costo"),
            Imagen = reader.IsDBNull(colImagen) ? null : reader.GetString("Imagen"),
            IdRes = reader.GetInt32("IdRes")
            });
        }
        return lista;
    }

}