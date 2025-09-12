using Microsoft.AspNetCore.Mvc;
using CommerceBack.Domain;
using CommerceBack.Servicios;
namespace CommerceWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly InvoiceService _invoiceService;

        public InvoiceController()
        {
            _invoiceService = new InvoiceService();
        }

        // GET: api/Invoice
        [HttpGet]
        public IActionResult GetAll()
        {
            var invoices = _invoiceService.GetAll();
            return Ok(invoices); // 200 OK con la lista de facturas
        }

        // GET: api/Invoice/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var invoice = _invoiceService.GetInvoice(id);
            if (invoice == null)
                return NotFound($"No se encontró la factura con Id = {id}");
            return Ok(invoice);
        }

        // POST: api/Invoice
        [HttpPost]
        public IActionResult Post([FromBody] Invoice invoice)
        {
            if (invoice == null)
                return BadRequest("Se esperaba una factura válida");

            var result = _invoiceService.SaveInvoice(invoice);
            if (result)
                return Ok("Factura guardada correctamente");
            return StatusCode(500, "No se pudo guardar la factura");
        }

        // PUT: api/Invoice/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Invoice invoice)
        {
            if (invoice == null || invoice.Number != id)
                return BadRequest("Datos de factura inválidos");

            var existing = _invoiceService.GetInvoice(id);
            if (existing == null)
                return NotFound($"No existe la factura con Id = {id}");

            var result = _invoiceService.SaveInvoice(invoice);
            if (result)
                return Ok("Factura actualizada correctamente");
            return StatusCode(500, "No se pudo actualizar la factura");
        }

        // DELETE: api/Invoice/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _invoiceService.DeleteInvoice(id);
            if (result)
                return Ok("Factura eliminada correctamente");
            return NotFound($"No se encontró la factura con Id = {id}");
        }
    }
}
