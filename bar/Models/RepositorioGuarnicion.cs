using MySql.Data.MySqlClient;
using Bar.Models;
using Bar.Data;

namespace Bar.Repositorios;

public class RepositorioGuarnicion
{
    private readonly Database _database;

    public RepositorioGuarnicion(Database database)
    {
        _database = database;
    }

    public List<Guarnicion> ListarTodos()
    {
        var lista = new List<Guarnicion>();

        using var conn = _database.GetConnection();
        conn.Open();

        var sql = "SELECT * FROM guarnicion";
        using var cmd = new MySqlCommand(sql, conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            lista.Add(new Guarnicion
            {
                IdGuarnicion = reader.GetInt32("IdGuarnicion"),
                Nombre = reader.GetString("Nombre"),
                Tipo = reader.GetString("Tipo")
            });
        }

        return lista;
    }
}
