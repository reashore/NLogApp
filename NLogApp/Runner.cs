using Microsoft.Extensions.Logging;

namespace NLogApp
{
    public class Runner
    {
        private readonly ILogger<Runner> _logger;

        public Runner(ILogger<Runner> logger)
        {
            _logger = logger;
        }

        public void DoAction(string name)
        {
            const string message = "Doing hard work! {Action}";
#pragma warning disable CA1303 // Do not pass literals as localized parameters
            _logger.LogDebug(20, message, name);
#pragma warning restore CA1303 // Do not pass literals as localized parameters
        }
    }
}
