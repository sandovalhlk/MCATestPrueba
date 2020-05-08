using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository;
using DataLayerMCA;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace BusinessLogicMCA
{
    public class EmailBL
    {
        /// <summary>
        /// Verificar si existe una configuracion del Email o si tienes datos
        /// </summary>
        /// <returns></returns>
        public static bool EmailDatosConfigurado()
        {
            var emailData = new BaseRepository<AspNetEmails>().GetFromDatabaseWithQuery("select TOP 1 * from dbo.AspNetEmails").ToList<AspNetEmails>();
            if (emailData == null)
                return false;

            return (emailData.Count <= 0 ? false : true);
        }

        /// <summary>
        /// Enviar contraseña al correo electronico a usuario nuevo , texto de Bienvenida al sistema
        /// </summary>
        /// <param name="Subject">Asunto</param>
        /// <param name="NameWelcome">Nombre de usuario(nombres y apellidos) para bienvenida</param>
        /// <param name="ToEmail">A</param>
        /// <param name="userApp">Nombre del login o inicio de session</param>
        /// <param name="NewPassword">Contraseña generada</param>
        /// <param name="Body">Cuerpo personalizado del mensaje en HTML</param>
        /// <param name="CC">Enviar copaias de correo , exameple1@gmail.com,example2@yahoo.es</param>
        /// <param name="urlSitio">Url del Sistio web para acceder</param>
        /// <param name="urlChangePassword">Url para cambiar de contraseña</param>
        /// <returns></returns>
        public static bool SendEmailPasswordWelcome(string Subject, string NameWelcome, string ToEmail, string userApp, string NewPassword, string Body = "", string CC = "", string urlSitio = "", string urlChangePassword = "")
        {

            string body = "<h3>El siguiente correo fue enviado a: " + ToEmail + ".</h3><br />";
            body = body + "Bienvenid@ <b>" + NameWelcome + "</b><br />" + ".";
            body = body + "<p>Al iniciar sesión necesitará cambiar su contraseña. <br/><br />";
            body = body + "Su usuario es: <b>" + userApp + "</b><br />";
            body = body + "Contraseña generada: <b>" + NewPassword + "</b><br />";

            if (string.IsNullOrEmpty(urlSitio) == false)
                body = body + "Para acceder al sistema click en el siguiente enlace: <a href='" + urlSitio + "' target='_blank'>" + urlSitio + "</a><br />";

            if (string.IsNullOrEmpty(urlChangePassword) == false)
                body = body + "Para cambiar contrase&ntildea click en el siguiente enlace <a href='" + urlChangePassword + "' target='_blank'>" + urlChangePassword + "</a></p>";

            if (string.IsNullOrEmpty(Body) == false)
                body = Body;

            SendEmail(Subject: Subject, ToEmail: ToEmail, body: body, CC: CC);
            return true;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="Subject">Asunto</param>
        /// <param name="ToEmail">A Email</param>
        /// <param name="Body">Cuerpo personalizado del mensaje en HTML</param>
        /// <param name="CC">Enviar copaias de correo , exameple1@gmail.com,example2@yahoo.es</param>
        /// <param name="filePath">Ruta de archivo</param>
        /// <returns></returns>
        public static bool SendEmail(string Subject, string ToEmail, string body, string CC = "", string filePath = "")
        {
            AspNetEmails EmailDatos = EmailBL.GetEmailData();

            MailMessage email = new MailMessage();
            email.To.Add(new MailAddress(ToEmail));
            email.From = new MailAddress(EmailDatos.Email);

            if (!string.IsNullOrEmpty(CC))
            {
                var arrayCC = CC.Split(new char[] { ',' });
                foreach (var item in arrayCC)
                {
                    email.CC.Add(new MailAddress(item.Trim()));
                }
            }

            if (!string.IsNullOrEmpty(filePath))
            {
                email.Attachments.Add(new Attachment(filePath));
            }
            email.Subject = Subject;
            email.Body = body;
            email.IsBodyHtml = true;
            email.Priority = MailPriority.Normal;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = EmailDatos.Host;
            smtp.Port = EmailDatos.Port;
            smtp.EnableSsl = EmailDatos.EnableSsl;
            smtp.UseDefaultCredentials = EmailDatos.UseDefaultCredentials;

            if (EmailDatos.EnableSsl)
            {
                /* Para evitar el error, debes utilizar este método que permite  la validación personalizada por el cliente del certificado de servidor */
                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chai, SslPolicyErrors sslPolicyErrors) { return true; };
            }

            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Credentials = new NetworkCredential(EmailDatos.Email, EmailDatos.Password);
            smtp.Send(email);
            email.Dispose();
            return true;
        }



        /// <summary>
        /// Obtener los datos del email para envios de correo electronico
        /// </summary>
        /// <returns></returns>
        public static AspNetEmails GetEmailData()
        {
            var repo = new BaseRepository<AspNetEmails>();
            var datos = repo.GetFromDatabaseWithQuery("select TOP 1 * from dbo.AspNetEmails").FirstOrDefault<AspNetEmails>();
            return datos;
        }


        #region  Metodos 

        /// <summary>
        /// Obtener texto para HTML de envios de Correo 
        /// </summary>
        /// <param name="opcion">0>> Registro de Usuario, 1>> Solicitar una contraseña temporal </param>
        /// <param name="toUsuario">Objeto Usuario</param>
        /// <param name="contrasena">Contraseña</param>
        /// <returns></returns>
        public static string GetHtmlEmail(int opcion, HttpRequestBase httpContext, string FirstName, string LastName, string UserName, string contrasena, string linkLogin = "")
        {
            StringBuilder html = new StringBuilder();

            var url_server = httpContext.Url.GetLeftPart(UriPartial.Authority) +
                            (httpContext.ApplicationPath != "/" ? httpContext.ApplicationPath + "/"
                           : httpContext.ApplicationPath);
            string imgLogo = url_server + "imagenes/shared/logo.png";
            string linkLogin_ = (string.IsNullOrEmpty(linkLogin) ? url_server + "Account/Login" : linkLogin);

            switch (opcion)
            {
                case 0: /*** HTML de Registro de Usuario***/
                    html.Append("<div style='background-color:#fff;color:#333;font-size:16px;font-family:arial,verdana,sans-serif;max-width:500px;width:500px;min-width:500px;margin:auto;text-align:center'>");
                    html.Append("<table style='max-width:500px;width:500px;min-width:500px;border-collapse:collapse' cellspacing='0' cellpadding='0' border='0'><tbody><tr><td style='width:25%;background:#1a3867;line-height:5px'>&nbsp;</td><td style='width:25%;background:#1697d4;line-height:5px'>&nbsp;</td><td style='width:25%;background:#0e4290;line-height:5px'>&nbsp;</td><td style='width:25%;background:#38aef9;line-height:5px'>&nbsp;</td></tr></tbody></table>");
                    html.Append("<br>");
                    html.Append("<table cellspacing='0' cellpadding='0' border='0' style='max-width:500px;width:500px;min-width:500px'>");
                    html.Append("<tbody>");
                    html.Append("<tr><td style='text-align:center'><img src=" + imgLogo + " alt='MTI' class='CToWUd' style='border: 1px solid #1E1E1E;width:150px'></td></tr>");
                    html.Append("<tr><td><br></td></tr>");
                    html.Append("<tr><td style='text-align:center;font-size:20px'><b>Bienvenid@</b> " + FirstName + " " + LastName + "</td></tr>");
                    html.Append("<tr><td><br></td></tr>");
                    html.Append("<tr><td style='text-align:left;font-size:20px'><b>Usuario:</b> " + UserName + "</td></tr>");
                    html.Append("<tr><td><br></td></tr>");
                    html.Append("<tr><td style='text-align:left;font-size:20px'><b>Contraseña temporal:</b> " + contrasena.Trim() + "</td></tr>");
                    html.Append("<tr><td><br></td></tr>");
                    html.Append("<tr><td style='border-top:1px solid #d0d0d0;height:1px'></td></tr>");
                    html.Append("<tr><td><br></td></tr>");
                    html.Append("<tr><td style='text-align:center'>Por favor, confirme su registro.</td></tr>");
                    html.Append("<tr><td><br></td></tr>");
                    html.Append("<tr><td style='text-align:center'><small>Favor cambiar la contraseña temporal al iniciar sesión</small></td></tr>");
                    html.Append("<tr><td><br></td></tr>");
                    html.Append("<tr><td style='border-top:1px solid #d0d0d0;height:1px'></td></tr>");
                    html.Append("<tr><td><br></td></tr>");
                    html.Append("<tr>");
                    html.Append("<td style='font-size:11px;color:#1A1818;max-width:500px;width:500px;min-width:500px;text-align:center'>");
                    html.Append("<table cellspacing='0' cellpadding='0' border='0' id='m_818744359545078712footer-text' style='max-width:500px;width:500px;min-width:500px'>");
                    html.Append("<tbody>");
                    html.Append("<tr>");
                    html.Append("<td style='text-align:center;font-size:12px'>");
                    html.Append("<span>Has recibido este e-mail por que has sido registrado al Sistema Contable MCA. <b>Ministerio de Transporte e Infraestructura</b></span>");
                    html.Append("</td>");
                    html.Append("</tr>");
                    html.Append("</tbody>");
                    html.Append("</table>");
                    html.Append("</td>");
                    html.Append("</tr>");
                    html.Append("</tbody>");
                    html.Append("</table>");
                    html.Append("</div>");

                    break;

                case 1: /*** HTML de solicitud de contraseña temporal  ***/
                    html.Append("<div style='background-color:#fff;color:#333;font-size:16px;font-family:arial,verdana,sans-serif;max-width:500px;width:500px;min-width:500px;margin:auto;text-align:center'>");
                    html.Append("<table style='max-width:500px;width:500px;min-width:500px;border-collapse:collapse' cellspacing='0' cellpadding='0' border='0'><tbody><tr><td style='width:25%;background:#1a3867;line-height:5px'>&nbsp;</td><td style='width:25%;background:#1697d4;line-height:5px'>&nbsp;</td><td style='width:25%;background:#0e4290;line-height:5px'>&nbsp;</td><td style='width:25%;background:#38aef9;line-height:5px'>&nbsp;</td></tr></tbody></table>");
                    html.Append("<br>");
                    html.Append("<table cellspacing='0' cellpadding='0' border='0' style='max-width:500px;width:500px;min-width:500px'>");
                    html.Append("<tbody>");
                    html.Append("<tr><td style='text-align:center'><img src=" + imgLogo + " alt='MTI' class='CToWUd' style='border: 1px solid #1E1E1E;width:150px'></td></tr>");
                    html.Append("<tr><td><br></td></tr>");
                    html.Append("<tr><td style='text-align:center;font-size:23px'>Acabas de solicitar una generación de contraseña.</td></tr>");
                    html.Append("<tr><td><br></td></tr>");
                    html.Append("<tr><td style='text-align:center;font-size:20px'>" + FirstName + " " + LastName + "</td></tr>");
                    html.Append("<tr><td><br></td></tr>");
                    html.Append("<tr><td style='text-align:left;font-size:20px'><b>Su usuario:</b> " + UserName + "</td></tr>");
                    html.Append("<tr><td><br></td></tr>");
                    html.Append("<tr><td style='text-align:left;font-size:20px'><b>Contraseña temporal:</b> " + contrasena.Trim() + "</td></tr>");
                    html.Append("<tr><td><br></td></tr>");
                    html.Append("<tr><td style='border-top:1px solid #d0d0d0;height:1px'></td></tr>");
                    html.Append("<tr><td><br></td></tr>");
                    html.Append("<tr><td style='text-align:center'><small>Favor cambiar la contraseña temporal al iniciar sesión</small></td></tr>");
                    html.Append("<tr><td><br></td></tr>");
                    html.Append("<tr><td style='border-top:1px solid #d0d0d0;height:1px'></td></tr>");
                    html.Append("<tr><td><br></td></tr>");
                    html.Append("<tr>");
                    html.Append("<td style='font-size:11px;color:#1A1818;max-width:500px;width:500px;min-width:500px;text-align:center'>");
                    html.Append("<table cellspacing='0' cellpadding='0' border='0' id='m_818744359545078712footer-text' style='max-width:500px;width:500px;min-width:500px'>");
                    html.Append("<tbody>");
                    html.Append("<tr>");
                    html.Append("<td style='text-align:center;font-size:12px'>");
                    html.Append("<span><b>Ministerio de Transporte e Infraestructura</b></span>");
                    html.Append("</td>");
                    html.Append("</tr>");
                    html.Append("</tbody>");
                    html.Append("</table>");
                    html.Append("</td>");
                    html.Append("</tr>");
                    html.Append("</tbody>");
                    html.Append("</table>");
                    html.Append("</div>");

                    break;
            }
            return html.ToString();
        }




        #endregion


    }
}

