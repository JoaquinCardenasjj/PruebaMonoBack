using Gestion_Humana_.Utils;
using MongoDB.Driver;
using PruebaMongoJoaquinCardenas.Modelos;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaMongoJoaquinCardenas.Servicios
{
    public class FacturaService
    {
        private GestorCorreo correo = new GestorCorreo();
        private readonly IMongoCollection<Facturas> _Facturas;
        
        private ClienteService _Clientes;

        public FacturaService(IPruebaMonostoreDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _Facturas = database.GetCollection<Facturas>(settings.FacturasCollectionName);
            _Clientes = new ClienteService(settings);
        }

        public List<Facturas> Get() =>
            _Facturas.Find(Factura => true).ToList();

        public Facturas Get(string id) =>
            _Facturas.Find<Facturas>(Factura => Factura.Id == id).FirstOrDefault();

        public Facturas Create(Facturas Factura)
        {
            _Facturas.InsertOne(Factura);
            return Factura;
        }

        public void Update(string id, Facturas FacturaIn) =>
            _Facturas.ReplaceOne(Factura => Factura.Id == id, FacturaIn);

        public void Remove(Facturas FacturaIn) =>
            _Facturas.DeleteOne(Factura => Factura.Id == FacturaIn.Id);

        public void Remove(string id) =>
            _Facturas.DeleteOne(Factura => Factura.Id == id);
        public void RevisarFacturas()
        {
            var ListaClientes = _Clientes.Get();
            foreach (var item in ListaClientes)
            {
                var FacturasPrimerRecordatorio = _Facturas
                    .Find(Factura => true && Factura.Cliente == item.Documento && Factura.Estado == "primerrecordatorio" && Factura.Pagada == "false")
                    .ToList();
                var FacturasSegundoRecordatorio = _Facturas
                    .Find(Factura => true && Factura.Cliente == item.Documento && Factura.Estado == "segundorecordatorio" && Factura.Pagada == "false")
                    .ToList();

                foreach (var item2 in FacturasPrimerRecordatorio)
                {
                    item2.Estado = "segundorecordatorio";
                    Update(item2.Id, item2);
                }
                if (FacturasPrimerRecordatorio.Count > 0)
                {
                    string cuerpo = CuerpoCorreo(FacturasPrimerRecordatorio, item.Nombres + " " + item.Apellidos);
                    correo.EnviarCorreo(item.Correo, "Notificación facturas no canceladas", cuerpo);
                }
                foreach (var item3 in FacturasSegundoRecordatorio)
                {
                    item3.Estado = "desactivado";
                    Update(item3.Id, item3);
                }
                if (FacturasSegundoRecordatorio.Count > 0)
                {
                    string cuerpo = CuerpoCorreo(FacturasSegundoRecordatorio, item.Nombres + " " + item.Apellidos);
                    correo.EnviarCorreo(item.Correo, "Notificación facturas no canceladas", cuerpo);
                }
            }


        }

        public string CuerpoCorreo(List<Facturas> Lista, string nombresApellidos)
        {
            var estiloCelda = "font-family:Arial, sans-serif;font-size:14px;padding:10px 5px;border-style:solid;border-width:1px;overflow:hidden;word-break:normal;border-color:#ccc;color:#333;background-color:#fff;text-align:left;vertical-align:top";
            var estiloTitulo = "font-family:Arial, sans-serif;font-size:14px;font-weight:normal;padding:10px  5px;border-style:solid;border-width:1px;overflow:hidden;word-break:normal;border-color:#ccc;color:#333;background-color:#f0f0f0;text-align:left;vertical-align:top";
            var estiloTabla = "border-collapse:collapse;border-spacing:0;border-color:#ccc;table-layout: fixed; width: 900px";
            // Estilos tomados de:https://www.tablesgenerator.com/html_tables
            var cuerpoBase = new StringBuilder();
            cuerpoBase.AppendLine("<p>&nbsp;<span style='color: #000000;'>Cordial saludo señor(a) " + nombresApellidos + "</span></p>");
            cuerpoBase.AppendLine("<div> &nbsp;</div>");
            cuerpoBase.AppendLine("<div>  <span style='color: #000000;'> Se informa que se le ha realizado un cambio de estado a sus facturas.</span> </div>");
            cuerpoBase.AppendLine("<div> &nbsp;</div>");
            cuerpoBase.AppendLine("<div> <span style='color: #000000;'> Datos sobre la factura:</span></div>");
            cuerpoBase.AppendLine("<div>");
            cuerpoBase.AppendLine("<div>");

            cuerpoBase.AppendLine("<div> &nbsp;</div>");
            cuerpoBase.AppendLine("<table style='" + estiloTabla + "' class='tg'><colgroup><col style='width: 200px'><col style='width: 200px'><col style='width: 200px'><col style='width: 300px'></colgroup>");
            cuerpoBase.AppendLine("<tr>");
            cuerpoBase.AppendLine("<th style='" + estiloTitulo + "'>Codigo factura</th>");
            cuerpoBase.AppendLine("<th style='" + estiloTitulo + "'>Cliente</th>");
       
            cuerpoBase.AppendLine("<th style='" + estiloTitulo + "'>Sub Total</th>");
            cuerpoBase.AppendLine("<th style='" + estiloTitulo + "'>Total</th>");
            //cuerpoBase.AppendLine("<th style='" + estiloTitulo + "'>&nbsp;</th>");
            cuerpoBase.AppendLine("</tr>");
            // FACTURAS
            cuerpoBase.AppendLine("*FACTURAS*");
            cuerpoBase.AppendLine("</table>");
            cuerpoBase.AppendLine("<div>&nbsp;</div>");
            cuerpoBase.AppendLine("<div> <span style='color: #000000;'>Cordialmente, </span></div>");
            cuerpoBase.AppendLine("<div>&nbsp;</div>");
            cuerpoBase.AppendLine("<div> <span style='color: #000000;'> <strong>Sistema del sistema</strong></span></div>");
            cuerpoBase.AppendLine("</div>");
            cuerpoBase.AppendLine("<div>&nbsp;</div>");
            cuerpoBase.AppendLine("</div>");

            var facturas = new StringBuilder();

            foreach (var item in Lista)
            {
                facturas = facturas.AppendLine("<tr>");
                facturas = facturas.AppendLine("<td style='" + estiloCelda + "'>" + item.CodigoFactura + "</td>");
                facturas = facturas.AppendLine("<td style='" + estiloCelda + "'>" + item.Cliente + "</td>");
           
                facturas = facturas.AppendLine("<td style='" + estiloCelda + "'>" + item.SubTotal + "</td>");
                facturas = facturas.AppendLine("<td style='" + estiloCelda + "'>" + item.TotalFactura + "</td>");
                facturas = facturas.AppendLine("</tr>");
            }
            cuerpoBase.Replace("*FACTURAS*", facturas.ToString());
            return cuerpoBase.ToString();

        }
    }
}
