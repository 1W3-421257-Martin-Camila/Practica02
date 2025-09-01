using System;
using System.Collections.Generic;
using Practica01.Domain;
using Practica01.Servicios;

class Program
{
    static void Main(string[] args)
    {
        // Instanciamos el Service
        InvoiceService oService = new InvoiceService();

        // Obtener todos los Serviceos - GetAll
        Console.WriteLine("Obtener todas las facturas - GetAll");

        // Llamamos al Service
        List<Invoice> lp = oService.GetAll();

        // Manejamos la respuesta
        if (lp.Count > 0)
        {
            foreach (Invoice p in lp)
                Console.WriteLine(p);
        }
        else
        {
            Console.WriteLine("No hay Facturas...");
        }

        // Obtener un Invoiceo por id - GetById
        Console.WriteLine("\nObtener una Factura por numero - GetById");

        // Llamamos al Service
        Invoice? invoiceFromDb = oService.GetInvoiceById(1);

        // Manejamos la respuesta
        if (invoiceFromDb != null)
        {
            Console.WriteLine(invoiceFromDb);
        }
        else
        {
            Console.WriteLine("No hay factura con ese id");
        }

        // Crear método de pago EXISTENTE en la BD
        PaymentMethod pm = new PaymentMethod() { Id = 1, Name = "Cash" };

        // Crear artículos
        Article a1 = new Article() { Name = "Laptop", UnitPrice = 1200.50m };
        Article a2 = new Article() { Name = "Mouse", UnitPrice = 25.00m };

        // Crear detalles
        InvoiceDetail d1 = new InvoiceDetail() { Article = a1, Quantity = 1 };
        InvoiceDetail d2 = new InvoiceDetail() { Article = a2, Quantity = 2 };

        // Crear factura
        Invoice newInvoice = new Invoice()
        {
            Date = DateTime.Now,
            Customer = "Camila",
            PaymentMethod = pm,
            Details = new List<InvoiceDetail>() { d1, d2 }
        };

        // Guardar factura
        InvoiceService service = new InvoiceService();
        bool result = service.SaveInvoice(newInvoice);

        if (result)
            Console.WriteLine("Factura guardada con éxito!");
        else
            Console.WriteLine("No se pudo guardar la factura.");

    }
}
