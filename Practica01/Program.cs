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
Invoice? Invoiceo5 = oService.GetInvoiceById(5);

// Manejamos la respuesta
if (Invoiceo5 != null)
{
    Console.WriteLine(Invoiceo5);
}
else
{
    Console.WriteLine("No hay factura con ese id");
}


