using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Bar.Models;
using Bar.Repositorios;

[Authorize(Roles = "resto")]
public class RestoController : Controller
{
    private readonly RepositorioRestaurante _repo;

    public RestoController(RepositorioRestaurante repo)
    {
        _repo = repo;
    }

    public IActionResult Index()
    {
        var lista = _repo.ListarTodos();
        return View(lista);
    }

    public IActionResult Crear()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Crear(Restaurante r)
    {
        r.IdUsuario = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        _repo.Crear(r);
        return RedirectToAction("Index");
    }

    public IActionResult Editar(int id)
    {
        var r = _repo.ObtenerPorId(id);
        if (r == null) return NotFound();
        return View(r);
    }

    [HttpPost]
    public IActionResult Editar(Restaurante r)
    {
        _repo.Editar(r);
        return RedirectToAction("Index");
    }

    public IActionResult Baja(int id)
    {
        _repo.Baja(id);
      return  RedirectToAction("Index");
    }
}