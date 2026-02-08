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
                (@Nombre, @Apellido, @Nick, @Email, @PasswordHash, @Telefono, @Domicilio, @Rol, @Avatar, Estado)";

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

            var sql = "SELECT * FROM Usuario WHERE Email = @Email";
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
    }
}
