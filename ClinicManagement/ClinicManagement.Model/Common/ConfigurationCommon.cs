using Microsoft.Extensions.Configuration;

namespace ClinicManagement.Model.Common
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
    public class ConfigurationCommon
    {
        private static ConfigurationCommon? _instance;
        private static IConfiguration? _config;

        private ConfigurationCommon()
        {
            _config = new ConfigurationBuilder().AddJsonFile($"appsettings.json", false, true).Build();
        }

        public static ConfigurationCommon Instance => _instance ??= new ConfigurationCommon();

        public string GetConfig(string key) => _config?[key] ?? string.Empty;

        public string GetConfig(string section, string key) => _config?[$"{section}:{key}"] ?? string.Empty;

        public string GetConfig(string section, string subSection, string key) => _config?[$"{section}:{subSection}:{key}"] ?? string.Empty;

        public T GetConfig<T>(string key) => (T)Convert.ChangeType(_config?[key] ?? string.Empty, typeof(T));

        public T GetConfig<T>(string section, string key) => (T)Convert.ChangeType(_config?[$"{section}:{key}"] ?? string.Empty, typeof(T));

        public T GetConfig<T>(string section, string subSection, string key) => (T)Convert.ChangeType(_config?[$"{section}:{subSection}:{key}"] ?? string.Empty, typeof(T));

#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8603 // Possible null reference return.
        public T GetSection<T>(string section) => _config.GetSection(section).Get<T>();
#pragma warning restore CS8603 // Possible null reference return.
#pragma warning restore CS8602 // Dereference of a possibly null reference.

#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8603 // Possible null reference return.
        public T GetSection<T>(string section, string subSection) => _config.GetSection(section).GetSection(subSection).Get<T>();
#pragma warning restore CS8603 // Possible null reference return.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
    }
}
