using QRTicketGenerator.API.Dtos;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace QRTicketGenerator.API.Services
{
    public class MailService
    {
        public static void SendEmail(MailSenderDto obj, byte[] file)
        {
            MailMessage message = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            message.From = new MailAddress("muradexample7@gmail.com");
            message.To.Add(new MailAddress(obj.To));
            message.Subject = obj.Subject;
            message.Body = obj.Body;
            if (obj.ToCC != null && obj.ToCC != "")
            {
                message.CC.Add(new MailAddress(obj.ToCC));
            }
            if (obj.ToBCC != null && obj.ToBCC != "")
            {
                message.Bcc.Add(new MailAddress(obj.ToBCC));
            }
            message.Attachments.Add(new Attachment(new MemoryStream(file), "Ticket.pdf"));
            smtp.Port = 587;
            smtp.Host = "smtp.gmail.com"; //for gmail host  
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("muradexample7@gmail.com", "qmawmufhrffueuhs");
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Send(message);
        }
    }
}
