namespace BoschZip
{
    public class FileShareSettings : ISenderSettings
    {
        public string Domain { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool UseCredentials { get; set; } = false;

    }
}
