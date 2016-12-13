using System;
using System.Collections.Generic;

using System.Threading;
using System.Threading.Tasks;

using Grpc.Core;

using Dd;

public class DictionaryServiceImpl : DictionaryService.DictionaryServiceBase {

    // Fields
    private static Dictionary<string, string> _dict = new Dictionary<string, string>(10000);

    // Methods
    public override Task<GetResponse> Get(GetRequest request, ServerCallContext context) {

        // Try to find the key locally
        string value;
        if(_dict.TryGetValue(request.Key, out value)) {
            return Task.FromResult(new GetResponse { Value = value, Node = "current", Found = true });
        }
        return Task.FromResult(new GetResponse { Found = false });
    }

    public override Task<SetResponse> Set(SetRequest request, ServerCallContext context) {
        _dict[request.Key] = request.Value;
        return Task.FromResult(new SetResponse { Success = true });
    }
}

public class Program {
    private static AutoResetEvent autoEvent = new AutoResetEvent(false);

    public static int Main(string[] args) {

        // Validate arguments
        if(args.Length < 1) {
            Console.WriteLine("Usage: dotnet server.dll {PORT}");
            return -1;
        }
        int port;
        if(!int.TryParse(args[0], out port)) {
            Console.WriteLine("invalid format for port, use a number");
            return -1;
        }

        // Build the gRPC server
        Console.WriteLine("Firing off GRPC server");
        var server = new Server {
            Services = { DictionaryService.BindService(new DictionaryServiceImpl()) },
            Ports = { new ServerPort("*", port, ServerCredentials.Insecure) }
        };

        // Launch the gRPC server
        ThreadPool.QueueUserWorkItem(_ => {
                server.Start();
                Console.WriteLine($"GRPC server listening on port {port}");
            }
        );
        autoEvent.WaitOne();
        server.ShutdownAsync().Wait();
        Console.WriteLine("Shutting down ...");
        return 0;
    }
}
