using MySql.Data.MySqlClient;
using Bar.Data;
using Bar.Models;

using Bar.Security;

namespace Bar.Repositorios
{
    public class RepositorioUsuario
    {
        private readonly Database _database;

        public RepositorioUsuario(Database database)
        {
            _database = database;
        }

        public void Crear(Usuario usuario)
        {
            //usuario.Password = PassHasher.Hash(password);

            using var conn = _database.GetConnection();
            conn.Open();

            var sql = @"
                INSERT INTO Usuario
                (Nombre, Apellido, Nick, Email, PasswordHash, Telefono, Domicilio, Rol, Avatar, Estado)
                VALUES
                (@Nombre, @Apellido, @Nick, @Email, @PasswordHash, @Telefono, @Domicilio, @Rol, @Avatar, 1)";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
            cmd.Parameters.AddWithValue("@Apellido", usuario.Apellido);
            cmd.Parameters.AddWithValue("@Nick", usuario.Nick);
            cmd.Parameters.AddWithValue("@Email", usuario.Email);
            cmd.Parameters.AddWithValue("@PasswordHash", usuario.PasswordHash);
            cmd.Parameters.AddWithValue("@Telefono", usuario.Telefono);
            cmd.Parameters.AddWithValue("@Domicilio", usuario.Domicilio);
            cmd.Parameters.AddWithValue("@Rol", usuario.Rol.ToString());
            cmd.Parameters.AddWithValue("@Avatar", usuario.Avatar);
            cmd.Parameters.AddWithValue("@Estado", usuario.Estado);

            cmd.ExecuteNonQuery();

            //Console.WriteLine("HASH GUARDADO: " + usuario.PasswordHash);

        }

        public Usuario? ObtenerPorId(int id)
    {
        using var conn = _database.GetConnection();
        conn.Open();

        var query = "SELECT * FROM Usuario WHERE IdUsuario = @id";
        using var cmd = new MySqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@id", id);

        using var reader = cmd.ExecuteReader();
        if(!reader.Read())
        return null;

    
    return Mapear(reader);
    }


    public void Editar(Usuario u)
    {
        using var conn = _database.GetConnection();
        conn.Open();

        var query = @"UPDATE Usuario
                    SET Nombre=@nombre, Apellido=@apellido, Email=@email, Nick=@nick, 
                    Domicilio=@domicilio, Telefono=@telefono, Avatar=@avatar
                    WHERE IdUsuario = @IdUsuario";
        
        using var cmd = new MySqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@nombre", u.Nombre);
        cmd.Parameters.AddWithValue("@apellido", u.Apellido);
        cmd.Parameters.AddWithValue("@nick", u.Nick);
        cmd.Parameters.AddWithValue("@email", u.Email);
        cmd.Parameters.AddWithValue("@domicilio", u.Domicilio);
        cmd.Parameters.AddWithValue("@telefono", u.Telefono);
        cmd.Parameters.AddWithValue("@avatar", u.Avatar);
         cmd.Parameters.AddWithValue("@IdUsuario", u.IdUsuario);
        cmd.ExecuteNonQuery();
    }

    public void Baja(int id)
    {
        using var conn = _database.GetConnection();
        conn.Open();

        var query = "UPDATE Usuario SET Estado = 0 WHERE IdUsuario = @id";
        using var cmd = new MySqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@id", id);

        cmd.ExecuteNonQuery();
    }


        public UsuarioDTO? Login(string email, string password)
        {
            using var conn = _database.GetConnection();
            conn.Open();

            var sql = "SELECT * FROM Usuario WHERE Email = @Email AND Estado = 1";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Email", email);

            using var reader = cmd.ExecuteReader();
            if (!reader.Read())
                return null;

            
            var storedHash = reader.GetString("PasswordHash");

            //.WriteLine("HASH BD: " + storedHash);
            //Console.WriteLine("PASS INGRESADA: " + password);


            if (!PassHasher.Verify(password, storedHash))
                return null;

            return new UsuarioDTO
            {
                IdUsuario = reader.GetInt32("IdUsuario"),
                Nombre = reader.GetString("Nombre"),
                Apellido = reader.GetString("Apellido"),
                Nick = reader.GetString("Nick"),
                Email = reader.GetString("Email"),
                Rol = Enum.Parse<RolUsuario>(reader.GetString("Rol"))

            };
        }

         private Usuario Mapear(MySqlDataReader reader)
    {
        return new Usuario
        {
            IdUsuario = reader.GetInt32("IdUsuario"),
            Nombre = reader.GetString("Nombre"),
            Apellido = reader.GetString("Apellido"),
            Nick = reader.GetString("Nick"),
            Email = reader.GetString("Email"),
            PasswordHash = reader.GetString("PasswordHash"),
            Domicilio = reader.GetString("Domicilio"),
            Telefono = reader.GetString("Telefono"),
            Estado = reader.GetBoolean("Estado"),
            Avatar = reader.IsDBNull(reader.GetOrdinal("Avatar"))
            ? "/img/default-avatar.png" : reader.GetString("Avatar")
        };
    }

