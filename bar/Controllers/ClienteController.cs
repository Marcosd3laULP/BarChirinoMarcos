using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Bar.Models;
using Bar.Repositorios;

[Authorize(Roles = "user")]
public class ClienteController : Controller
{
    private readonly RepositorioUsuario _repo;
    private readonly RepositorioPlato repoPlato;
    private readonly RepositorioRestaurante repoResto;

    public ClienteController(
        RepositorioUsuario repo,
        RepositorioPlato _repoPlato,
        RepositorioRestaurante _repoResto)
    {
        _repo = repo;
        repoPlato = _repoPlato;
        repoResto = _repoResto;
    }

    public IActionResult Index()
    {
        return View();
    }

     public IActionResult ExplorarPlatos()
    {    
        return View();
    }

    public IActionResult Detalle(int id)
    {
        var plato = repoPlato.ObtenerPorId(id);
        return View(plato);
    }

    [HttpGet]
    public IActionResult GetPlatosPublicos(
        int page =1, 
        int pageSize = 6,
        string? nombre = null,
        string? ingredientes = null,
        int? costoMax = null)
    {

        
        int total = repoPlato.ContarFiltradosPublicos(
            nombre,
            ingredientes,
            costoMax);

        var platos = repoPlato.ObtenerFiltradosPublicosPaginado( 
            page, 
            pageSize,
            nombre,
            ingredientes,
            costoMax);

        return Json(new
        {
            data = platos,
            total = total,
            page = page,
            pageSize = pageSize
        });
    }

    public IActionResult ExplorarPorRestaurante(int idRes)
{
    ViewBag.IdRes = idRes;
    return View();
}

[HttpGet]
public IActionResult GetPlatosPorRestaurante(
    int idRes,
    int page = 1,
    int pageSize = 6
)
{
    var platos = repoPlato.ObtenerPorRestaurantePaginado(
        idRes,
        page,
        pageSize
    );

    int total = repoPlato.ContarPorRestaurante(idRes);

    return Json(new
    {
        data = platos,
        total = total
    });
}

public IActionResult ExplorarRestaurantes()
{
    return View();
}

[HttpGet]
public IActionResult GetRestaurantesPublicos(
    int page = 1,
    int pageSize = 6,
    string? nombre = null,
    string? ubicacion = null,
    string? especialidad = null
)
{
    int total = repoResto.ContarRestaurantesFiltradosPublicos(
        nombre,
        ubicacion,
        especialidad
    );

    var restaurantes = repoResto.ObtenerRestaurantesFiltradosPublicosPaginado(
        page,
        pageSize,
        nombre,
        ubicacion,
        especialidad
    );

    return Json(new
    {
        data = restaurantes,
        total = total,
        page = page,
        pageSize = pageSize
    });
}


}