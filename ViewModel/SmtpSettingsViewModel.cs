namespace Stock_CMS.ViewModel
{
    public class SmtpSettingsViewModel
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string SenderName { get; set; }
    }
}
