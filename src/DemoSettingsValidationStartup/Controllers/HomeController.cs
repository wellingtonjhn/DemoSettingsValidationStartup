using DemoSettingsValidationStartup.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DemoSettingsValidationStartup.Controllers
{
    [Route("api/[controller]")]
    public class HomeController : Controller
    {
        private readonly IOptions<MySettings> _settings;

        public HomeController(IOptions<MySettings> settings)
        {
            _settings = settings;
        }

        [HttpGet]
        public string Get()
        {
            return _settings.Value.ApplicationName;
        }
    }
}