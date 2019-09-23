using Grpc.Core;
using Grpc.Net.Client;
using StreamServer;
using System;
using System.Threading.Tasks;

namespace StreamConsumerClient
{
    internal static class Program
    {
        private static async Task Main() {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");

            var client = new Streamer.StreamerClient(channel);
            Console.WriteLine("Enter your name:");
            var replyStream = client.GetReplyStream(new StreamContextRequest { Name = Console.ReadLine() });
            await foreach (var streamReply in replyStream.ResponseStream.ReadAllAsync())
                Console.WriteLine(streamReply.Message);
        }
    }
}