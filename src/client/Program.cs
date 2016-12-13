﻿using System;

using Grpc.Core;

using Dd;

public static class Program {
    public static int Main(string[] args) {
        if(args.Length < 1) {
            Console.WriteLine("Usage: dotnet client.dll {PORT}");
            return -1;
        }
        int port;
        if(!int.TryParse(args[0], out port)) {
            Console.WriteLine("invalid format for port, use a number");
            return -1;
        }
        try {
            var channel = new Channel($"127.0.0.1:{port}", ChannelCredentials.Insecure);
            var client = new DictionaryService.DictionaryServiceClient(channel);
            var callOptions = new CallOptions(deadline: DateTime.UtcNow.Add(TimeSpan.FromSeconds(3)));

            // Set key
            var setRequest = new SetRequest { Key = "foo", Value = "bar" };
            var setResponse = client.Set(setRequest, callOptions);
            Console.WriteLine($"setRequest={setRequest}");
            Console.WriteLine($"setResponse={setResponse}");

            // Retrieve key
            var getRequest = new GetRequest { Key = "foo" };
            var getResponse = client.Get(getRequest, callOptions);
            Console.WriteLine($"getRequest={getRequest}");
            Console.WriteLine($"getResponse={getResponse}");
            channel.ShutdownAsync().Wait();
            Console.WriteLine("\nGRPC client exiting");
        } catch (Exception e) {
            Console.WriteLine($"There was an error: {e.Message}");
            return -1;
        }
        return 0;
    }
}
