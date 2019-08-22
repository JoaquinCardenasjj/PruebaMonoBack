
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Gestion_Humana_.Utils
{
    public class GestorCorreo
    {
        private SmtpClient cliente;
        private static IConfiguration Configuration { get; set; }
        private MailMessage email;
        public GestorCorreo()
        {
            InicializaConfiguracion();
            var host = Configuration["Configuracion:host"];

            cliente = new SmtpClient(Configuration["Configuracion:host"], Int32.Parse(Configuration["Configuracion:port"]))
            {
                EnableSsl = Boolean.Parse(Configuration["Configuracion:enableSsl"]),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(Configuration["Configuracion:user"], Configuration["Configuracion:password"])
            };
        }
        private static void InicializaConfiguracion()
        {
            var builder = new ConfigurationBuilder()
                                    .SetBasePath(Directory.GetCurrentDirectory())
                                    .AddJsonFile("Utils/ConfigurationEmail.json");
            Configuration = builder.Build();
        }
        public void EnviarCorreo(string destinatario, string asunto, string mensaje, bool esHtlm = true)
        {
            email = new MailMessage(Configuration["Configuracion:user"], destinatario, asunto, mensaje);
            email.IsBodyHtml = esHtlm;
            cliente.Send(email);
        }
        public void EnviarCorreo(MailMessage message)
        {
            cliente.Send(message);
        }
        public async Task EnviarCorreoAsync(MailMessage message)
        {
            await cliente.SendMailAsync(message);
        }
    }
}
