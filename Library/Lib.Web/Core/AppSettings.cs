namespace Lib.Web.Core
{
    public class AppSettings
    {
        public string ConnectionString { get; set; }
        public string AppSecreateKey { get; set; }
        public string MainSiteURL { get; set; }
        public string AdminSiteURL { get; set; }
        public string AdminEmail { get; set; }
        public string[] AllowOriginsUrls { get; set; }
        public bool RequestLog { get; set; }
    }
}
