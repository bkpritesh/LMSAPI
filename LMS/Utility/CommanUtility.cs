using Microsoft.Extensions.Options;
using Model;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace LMS.Utility
{
    public class CommanUtility : ICommanUtility
    {
        private readonly AppSettings _appSettings;

        public CommanUtility(IOptions<AppSettings> options)
        {
            _appSettings = options.Value;
        }
        public string DecryptPassword(string password)
        {
            byte[] cipherBytes = Convert.FromBase64String(password);
            using (Aes encryptor = Aes.Create())
            {
                byte[] saltArray = Encoding.ASCII.GetBytes(_appSettings.Salt);
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(_appSettings.EncryptionKey, saltArray, 10000, HashAlgorithmName.SHA256);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    password = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return password;
        }

        public string EncryptPassword(string password)
        {
            try
            {
                byte[] clearBytes = Encoding.Unicode.GetBytes(password);
                using (Aes encryptor = Aes.Create())
                {
                    byte[] saltArray = Encoding.ASCII.GetBytes(_appSettings.Salt);
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(_appSettings.EncryptionKey, saltArray, 10000, HashAlgorithmName.SHA256);
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(clearBytes, 0, clearBytes.Length);
                            cs.Close();
                        }
                        password = Convert.ToBase64String(ms.ToArray());
                    }
                }
                return password;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool EmailSend(string SenderEmail, string Subject, string Message, string AttchServerPath, bool IsBodyHtml = true)
        {
            //Logger logger = LogManager.GetLogger("databaseLogger");
            bool status = false;
            try
            {
                string HostAddress = _appSettings.EmailServiceHostAddress;
                string FormEmailId = _appSettings.EmailSericeSender;
                string Port = _appSettings.EmailServicePort;

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
