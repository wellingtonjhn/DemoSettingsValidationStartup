using System.ComponentModel.DataAnnotations;

namespace DemoSettingsValidationStartup.Settings
{
    public class MySettings : ConfigurationSettings
    {
        [Required]
        public string ApplicationName { get; set; }

        public string Author { get; set; }
    }
}