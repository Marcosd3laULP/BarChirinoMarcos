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
                    WHERE IdPlato = @id";
        
        using var cmd = new MySqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@nombre", p.Nombre);
        cmd.Parameters.AddWithValue("@ingredientes", p.Ingredientes);
        cmd.Parameters.AddWithValue("@descripcion", p.Descripcion);
        cmd.Parameters.AddWithValue("@costo", p.Costo);
        cmd.Parameters.AddWithValue("@imagen", p.Imagen);

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
}