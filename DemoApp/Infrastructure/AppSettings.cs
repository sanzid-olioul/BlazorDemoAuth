namespace DemoApp.Infrastructure
{
    public static class AppSettings
    {
        public static Settings Settings { get; set; } = new();
    }
    public class Settings
    {
        public string AdminEmail { get; set; }
        public string AdminPassword { get; set; }
        public string Key {  get; set; }
        public string Iv { get; set; }
    }
}