    public void BajaConReglas(int idUsuario)
{
    using var conn = _database.GetConnection();
    conn.Open();

    using var transaction = conn.BeginTransaction();

    try
    {
        var getRolQuery = "SELECT Rol FROM Usuario WHERE IdUsuario = @id";
        using var cmdRol = new MySqlCommand(getRolQuery, conn, transaction);
        cmdRol.Parameters.AddWithValue("@id", idUsuario);

        var rol = cmdRol.ExecuteScalar()?.ToString();

        if (rol == null)
            throw new Exception("Usuario no encontrado");

      
        var bajaUsuario = "UPDATE Usuario SET Estado = 0 WHERE IdUsuario = @id";
        using var cmdUser = new MySqlCommand(bajaUsuario, conn, transaction);
        cmdUser.Parameters.AddWithValue("@id", idUsuario);
        cmdUser.ExecuteNonQuery();


        if (rol == "resto")
        {
            // Buscar restaurante
            var getResto = "SELECT IdRes FROM Restaurante WHERE IdUsuario = @id";
            using var cmdResto = new MySqlCommand(getResto, conn, transaction);
            cmdResto.Parameters.AddWithValue("@id", idUsuario);

            var idResObj = cmdResto.ExecuteScalar();

            if (idResObj != null)
            {
                int idRes = Convert.ToInt32(idResObj);

                // Baja restaurante
                var bajaResto = "UPDATE Restaurante SET Estado = 0 WHERE IdRes = @idRes";
                using var cmdBajaResto = new MySqlCommand(bajaResto, conn, transaction);
                cmdBajaResto.Parameters.AddWithValue("@idRes", idRes);
                cmdBajaResto.ExecuteNonQuery();

                // Baja platos
                var bajaPlatos = "UPDATE Plato SET Estado = 0 WHERE IdRes = @idRes";
                using var cmdBajaPlatos = new MySqlCommand(bajaPlatos, conn, transaction);
                cmdBajaPlatos.Parameters.AddWithValue("@idRes", idRes);
                cmdBajaPlatos.ExecuteNonQuery();
            }
        }

        transaction.Commit();
    }
    catch
    {
        transaction.Rollback();
        throw;
    }
}

    //////////// PAGINADOS Y BUSCADOR //////////////////
    public int ContarUsuariosFiltrados(
    string? nick,
    string? email,
    List<string>? roles
)
{
    using var conn = _database.GetConnection();
    conn.Open();

    var query = @"SELECT COUNT(*) FROM usuario
                  WHERE 1=1 AND Estado = true";

    using var cmd = new MySqlCommand();
    cmd.Connection = conn;

    if (!string.IsNullOrWhiteSpace(nick))
    {
        query += " AND Nick LIKE @nick";
        cmd.Parameters.AddWithValue("@nick", $"%{nick}%");
    }

    if (!string.IsNullOrWhiteSpace(email))
    {
        query += " AND Email LIKE @email";
        cmd.Parameters.AddWithValue("@email", $"%{email}%");
    }

    if (roles != null && roles.Any())
    {
        query += " AND Rol IN (";

        for (int i = 0; i < roles.Count; i++)
        {
            query += $"@rol{i}";
            if (i < roles.Count - 1)
                query += ",";

            cmd.Parameters.AddWithValue($"@rol{i}", roles[i]);
        }

        query += ")";
    }

        cmd.CommandText = query;

        return Convert.ToInt32(cmd.ExecuteScalar());
    }

    public List<UsuarioListadoVM> ObtenerUsuariosPaginado(
    int page,
    int pageSize,
    string? nick,
    string? email,
    List<string>? roles
)
{
    var lista = new List<UsuarioListadoVM>();
    int offset = (page - 1) * pageSize;

    using var conn = _database.GetConnection();
    conn.Open();

    var query = @"SELECT IdUsuario, Nick, Email, Rol, Estado
                  FROM usuario
                  WHERE Estado = true";

    using var cmd = new MySqlCommand();
    cmd.Connection = conn;

    if (!string.IsNullOrWhiteSpace(nick))
    {
        query += " AND Nick LIKE @nick";
        cmd.Parameters.AddWithValue("@nick", $"%{nick}%");
    }

    if (!string.IsNullOrWhiteSpace(email))
    {
        query += " AND Email LIKE @email";
        cmd.Parameters.AddWithValue("@email", $"%{email}%");
    }

    if (roles != null && roles.Any())
    {
        query += " AND Rol IN (";

        for (int i = 0; i < roles.Count; i++)
        {
            query += $"@rol{i}";
            if (i < roles.Count - 1)
                query += ",";

            cmd.Parameters.AddWithValue($"@rol{i}", roles[i]);
        }

        query += ")";
    }

    query += @" ORDER BY IdUsuario
                LIMIT @limit OFFSET @offset";

    cmd.Parameters.AddWithValue("@limit", pageSize);
    cmd.Parameters.AddWithValue("@offset", offset);

    cmd.CommandText = query;

    using var reader = cmd.ExecuteReader();
    while (reader.Read())
    {
        lista.Add(new UsuarioListadoVM
        {
            IdUsuario = reader.GetInt32("IdUsuario"),
            Nick = reader.GetString("Nick"),
            Email = reader.GetString("Email"),
            Rol = reader.GetString("Rol"),
            Estado = reader.GetBoolean("Estado")
        });
    }

        return lista;
    }   


    }
}
