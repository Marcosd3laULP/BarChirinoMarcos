using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Bar.Models;
using Bar.Repositorios;
using Bar.Data;

public class AdminController : Controller
{
    private readonly RepositorioUsuario _repo;
    

    public AdminController(RepositorioUsuario repo)
    {
        _repo = repo;
    }

[Authorize(Roles = "admin")]

public IActionResult Index()
    {
        return View();
    }


[Authorize(Roles = "admin")]
[HttpPost]
public IActionResult BajaUsuario(int idUsuario)
    {
        _repo.BajaConReglas(idUsuario);
        return Ok();
    }

    //PAGINADO DE USUARIOS:
[Authorize(Roles = "admin")]
    public IActionResult Lista()
    {
        return View();
    }
[Authorize(Roles = "admin")]
[HttpGet]
public IActionResult GetUsuarios(
    int page = 1,
    int pageSize = 6,
    string? nick = null,
    string? email = null,
    List<string>? roles = null
)
{
    int total = _repo.ContarUsuariosFiltrados(
        nick,
        email,
        roles
    );

    var usuarios = _repo.ObtenerUsuariosPaginado(
        page,
        pageSize,
        nick,
        email,
        roles
    );

    return Json(new
    {
        data = usuarios,
        total,
        page,
        pageSize
    });
}
}
