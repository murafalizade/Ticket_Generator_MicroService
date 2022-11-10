using System.Net;
using System.Net.Mail;

namespace QRTicketGenerator.API.Services
{
    public class MailDataervice
    {
        public static void SendEmail(string mailAddress, string fullName)
        {
            MailMessage message = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            message.From = new MailAddress("muradexample7@gmail.com");
            message.To.Add(new MailAddress(mailAddress));
            message.Subject = "Information";
            message.Body = $"Thank {fullName} for participate our forum.";
            message.Attachments.Add(new Attachment("resultqr.pdf"));
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
