# Client and Server

## Introduction
This part won't contain a single line of code. In this repository there are files with source code I am using and I believe that you have at least basic knowledge how socket programming works. If you have never programmed an app using sockets then you should get comfortable in using those before you jump into doing custom networking in Unity. 

## Server
In this project we will use C# socket programming using asynchronous functions as they are relatively easy to use. In TCP our server socket listens on a port, awaiting any incoming connections. Upon getting a new connection, we will add this newly created client socket into our list of clients. Those clients now will send us some data. As you suspect, in most of the cases, we will receive JSON serialized Commands. Then we should handle those strings by deserializing them and using in server's command executor.

## Async and Unity

Unity uses one main thread and whenever any of the asynchronous functions try to execute a MonoBehaviour function on different thread, Unity shows up an error. This is one of the problems you may end up getting while using Async Socket Programming. However, we can just put those functions into a list of actions that will execute them on main thread. See: [ThreadManager](//MultiplayerArchitectureUnity/Assets/Scripts/Clientside/ThreadManager.cs)
