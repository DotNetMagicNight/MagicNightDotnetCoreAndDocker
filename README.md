San Diego .NET Magic Night December 2016
========================================

Dependencies
------------
1. [.NET Core 1.1](https://www.microsoft.com/net/core#windowsvs2015)
2. [Docker](https://www.docker.com/products/overview#/install_the_platform)
3. [Docker Compose](https://docs.docker.com/compose/overview/)
4. Optionally GNU Make

Other technologies we will be using
-----------------------------------
1. [GRPC](http://www.grpc.io/docs/tutorials/basic/csharp.html)
3. Google [Protocol Buffers 3.0](https://developers.google.com/protocol-buffers/docs/proto3)

What is in this repository
--------------------------
1. C# GRPC server exposing two calls, Get(key) and Set(key, value) (see `src/server/`)
2. A way to spin up three nodes running the server. (see `src/server/docker-compose.yml`, use it with `make docker_run_servers`)
3. `Protobuf` message types and `GRPC` server definition (`src/protos/service.proto`) 
4. Sample `C#` client (see `src/client/`)
5. `protoc 3.0` compiler (see `src/packages/Grpc.Tools.1.0.0/tmp/packages/Grpc.Tools.1.0.0/tools/{OS}`)
6. Nice commands to rebuild the server, the client, the Docker images, and running the containers (see `Makefile`)

The Challenge(s)
----------------
1. Implement an RPC call to list all the keys on a given node.
2. Design and implement a way to partition the data among nodes in order to create buckets of keys that are handled by a single node.
3. Make the system fault-tolerant in the event of losing one node (meaning, if a node had a key/value and it goes away, that you can still find that key/value).