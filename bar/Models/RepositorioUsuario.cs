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
    }
}
