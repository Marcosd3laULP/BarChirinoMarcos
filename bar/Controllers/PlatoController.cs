using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Bar.Models;
using Bar.Repositorios;

[Authorize (Roles = "resto")]
public class PlatoController : Controller
{
    private readonly RepositorioPlato repoPlato;
    private readonly RepositorioRestaurante repoResto;

    public PlatoController(
        RepositorioPlato _repoPlato, 
        RepositorioRestaurante _repoResto)
    {
        repoPlato = _repoPlato;
        repoResto = _repoResto;
    }

    public IActionResult Index()
    {
        int idUsuario = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var restaurante = repoResto.BuscarPorUsuario(idUsuario);
        if(restaurante == null)
            return RedirectToAction("Crear", "Restaurante");

        var platos = repoPlato.ObtenerPorRestaurante(restaurante.IdRes);
        return View(platos);
    }

    public IActionResult Crear()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Crear(Plato p, IFormFile? Imagen)
    {
        int idUsuario = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var restaurante = repoResto.BuscarPorUsuario(idUsuario);
        if(restaurante == null)
            return RedirectToAction("Index");
        
        if(Imagen != null && Imagen.Length > 0)
        {
            var fileName = Guid.NewGuid() + Path.GetExtension(Imagen.FileName);
            var path = Path.Combine("wwwroot/platos", fileName);

            using var stream = new FileStream(path, FileMode.Create);
            Imagen.CopyTo(stream);

            p.Imagen = "/platos/" + fileName;
        }

        p.IdRes = restaurante.IdRes;
        repoPlato.Crear(p);

    return RedirectToAction("Index");
    }

    public IActionResult Editar(int id)
    {
        var plato = repoPlato.ObtenerPorId(id);
        if(plato == null)
        return NotFound();

        return View(plato);
    }

    [HttpPost]
    public IActionResult Editar(Plato p)
    {
        repoPlato.Editar(p);
        return RedirectToAction("Index");
    }

    public IActionResult Baja(int id)
    {
       repoPlato.Baja(id);
       return RedirectToAction("Index"); 
    }
}