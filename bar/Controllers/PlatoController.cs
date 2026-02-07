using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Bar.Models;
using Bar.Repositorios;
using Bar.Data;

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

        return View(); //Vue ahora se encarga del listado
    }

    [HttpGet]
    public IActionResult GetPlatos(
        int page =1, 
        int pageSize = 6,
        string? nombre = null,
        string? ingredientes = null,
        int? costoMax = null)
    {
         int idUsuario = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var restaurante = repoResto.BuscarPorUsuario(idUsuario);
        
        if(restaurante == null)
            return Json(new {data = new List<Plato>(), total = 0});

        
        int total = repoPlato.ContarFiltrados(
            restaurante.IdRes,
            nombre,
            ingredientes,
            costoMax);

        var platos = repoPlato.ObtenerFiltradosPaginado(
            restaurante.IdRes, 
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
    public IActionResult Editar(Plato p, IFormFile? Imagen)
    {

        if(Imagen != null && Imagen.Length > 0)
        {
            var fileName = Guid.NewGuid() + Path.GetExtension(Imagen.FileName);
            var path = Path.Combine("wwwroot/platos", fileName);

            using var stream = new FileStream(path, FileMode.Create);
            Imagen.CopyTo(stream);

            p.Imagen = "/platos/" + fileName;
        }
        
        repoPlato.Editar(p);
        return RedirectToAction("Index");
    }

   
 [HttpDelete]
    public IActionResult BajaAjax(int id)
    {
        try
        {
            Console.WriteLine("ENTRÓ A BAJA AJAX, ID = " + id);
            repoPlato.Baja(id); // baja lógica en MySQL
            return Ok();
        }
        catch
        {
            return BadRequest();
        }
    }


    public IActionResult Detalle(int id)
    {
        var plato = repoPlato.ObtenerPorId(id);
        return View(plato);
    }
}