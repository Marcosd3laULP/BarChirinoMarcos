using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Bar.Models;
using Bar.Repositorios;

[Authorize(Roles = "resto, admin")]
public class RestauranteController : Controller
{
    private readonly RepositorioRestaurante _repo;

    public RestauranteController(RepositorioRestaurante repo)
    {
        _repo = repo;
    }

    public IActionResult Index()
    {
        int idUsuario = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var restaurante = _repo.BuscarPorUsuario(idUsuario);
        return View(restaurante);
    }

    public IActionResult Crear()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Crear(Restaurante r, IFormFile? Imagen)
    {
        r.IdUsuario = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        
        if(Imagen != null && Imagen.Length > 0)
        {
            var fileName = Guid.NewGuid() + Path.GetExtension(Imagen.FileName);
            var path = Path.Combine("wwwroot/restaurante", fileName);

            using var stream = new FileStream(path, FileMode.Create);
            Imagen.CopyTo(stream);

            r.Imagen = "/restaurante/" + fileName;
        }

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
    public IActionResult Editar(Restaurante r, IFormFile? Imagen)
    {

         if(Imagen != null && Imagen.Length > 0)
        {
            var fileName = Guid.NewGuid() + Path.GetExtension(Imagen.FileName);
            var path = Path.Combine("wwwroot/restaurante", fileName);

            using var stream = new FileStream(path, FileMode.Create);
            Imagen.CopyTo(stream);

            r.Imagen = "/restaurante/" + fileName;
        }
        _repo.Editar(r);
        return RedirectToAction("Index");
    }

    public IActionResult Baja(int id)
    {
        _repo.Baja(id);
      return  RedirectToAction("Index");
    }
}