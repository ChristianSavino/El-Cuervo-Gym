using El_Cuervo_Gym_Web.Controllers.Models;
using El_Cuervo_Gym_Web.Core.Ingresos.Domain;
using El_Cuervo_Gym_Web.Core.Ingresos.Logic;
using El_Cuervo_Gym_Web.Core.Parametros.Logic;
using El_Cuervo_Gym_Web.Core.Socio.Logic;
using El_Cuervo_Gym_Web.Core.Utils;
using Microsoft.AspNetCore.Mvc;

namespace El_Cuervo_Gym_Web.Controllers
{
    [ApiController]
    [Route("api/socios")]
    public class SocioController : Controller
    {
        private readonly ISocioService _socioService;
        private readonly IIngresoService _ingresoService;
        private readonly IParametros _parametros;

        public SocioController(ISocioService socioService, IIngresoService ingresoService, IParametros parametros)
        {
            _socioService = socioService;
            _ingresoService = ingresoService;
            _parametros = parametros;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] SocioLogin login)
        {
            var socio = await _socioService.LogearSocio(login.Documento, login.NroSocio);

            return Ok(socio);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser([FromRoute] int id)
        {
            var socio = await _socioService.ObtenerSocioConPagosPorId(id);
            if (socio == null)
            {
                return NotFound();
            }
            return Ok(socio);
        }

        [HttpGet("ingresos/{id}")]
        public async Task<IActionResult> GetIngresosSocio([FromRoute] int id)
        {
            var ingresos = await _ingresoService.ObtenerIngresosEnElDiaSocio(DateTime.Now, id);
            return Ok(ingresos);
        }

        [HttpGet("parametro/diasCuotaMax")]
        public IActionResult GetDiasCuotaMax()
        {
            var parametro = int.Parse(_parametros.ObtenerTodosLosParametros().First(p => p.Clave == Helper.DiasMaxPermitidoParamName).Valor);
            return Ok(parametro);
        }

        [HttpPost("ingreso")]
        public async Task<IActionResult> InsertarIngreso([FromBody] Ingreso ingreso)
        {
            var resultado = await _ingresoService.InsertarIngreso(ingreso);
            return Ok(resultado);
        }
    }
}
