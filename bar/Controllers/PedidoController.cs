using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Bar.Models;
using Bar.Repositorios;
using Bar.Data;

namespace Bar.Controllers;

public class PedidoController : Controller
{
    private readonly RepositorioPedido _repoPedido;
    private readonly RepositorioPedidoDetalle _repoPedidoDetalle;
    private readonly RepositorioPlato _repoPlato;
    private readonly RepositorioUsuario _repoUsuario;
    private readonly RepositorioBebida _repoBebida;
    private readonly RepositorioGuarnicion _repoGuarnicion;
    private readonly RepositorioAderezo _repoAderezo;

    private readonly RepositorioRestaurante _repoResto;

    public PedidoController(
        RepositorioPedido repoPedido,
        RepositorioPlato repoPlato,
        RepositorioPedidoDetalle repoPedidoDetalle,
        RepositorioUsuario repoUsuario,
        RepositorioBebida repoBebida,
        RepositorioGuarnicion repoGuarnicion,
        RepositorioAderezo repoAderezo,
        RepositorioRestaurante repoResto)
    {
        _repoPedido = repoPedido;
        _repoPlato = repoPlato;
        _repoUsuario = repoUsuario;
        _repoPedidoDetalle = repoPedidoDetalle;
        _repoBebida = repoBebida;
        _repoGuarnicion = repoGuarnicion;
        _repoAderezo = repoAderezo;
        _repoResto = repoResto;

    }

[Authorize(Roles = "user, admin")]
    [HttpPost]
public IActionResult CalcularSubtotal([FromBody] CalcularSubtotalDTO dto)
{
    var pedido = _repoPedido.ObtenerPorId(dto.IdPedido);
    if (pedido == null || pedido.Estado != "pendiente")
        return BadRequest();

    var plato = _repoPlato.ObtenerPorId(dto.IdPlato);
    if (plato == null)
        return BadRequest();

    int subtotal = plato.Costo;

    if (dto.IdBebida.HasValue)
    {
        var bebida = _repoBebida.ObtenerPorId(dto.IdBebida.Value);
        if (bebida != null)
            subtotal += bebida.Costo;
    }

    return Json(new { subTotal = subtotal });
}

[Authorize(Roles = "user, admin")]
[HttpGet]
public IActionResult CrearDetalle(int idPlato)
{

     int idUsuario = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

    var plato = _repoPlato.ObtenerPorId(idPlato);
    if (plato == null)
        return NotFound();
    
    var pedido = new Pedido
    {
        IdUsuario = idUsuario,
        Monto = plato.Costo,
        Fecha = DateTime.Now,
        Estado = "pendiente"
    };

    int idPedido = _repoPedido.Crear(pedido);

    var vm = new PedidoDetalleVM
    {
        IdPedido = idPedido,
        IdPlato = idPlato,
        Plato = plato,
        Bebidas = _repoBebida.ListarTodos(),
        Guarniciones = _repoGuarnicion.ListarTodos(),
        Aderezos = _repoAderezo.ListarTodos(),
        SubTotal = plato.Costo
    };

    return View(vm);
}

[Authorize(Roles = "user, admin")]
[HttpPost]
public IActionResult GuardarDetalle(PedidoDetalleVM vm)
{

    var pedido = _repoPedido.ObtenerPorId(vm.IdPedido);
    if (pedido == null || pedido.Estado != "pendiente")
        return BadRequest("Pedido inválido");

  
    var plato = _repoPlato.ObtenerPorId(vm.IdPlato);
    if (plato == null)
        return BadRequest("Plato inexistente");

    int subtotal = plato.Costo;


    if (vm.IdBebida.HasValue)
    {
        var bebida = _repoBebida.ObtenerPorId(vm.IdBebida.Value);
        if (bebida != null)
        {
            subtotal += bebida.Costo;
        }
    }

    var detalle = new PedidoDetalle
    {
        IdPedido = vm.IdPedido,
        IdPlato = vm.IdPlato,
        IdBebida = vm.IdBebida,
        IdGuarnicion = vm.IdGuarnicion,
        IdAderezo = vm.IdAderezo,
        SubTotal = subtotal
    };

    _repoPedidoDetalle.Crear(detalle);

    _repoPedido.ActualizarMonto(vm.IdPedido, subtotal);

    return RedirectToAction("Index", "Cliente");
}

[Authorize(Roles = "user, admin")]
[HttpGet]
public IActionResult PedidosCliente()
{
    return View();
}

[Authorize(Roles = "user, admin")]
[HttpGet]
public IActionResult ListarPedidosCliente(
    int page = 1,
    DateTime? desde = null,
    DateTime? hasta = null
)
{
     int idUsuario = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
    int pageSize = 5;

    var total = _repoPedido.ContarPedidosCliente(idUsuario, desde, hasta);
    var pedidos = _repoPedido.ObtenerPedidosClientePaginadoMV(
        idUsuario, page, pageSize, desde, hasta
    );

    return Json(new
    {
        total,
        pedidos
    });
}

[Authorize(Roles = "resto, admin")]
[HttpGet]
public IActionResult PedidosCocinero()
{
    return View();
}

[Authorize(Roles = "resto, admin")]
[HttpGet]
public IActionResult ListarPedidosCocinero(
    int page = 1,
    DateTime? desde = null,
    DateTime? hasta = null
)
{
    int idUsuario = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

    var resto = _repoResto.BuscarPorUsuario(idUsuario);
    if (resto == null)
        return Unauthorized();

    int pageSize = 5;

    var total = _repoPedido.ContarPedidosRestaurante(resto.IdRes, desde, hasta);
    var pedidos = _repoPedido.ObtenerPedidosRestaurantePaginadoVM(
        resto.IdRes, page, pageSize, desde, hasta
    );

    return Json(new { total, pedidos });
}

[Authorize(Roles = "resto, admin")]
[HttpPost]
public IActionResult CambiarEstado(int idPedido)
{
    _repoPedido.CambiarEstado(idPedido, "completado");
    return Ok();
}

}