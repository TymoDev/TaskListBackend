using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastracture.EmailLogic
{
    public class EmailOptions
    {
        public string SmtpServer { get; set; } = string.Empty;
        public int Port { get; set; }
        public bool UseSsl { get; set; }
        public string FromEmail { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

}
