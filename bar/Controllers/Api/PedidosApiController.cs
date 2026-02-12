using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/pedidos")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

public class PedidosApiController : ControllerBase
{
    [Authorize]
    [HttpGet]
    public IActionResult GetPedidos()
    {
        return Ok("Accediste correctamente usando JWT 🎉");
    }
}
