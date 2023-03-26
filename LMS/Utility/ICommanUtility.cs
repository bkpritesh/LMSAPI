namespace LMS.Utility
{
    public interface ICommanUtility
    {
        public string EncryptPassword(string password);
        public string DecryptPassword(string password);
        public bool EmailSend(string SenderEmail, string Subject, string Message, string AttchServerPath, bool IsBodyHtml = true);
    }
}
