using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace StreamServer
{
    public class StreamService : Streamer.StreamerBase
    {
        private readonly ILogger<StreamService> _logger;
        private readonly IRandomService _randomService;

        public StreamService(ILogger<StreamService> logger, IRandomService randomService) {
            _logger = logger;
            _randomService = randomService;
        }

        public override async Task GetReplyStream(StreamContextRequest request, IServerStreamWriter<StreamReply> responseStream, ServerCallContext context) {
            _logger.LogInformation("Stream request from {Name}", request.Name);
            while (!context.CancellationToken.IsCancellationRequested) {
                _logger.LogInformation("Sending stream update to {Name}", request.Name);
                await responseStream.WriteAsync(GetCurrentStreamReply(request));
                await Task.Delay(TimeSpan.FromSeconds(0.5));
            }
            _logger.LogInformation("Stream disconnected by {Name}", request.Name);
        }

        public override Task<StreamReply> GetReply(StreamContextRequest request, ServerCallContext context) {
            _logger.LogInformation("Single reply requested by {Name}", request.Name);
            return Task.FromResult(GetCurrentStreamReply(request));
        }

        private StreamReply GetCurrentStreamReply(StreamContextRequest request) => new StreamReply { Message = $"{DateTime.Now}: Hi {request.Name}. Have the current random: {_randomService.CurrentRandomInt}" };
    }
}