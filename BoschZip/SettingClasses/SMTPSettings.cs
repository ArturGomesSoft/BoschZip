using System.Net.Mail;

namespace BoschZip
{
    public class SMTPSettings : ISenderSettings
    {
        private string senderName = string.Empty;
        public string SenderName
        {
            get
            {
                if (string.IsNullOrEmpty(senderName))
                {
                    senderName = SenderAddress;
                }
                return senderName;
            }
            set
            {
                senderName = value;
            }
        }
        public string SenderAddress { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string SmtpHost { get; set; } = string.Empty;
        public int SmtpPort { get; set; } = 0;
        public SmtpDeliveryMethod SmtpDeliverymethod { get; set; } = SmtpDeliveryMethod.Network;
        public bool SmtpUserDefaultCredentials { get; set; } = false;
        public bool SmtpEnableSsl { get; set; } = true;

    }
}
