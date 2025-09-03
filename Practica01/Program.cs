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
        invoice.PaymentMethod = new PaymentMethod();
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

        Console.WriteLine("--------- TODOS LOS ARTÍCULOS: ---------");

        List<Article> listArt = aService.GetArticles();
        if (listArt.Count > 0)
        {
            foreach (Article a in listArt)
            {
                Console.WriteLine(a);
            }
        }
        else
        {
            Console.WriteLine("!!!!! NO HAY ARTÍCULOS ");
        }

        Console.WriteLine("--------- OBTENER UN ARTÍCULO POR SU ID --------- ");

        Article? article2 = aService.GetArticle(2);
        if (article2 != null)
        {
            Console.WriteLine(article2);
        }
        else
        {
            Console.WriteLine("!!!!! NO HAY UN ARTÍCULO CON NÚMERO 2 ");
        }

        Console.WriteLine("--------- ELIMINAR UN ARTÍCULO POR SU NUMERO ---------");

        bool resultDeleteArt = aService.DeleteArticle(3);
        if (resultDeleteArt)
        {
            Console.WriteLine("Se eliminó el artículo N°3 con éxito ");
        }
        else
        {
            Console.WriteLine("!!!!! NO SE PUDO ELIMINAR EL ARTÍCULO N°3");
        }

        Console.WriteLine("--------- CREAR UN ARTÍCULO ---------");

        Article article = new Article();
        article.Name = "Test";
        article.UnitPrice = 2500;

        bool resultCreateArt = aService.SaveArticle(article);

        if (resultCreateArt)
        {
            Console.WriteLine("Se creó el artículo con éxito");
        }
        else
        {
            Console.WriteLine("!!!!! NO SE PUDO CREAR EL ARTÍCULO ");
        }

        Console.WriteLine("--------- MODIFICAR UN ARTÍCULO ---------");

        article.Id = 5;
        article.Name = "Microfibra rosa";
        article.UnitPrice = 1000;

        bool resultUpdateArt = aService.SaveArticle(article);

        if (resultUpdateArt)
        {
            Console.WriteLine("Se modificó el artículo con éxito");
        }
        else
        {
            Console.WriteLine("!!!!! NO SE PUDO MODIFICAR EL ARTÍCULO ");
        }
    }
}

