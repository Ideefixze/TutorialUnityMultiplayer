# Client and Server

## Introduction
This part won't contain a single line of code. In this repository there are files with source code I am using and I believe that you have at least basic knowledge how socket programming works. If you have never programmed an app using sockets then you should get comfortable in using those before you jump into doing custom networking in Unity. 

## Server
In this project we will use C# socket programming using asynchronous functions as they are relatively easy to use. In TCP our server socket listens on a port, awaiting any incoming connections. Upon getting a new connection, server will add this newly created client socket into his list of clients. Those clients (**ClientData** in the source code) now will be sending some data to the server. As you suspect, in most of the cases, server will receive JSON serialized Commands. Then it should handle those strings by deserializing them and using in server's command executor. See: [Server](/MultiplayerArchitectureUnity/Assets/Scripts/Networking/Server.cs)

## DataHandlers and ConnectionHandlers
**ClientData** is not responsible for handling data. Using some OOP we should make other classes/interfaces that will make use of received data. DataHandler should receive ```bytes[]``` and make it a string. It is JSON string, so it could be deserialized and then executed on our server.

Same would apply with ConnectionHandler. However, in this case, whenever a connection is made, our server sends back a **Game Data** (aka our current **Game State**) to newly connected player.

## Async and Unity

Unity uses one main thread and whenever any of the asynchronous functions try to execute a MonoBehaviour function on different thread, Unity shows up an error. This is one of the problems you may end up getting while using Async Socket Programming. However, we can just put those functions into a list of actions that will execute them on main thread. See: [ThreadManager](/MultiplayerArchitectureUnity/Assets/Scripts/Clientside/ThreadManager.cs)

**This trick was used by Tom Weiland in his tutorial of C# Server programming**

## Client
This class should be fairly simple as it's only job is to connect, send, receive data and eventually disconnect. Some things that are used in server class can be applied to this Client class. Of course, it needs it's own **DataHandler** too.
See: [Client](/MultiplayerArchitectureUnity/Assets/Scripts/Networking/Client.cs)

## What's next?
Next? This is all we need! You can now send commands, execute them on both sides, and now there is nothing that would stop you from presenting those changes in your game! Step-by-step solution for perfect customized networking for Unity doesn't exist. This is just a tip of an iceberg, but it is enough to be a strong basis of future projects. The next thing should be reading this code repository or playing with it. I will also leave you a bunch of good tutorials and articles that helped me a lot and will make your adventure with making multiplayer much easier.


Command Pattern:
- https://refactoring.guru/design-patterns/command
- https://medium.com/gamedev-architecture/decoupling-game-code-via-command-pattern-debugging-it-with-time-machine-2b177e61556c

Networking:
- https://www.gabrielgambetta.com/client-server-game-architecture.html
- https://www.gamasutra.com/view/feature/131503/1500_archers_on_a_288_network_.php

Client/Server implementation:
- https://docs.microsoft.com/pl-pl/dotnet/framework/network-programming/asynchronous-client-socket-example
- https://docs.microsoft.com/pl-pl/dotnet/framework/network-programming/asynchronous-server-socket-example
