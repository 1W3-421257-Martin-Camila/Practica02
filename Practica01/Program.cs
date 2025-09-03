using System;
using System.Collections.Generic;
using Practica01.Domain;
using Practica01.Services;
using Practica01.Servicios;

class Program
{
    static void Main(string[] args)
    {
        // TEST DE FACTURAS
        InvoiceService iService = new InvoiceService();

        Console.WriteLine("--------- TODOS LAS FACTURAS: ---------");

        List<Invoice> list = iService.GetAll();
        if (list.Count > 0) 
        {
            foreach (Invoice i in list) 
            {
                Console.WriteLine(i);
            }
        }
        else
        {
            Console.WriteLine("!!!!! NO HAY FACTURAS ");
        }

        Console.WriteLine("--------- OBTENER UNA FACTURA POR SU NÚMERO --------- ");

        Invoice? invoice4 = iService.GetInvoice(4);
        if (invoice4 != null)
        {
            Console.WriteLine(invoice4);
        }
        else
        {
            Console.WriteLine("!!!!! NO HAY UNA FACTURA CON NÚMERO 4 ");
        }

        Console.WriteLine("--------- ELIMINAR UNA FACTURA POR SU NUMERO ---------");

        bool resultDelete = iService.DeleteInvoice(3);
        if (resultDelete)
        {
            Console.WriteLine("Se eliminó la factura N°3 con éxito ");
        }
        else
        {
            Console.WriteLine("!!!!! NO SE PUDO ELIMINAR LA FACTURA N°3");
        }

        Console.WriteLine("--------- CREAR UNA FACTURA ---------");

        Invoice invoice = new Invoice();
        invoice.Customer = "Camila Martín";
        invoice.PaymentMethod.Id = 1;
        invoice.Date = DateTime.Now;

        bool resultCreate = iService.SaveInvoice(invoice);

        if (resultCreate)
        {
            Console.WriteLine("Se creó la factura con éxito");
        }
        else
        {
            Console.WriteLine("!!!!! NO SE PUDO CREAR LA FACTURA ");
        }

        Console.WriteLine("--------- MODIFICAR UNA FACTURA ---------");

        invoice.Number = 1;
        invoice.Customer = "Juan Pérez";
        invoice.PaymentMethod.Id = 2;
        invoice.Date = DateTime.Now;

        bool resultUpdate = iService.SaveInvoice(invoice);

        if (resultUpdate)
        {
            Console.WriteLine("Se modificó la factura con éxito");
        }
        else
        {
            Console.WriteLine("!!!!! NO SE PUDO MODIFICAR LA FACTURA ");
        }


        //TEST DE ARTÍCULOS
        ArticleService aService = new ArticleService();
    }
}

