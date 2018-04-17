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

    public static void generateMail() {
      SmtpServer.UseDefaultCredentials = false;
      SmtpServer.Credentials = new NetworkCredential("politieke.barometer.mail@gmail.com", "Arnoisdebesteteamleader");
      SmtpServer.Port = 465;
      SmtpServer.EnableSsl = true;
      SmtpServer.Host = "smtp.gmail.com";
      isGenerated = true;
    }

    public static void sendMail(string to, string subject, string content) {
      if (!Mail.isGenerated) {
        generateMail();
      }
      Email = new MailMessage();
      Email.From = new MailAddress("politieke.barometer.mail@gmail.com");
      Email.To.Add(to);
      Email.Subject = subject;
      Email.IsBodyHtml = true;
      Email.Body = content;
      SmtpServer.Send(Email);
    }
  }
}
