using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;

namespace LMS.Utility
{
    public class SendEmail
    {
        public static bool EmailSend(string SenderEmail, string Subject, string Message, string AttchServerPath, bool IsBodyHtml = true)
        {
            //Logger logger = LogManager.GetLogger("databaseLogger");
            bool status = false;
            try
            {
                string HostAddress = "gravity.herosite.pro";
                string FormEmailId = "auto-mail@gyanshaktitech.com";
                string Port = "587";

                SmtpClient smtpClient = new SmtpClient();
                System.Net.NetworkCredential network = new System.Net.NetworkCredential();
                MailMessage msg = new MailMessage();
                msg.From = new MailAddress(FormEmailId);
                msg.To.Add(SenderEmail);
                msg.Body = Message;
                msg.Subject = Subject;
                msg.IsBodyHtml = IsBodyHtml;

                if (!string.IsNullOrEmpty(AttchServerPath))
                {
                    if (File.Exists(AttchServerPath))
                    {
                        System.Net.Mail.Attachment attachment;
                        attachment = new System.Net.Mail.Attachment(AttchServerPath);
                        msg.Attachments.Add(attachment);
                    }
                }
                smtpClient.Host = HostAddress;
                smtpClient.Port = Convert.ToInt32(Port);
                smtpClient.Credentials = new NetworkCredential("auto-mail@gyanshaktitech.com", "d6v1gD_55");
                smtpClient.EnableSsl = true;
                smtpClient.Send(msg);
                return true;

            }
            catch (Exception ex)
            {
                //logger.Info("Error Email Send on :-" + SenderEmail);
                //logger.Error(ex, ex.Message);
                return status;
            }
        }
    }
}
