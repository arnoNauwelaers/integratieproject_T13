using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;

namespace BL
{
  public class EmailSender
  {
    private static EmailSender sender;
    private string email = "politiekebarometert13@gmail.com";
    private string password = "barometert13";
    private string server = "smtp.gmail.com";

    private EmailSender()
    {
    }

    public static EmailSender Instance
    {
      get
      {
        if (sender == null)
        {
          sender = new EmailSender();
        }
        return sender;
      }
    }

    
    public void SendEmail(string name, string login, string pwd)
    {
      // Creating the email
      MailMessage msg = new MailMessage();

      // Set the sender
      msg.From = new MailAddress("politiekebarometert13@gmail.com", "t13");

      // Set Retriever
      msg.To.Add(new MailAddress(login, name));

      // Set subject
      msg.Subject = "Registratie Barometer Platform";

      // set message
      msg.Body = String.Format("Hallo {0},\n\n" +
                            "Je bent nu geresistreerd bij ons platform.\n" +
                            "Je kan nu inloggen met deze gegevens:\n" +
                             "Email/gebruikersnaam:  {1}\n" +
                             "wachtwoord: {2}\n\n" +
                             "Je kan inloggen met deze link: http://localhost:44330/account/login \n\n" +
                             "Beleefde groetjes,\n" +
                             "t13 admin team",
                              name, login, pwd);

      // Set priority
      msg.Priority = MailPriority.High;

      // Connect to server
      SmtpClient sender = new SmtpClient(server, 25);
      sender.EnableSsl = true;
      sender.Credentials = new NetworkCredential(email, password);

      // Send email
      // krijg nog verbindingsfout oid, voorlopig in commentaar
      //sender.Send(msg);
    }
  }
}
