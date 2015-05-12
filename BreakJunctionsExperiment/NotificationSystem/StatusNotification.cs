using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using System.Windows;

namespace BreakJunctions.NotificationSystem
{
    public class StatusNotification
    {
        private static StatusNotification _Instance;
        public static StatusNotification Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new StatusNotification();

                return _Instance;
            }
        }
        private StatusNotification() { }

        private static Registry_NotificationSystem NotificationSystem = BreakJunctionsRegistry.Instance.Reg_NotificationSystem;

        public void SendStatusE_Mail(string _Subject, string _Body)
        {
            if (NotificationSystem.User_E_Mails.Count > 0)
            {
                try
                {
                    var mail = new MailMessage();
                    var smtpServer = new SmtpClient("smtp.live.com");

                    mail.From = new MailAddress(NotificationSystem.E_Mail_Address);
                    foreach (var MailInfo in NotificationSystem.User_E_Mails)
                        mail.To.Add(MailInfo.E_Mail_Address);


                    mail.Subject = _Subject;
                    mail.Body = _Body;

                    smtpServer.Port = 587;
                    smtpServer.Credentials = new NetworkCredential(NotificationSystem.E_Mail_Address, NotificationSystem.E_Mail_Password);
                    smtpServer.EnableSsl = true;
                    smtpServer.SendAsync(mail, this);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error sending message!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
