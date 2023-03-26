using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class AppSettings
    {
        public string EncryptionKey { get; set; } = string.Empty;
        public string Salt { get; set; } = string.Empty;
        public string EmailServiceHostAddress { get; set; } = string.Empty;
        public string EmailServicePort { get; set; } = string.Empty;
        public string EmailSericeSender { get; set; } = string.Empty;

    }
}
