using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;

namespace BL {
  public class Mail {
    static SmtpClient SmtpServer;
    static MailMessage Email;
    public static Boolean isGenerated = false;

    public static void GenerateMail() {
            SmtpServer = new SmtpClient
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("politieke.barometer.mail@gmail.com", "Arnoisdebesteteamleader"),
                Port = 587,
                EnableSsl = true,
                Host = "smtp.gmail.com"
            };
            isGenerated = true;
    }

    public static void SendMail(string to, string subject, string content) {
      if (!Mail.isGenerated) {
        GenerateMail();
      }
            Email = new MailMessage
            {
                From = new MailAddress("politieke.barometer.mail@gmail.com")
            };
            Email.To.Add(to);
      Email.Subject = subject;
      Email.IsBodyHtml = true;
      Email.Body = content;
      SmtpServer.Send(Email);
    }
  }
}
