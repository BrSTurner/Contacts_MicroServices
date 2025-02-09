namespace FIAP.DatabaseManagement.WS.Contacts.Workers
{
    public class PersistanceWorker : BackgroundService
    {
        private readonly ILogger<PersistanceWorker> _logger;

        public PersistanceWorker(
            ILogger<PersistanceWorker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _logger.LogInformation("Persistance Worker - ON");
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong persisting Contact");
            }
            finally
            {
                _logger.LogInformation("Persistance Worker - OFF");
            }
        }
    }
}
