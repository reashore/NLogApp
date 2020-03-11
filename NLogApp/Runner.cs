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
            _logger.LogDebug(20, message, name);
        }
    }
}
