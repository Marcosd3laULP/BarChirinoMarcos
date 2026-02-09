using MySql.Data.MySqlClient;
using Bar.Models;
using Bar.Data;

namespace Bar.Repositorios;

public class RepositorioAderezo
{
    private readonly Database _database;

    public RepositorioAderezo(Database database)
    {
        _database = database;
    }

    public void Crear(Aderezo a)
    {
        using var conn = _database.GetConnection();
        conn.Open();

        var sql = @"INSERT INTO aderezo (Nombre, Tipo, Costo)
                    VALUES (@Nombre, @Tipo, @Costo)";

        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Nombre", a.Nombre);
        cmd.Parameters.AddWithValue("@Tipo", a.Tipo);

        cmd.ExecuteNonQuery();
    }

    public List<Aderezo> ListarTodos()
    {
        var lista = new List<Aderezo>();

        using var conn = _database.GetConnection();
        conn.Open();

        var sql = "SELECT * FROM aderezo";
        using var cmd = new MySqlCommand(sql, conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
            lista.Add(Mapear(reader));

        return lista;
    }

    public Aderezo? ObtenerPorId(int id)
    {
        using var conn = _database.GetConnection();
        conn.Open();

        var sql = "SELECT * FROM aderezo WHERE IdAderezo = @id";
        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@id", id);

        using var reader = cmd.ExecuteReader();
        if (!reader.Read()) return null;

        return Mapear(reader);
    }

    private Aderezo Mapear(MySqlDataReader reader)
    {
        return new Aderezo
        {
            IdAderezo = reader.GetInt32("IdAderezo"),
            Nombre = reader.GetString("Nombre"),
            Tipo = reader.GetString("Tipo")
        };
    }
}
