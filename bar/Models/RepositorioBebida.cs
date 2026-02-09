using MySql.Data.MySqlClient;
using Bar.Models;
using Bar.Data;

namespace Bar.Repositorios;

public class RepositorioBebida
{
    private readonly Database _database;

    public RepositorioBebida(Database database)
    {
        _database = database;
    }

    public List<Bebida> ListarTodos()
    {
        var lista = new List<Bebida>();

        using var conn = _database.GetConnection();
        conn.Open();

        var sql = "SELECT * FROM bebida";
        using var cmd = new MySqlCommand(sql, conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            lista.Add(new Bebida
            {
                IdBebida = reader.GetInt32("IdBebida"),
                Nombre = reader.GetString("Nombre"),
                Tipo = reader.GetString("Tipo"),
                Costo = reader.GetInt32("Costo")
            });
        }

        return lista;
    }

    public Bebida? ObtenerPorId(int id)
{
    using var conn = _database.GetConnection();
    conn.Open();

    var sql = "SELECT * FROM bebida WHERE IdBebida = @id";
    using var cmd = new MySqlCommand(sql, conn);
    cmd.Parameters.AddWithValue("@id", id);

    using var reader = cmd.ExecuteReader();
    if (!reader.Read()) return null;

    return new Bebida
    {
        IdBebida = reader.GetInt32("IdBebida"),
        Nombre = reader.GetString("Nombre"),
        Tipo = reader.GetString("Tipo"),
        Costo = reader.GetInt32("Costo")
    };
}

}
