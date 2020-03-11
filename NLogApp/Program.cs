using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;

namespace NLogApp
{
    public static class Program
    {
        public static void Main()
        {
            Logger logger = LogManager.GetCurrentClassLogger();

            try
            {
                IConfigurationRoot configurationBuilder = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory()) //From NuGet Package Microsoft.Extensions.Configuration.Json
                   .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                   .Build();

                IServiceProvider servicesProvider = BuildDi(configurationBuilder);

                using (servicesProvider as IDisposable)
                {
                    Runner runner = servicesProvider.GetRequiredService<Runner>();
                    runner.DoAction("Action1");

                    string message = "Press any key to exit";
#pragma warning disable CA1303 // Do not pass literals as localized parameters
                    Console.WriteLine(message);
#pragma warning restore CA1303 // Do not pass literals as localized parameters
                    Console.ReadKey();
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Stopped program because of exception");
                throw;
            }
            finally
            {
                LogManager.Shutdown();
            }
        }

        private static IServiceProvider BuildDi(IConfiguration configuration)
        {
            return new ServiceCollection()
               .AddTransient<Runner>() // Runner is the custom class
               .AddLogging(loggingBuilder =>
               {
                   // configure Logging with NLog
                   loggingBuilder.ClearProviders();
                   loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                   // obsolete
                   loggingBuilder.AddNLog(configuration);
               })
               .BuildServiceProvider();
        }
    }
}
