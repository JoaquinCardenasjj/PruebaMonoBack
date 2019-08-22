using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PruebaMongoJoaquinCardenas.Modelos;
using PruebaMongoJoaquinCardenas.Servicios;

namespace PruebaMongoJoaquinCardenas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacturasController : ControllerBase
    {
        private readonly FacturaService _FacturaService;

        public FacturasController(FacturaService FacturaService)
        {
            _FacturaService = FacturaService;
        }

        [HttpGet]
        public ActionResult<List<Facturas>> Get() =>
            _FacturaService.Get();

        [HttpGet("{id:length(24)}", Name = "GetFactura")]
        public ActionResult<Facturas> Get(string id)
        {
            var Factura = _FacturaService.Get(id);

            if (Factura == null)
            {
                return NotFound();
            }

            return Factura;
        }
        [Route("revisar")]
        [HttpPost]
        public ActionResult<Facturas> Revisar()
        {
            _FacturaService.RevisarFacturas();
            return null;
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Facturas FacturaIn)
        {
            var Factura = _FacturaService.Get(id);

            if (Factura == null)
            {
                return NotFound();
            }

            _FacturaService.Update(id, FacturaIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var Factura = _FacturaService.Get(id);

            if (Factura == null)
            {
                return NotFound();
            }

            _FacturaService.Remove(Factura.Id);

            return NoContent();
        }
    }
}