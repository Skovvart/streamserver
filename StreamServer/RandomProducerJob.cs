using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StreamServer
{
    public class RandomProducerJob : BackgroundService
    {
        private readonly ILogger<RandomProducerJob> _logger;
        private readonly Random _random;
        private readonly IRandomService _randomService;

        public RandomProducerJob(ILogger<RandomProducerJob> logger, IRandomService randomService) {
            _logger = logger;
            _random = new Random();
            _randomService = randomService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            while (!stoppingToken.IsCancellationRequested) {
                _logger.LogInformation("Time to produce some randoms");
                _randomService.CurrentRandomInt = _random.Next();
                await Task.Delay(TimeSpan.FromSeconds(3));
            }
        }
    }
}