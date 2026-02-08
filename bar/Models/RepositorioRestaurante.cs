using MySql.Data.MySqlClient;
using Bar.Models;
using Bar.Data;

namespace Bar.Repositorios;

public class RepositorioRestaurante
{
    private readonly Database _database;
    private string connectionString;

    public RepositorioRestaurante(Database database)
    {
        _database = database;
    }

    public void Crear(Restaurante resto)
    {
        using var conn = _database.GetConnection();
        conn.Open();

        var query = @"INSERT INTO Restaurante
        (Nombre, Ubicacion, Especialidad, Imagen, IdUsuario, Estado)
        VALUES (@Nombre, @Ubicacion, @Especialidad, @Imagen, @IdUsuario, 1)";

        using var cmd = new MySqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@Nombre", resto.Nombre);
        cmd.Parameters.AddWithValue("@Ubicacion", resto.Ubicacion);
        cmd.Parameters.AddWithValue("@Especialidad", resto.Especialidad);
        cmd.Parameters.AddWithValue("@Imagen", resto.Imagen);
        cmd.Parameters.AddWithValue("@IdUsuario", resto.IdUsuario);
        
        cmd.ExecuteNonQuery();
    }

    public List<Restaurante> ListarTodos()
    {
        var lista = new List<Restaurante>();

        using var conn = _database.GetConnection();
        conn.Open();

        var query = "SELECT * FROM Restaurante WHERE Estado = 1";
        using var cmd = new MySqlCommand(query, conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            lista.Add(Mapear(reader));
        }
        return lista;
    }

    public Restaurante? ObtenerPorId(int id)
    {
        using var conn = _database.GetConnection();
        conn.Open();

        var query = "SELECT * FROM Restaurante WHERE IdRes = @id";
        using var cmd = new MySqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@id", id);

        using var reader = cmd.ExecuteReader();
        if(!reader.Read())
        return null;

    
    return Mapear(reader);
    }


    public void Editar(Restaurante r)
    {
        using var conn = _database.GetConnection();
        conn.Open();

        var query = @"UPDATE Restaurante
                    SET Nombre=@nombre, Ubicacion=@ubicacion, Especialidad=@especialidad, Imagen=@imagen
                    WHERE IdRes = @IdRes";
        
        using var cmd = new MySqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@nombre", r.Nombre);
        cmd.Parameters.AddWithValue("@ubicacion", r.Ubicacion);
        cmd.Parameters.AddWithValue("@Especialidad", r.Especialidad);
        cmd.Parameters.AddWithValue("@Imagen", r.Imagen);
        cmd.Parameters.AddWithValue("@IdRes", r.IdRes);

        cmd.ExecuteNonQuery();
    }

    public void Baja(int id)
    {
        using var conn = _database.GetConnection();
        conn.Open();

        var query = "UPDATE Restaurante SET Estado = 0 WHERE IdRes = @id";
        using var cmd = new MySqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@id", id);

        cmd.ExecuteNonQuery();
    }

    private Restaurante Mapear(MySqlDataReader reader)
    {
        return new Restaurante
        {
            IdRes = reader.GetInt32("IdRes"),
            Nombre = reader.GetString("Nombre"),
            Ubicacion = reader.GetString("Ubicacion"),
            Especialidad = reader.GetString("Especialidad"),
            Estado = reader.GetBoolean("Estado"),
            IdUsuario = reader.GetInt32("IdUsuario"),
            Imagen = reader.IsDBNull(reader.GetOrdinal("Imagen"))
            ? "/img/default-restaurante.jpg" : reader.GetString("Imagen")
        };
    }

    public Restaurante? BuscarPorUsuario(int idUsuario)
    {
        using var conn = _database.GetConnection();
        conn.Open();

        var query = "SELECT * FROM Restaurante WHERE IdUsuario = @id AND Estado = true";
        using var cmd = new MySqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@id", idUsuario);

        using var reader = cmd.ExecuteReader();
        if(!reader.Read())
        return null;

    return Mapear(reader);
    }
}