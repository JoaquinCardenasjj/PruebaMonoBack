using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PruebaMongoJoaquinCardenas.Modelos;
using PruebaMongoJoaquinCardenas.Servicios;

namespace PruebaMongoJoaquinCardenas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowSpecificOrigin")]
    public class ClientesController : ControllerBase
    {
        private readonly ClienteService _ClienteService;

        public ClientesController(ClienteService ClienteService)
        {
            _ClienteService = ClienteService;
        }

        [HttpGet]
        public ActionResult<List<Clientes>> Get() =>
            _ClienteService.Get();

        [HttpGet("{id:length(24)}", Name = "GetCliente")]
        public ActionResult<Clientes> Get(string id)
        {
            var Cliente = _ClienteService.Get(id);

            if (Cliente == null)
            {
                return NotFound();
            }

            return Cliente;
        }

        [HttpPost]
        public ActionResult<Clientes> Create(Clientes Cliente)
        {
            _ClienteService.Create(Cliente);

            return CreatedAtRoute("GetCliente", new { id = Cliente.Id.ToString() }, Cliente);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Clientes ClienteIn)
        {
            var Cliente = _ClienteService.Get(id);

            if (Cliente == null)
            {
                return NotFound();
            }

            _ClienteService.Update(id, ClienteIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var Cliente = _ClienteService.Get(id);

            if (Cliente == null)
            {
                return NotFound();
            }

            _ClienteService.Remove(Cliente.Id);

            return NoContent();
        }
    }
}