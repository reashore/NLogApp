using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using static System.Console;

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
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                   .Build();

                IServiceProvider servicesProvider = BuildDi(configurationBuilder);

                Runner runner = servicesProvider.GetRequiredService<Runner>();
                runner.DoAction("Action1");
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Program exception");
                throw;
            }
            finally
            {
                LogManager.Shutdown();
            }

            WriteLine("Done");
            ReadKey();
        }

        private static IServiceProvider BuildDi(IConfiguration configuration)
        {
            return new ServiceCollection()
               .AddTransient<Runner>()
               .AddLogging(loggingBuilder =>
               {
                   loggingBuilder.ClearProviders();
                   loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                   loggingBuilder.AddNLog(configuration);
               })
               .BuildServiceProvider();
        }
    }
}
