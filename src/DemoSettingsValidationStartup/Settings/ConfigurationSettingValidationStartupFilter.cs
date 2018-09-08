using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace DemoSettingsValidationStartup.Settings
{
    public class ConfigurationSettingValidationStartupFilter : IStartupFilter
    {
        private readonly IEnumerable<ConfigurationSettings> _configurationSettings;
        private readonly ILogger<ConfigurationSettingValidationStartupFilter> _logger;

        public ConfigurationSettingValidationStartupFilter(
            IEnumerable<ConfigurationSettings> configurationSettings,
            ILogger<ConfigurationSettingValidationStartupFilter> logger)
        {
            _configurationSettings = configurationSettings;
            _logger = logger;
        }

        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            foreach (var settings in _configurationSettings)
            {
                if (settings.Validate())
                {
                    continue;
                }

                var settingsName = settings.GetType().Name;
                var message = $"Invalid {settingsName} configuration. {string.Join(",", settings.ValidationResult)}";
                var exception = new InvalidOperationException(message);

                _logger.LogError(exception, "Invalid configuration. The application initialisation is aborted.");

                throw exception;
            }

            return next;
        }
    }

    public static class ConfigureOptionsExtensions
    {
        public static IServiceCollection RegisterSettings<TOptions>(this IServiceCollection services, IConfiguration configuration)
            where TOptions : class, new()
        {
            var name = typeof(TOptions).Name;

            services.Configure<TOptions>(configuration.GetSection(name));
            services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<TOptions>>().Value as ConfigurationSettings);

            return services;
        }
    }
}