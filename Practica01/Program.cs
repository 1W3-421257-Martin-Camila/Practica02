// Instanciamos el Service
using Practica01.Domain;
using Practica01.Servicios;

InvoiceService oService = new InvoiceService();

// Obtener todos los Serviceos - GetAll
Console.WriteLine("Obtener todas las facturas - GetAll");

// Llamamos al Service
List<Invoice> lp = oService.GetAll();

// Manejamos la respuesta
if (lp.Count > 0)
    foreach (Invoice p in lp)
        Console.WriteLine(p);
else
    Console.WriteLine("No hay Facturas...");

// Obtener un Invoiceo por id - GetById
Console.WriteLine("\nObtener una Factura por numero - GetById");

// Llamamos al Service
Invoice? invoice = oService.GetInvoiceById(1);

// Manejamos la respuesta
if (invoice != null)
{
    Console.WriteLine(invoice);
}
else
{
    Console.WriteLine("No hay factura con ese id");
}



