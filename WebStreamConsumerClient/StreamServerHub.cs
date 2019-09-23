using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.SignalR;
using StreamServer;
using System.Collections.Generic;
using System.Threading;

namespace WebStreamConsumerClient
{
    public class StreamServerHub : Hub
    {
        public async IAsyncEnumerable<StreamReply> BeginStream(string name, CancellationToken cts) {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");

            var client = new Streamer.StreamerClient(channel);
            var replyStream = client.GetReplyStream(new StreamContextRequest { Name = name }, cancellationToken: cts);
            await foreach (var streamReply in replyStream.ResponseStream.ReadAllAsync(cts))
                yield return streamReply;
        }
    }
}