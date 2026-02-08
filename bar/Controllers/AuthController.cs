using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Bar.Models;
using Bar.Repositorios;
using Bar.Security;

public class AuthController : Controller
{
    private readonly RepositorioUsuario _repo;

    public AuthController(RepositorioUsuario repo)
    {
        _repo = repo;
    }

    //GET LOGIN
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    //POST LOGIN
    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        var user = _repo.Login(email, password);

        if (user == null)
        {
            ViewBag.Error = "Email o contraseña incorrectos";
            return View();
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.IdUsuario.ToString()),
            new Claim(ClaimTypes.Name, user.Nick),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Rol.ToString())
        };

        var identity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme
            );
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity)
        );

        if (user.Rol == RolUsuario.resto)
        {
            return RedirectToAction("Index", "Restaurante");
        }
        else if(user.Rol == RolUsuario.user)
        {
            return RedirectToAction("Index", "Cliente");
        }

        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction("Login");
    }

    public IActionResult Perfil()
{
    int idUsuario = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

    var usuario = _repo.ObtenerPorId(idUsuario);

    return View(usuario);
}

public IActionResult Editar()
{
    int idUsuario = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
    var usuario = _repo.ObtenerPorId(idUsuario);
    return View(usuario);
}

[HttpPost]
public IActionResult Editar(Usuario model, IFormFile avatar)
{
    if (avatar != null && avatar.Length > 0)
    {
        string carpeta = Path.Combine(Directory.GetCurrentDirectory(),
                                      "wwwroot/img/avatars");

        if (!Directory.Exists(carpeta))
            Directory.CreateDirectory(carpeta);

        string nombreArchivo = $"{Guid.NewGuid()}{Path.GetExtension(avatar.FileName)}";
        string rutaCompleta = Path.Combine(carpeta, nombreArchivo);

        using (var stream = new FileStream(rutaCompleta, FileMode.Create))
        {
            avatar.CopyTo(stream);
        }

        model.Avatar = "/img/avatars/" + nombreArchivo;
    }

    _repo.Editar(model);

    return RedirectToAction("Perfil");
}




    //REGISTRO DE NUEVO USUARIO//

    [HttpGet]
    public IActionResult Registro()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Registro(Usuario usuario, string password, IFormFile? Avatar)
    {
        usuario.PasswordHash = PassHasher.Hash(password);

        //PARA ASIGNAR UN ROL POR DEFECTO
        if (usuario.Rol == 0)
            usuario.Rol = RolUsuario.user;

        if (Avatar != null && Avatar.Length > 0)
        {
            var fileName = Guid.NewGuid() + Path.GetExtension(Avatar.FileName);
            var path = Path.Combine("wwwroot/avatars", fileName);

            using var stream = new FileStream(path, FileMode.Create);
            Avatar.CopyTo(stream);

            usuario.Avatar = "/avatars/" + fileName;
        }

        _repo.Crear(usuario);

        return RedirectToAction("Login");
    }
}