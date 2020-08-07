# Unity Multiplayer and Architecture using Command Pattern
## 1. Introduction

With growing intrest in multiplayer games, many of you, game developers may stumble upon a problem: *how do I make a multiplayer that works?* Unity no longer supports UNET and it is a small challenge to make your own networking. However your solution will be the best, because you, as developer, know what your game needs. Also it is crucial that if you want to have a multiplayer game, you gotta think about it from the start. Singleplayer is just one player multiplayer, but multiplayer can't be a singleplayer for many players. Of course if we are talking about playing via the Internet.

### 1.1 But... How multiplayer works?
Simply speaking, we have to know how computers communicate. As your PC is connected to the Internet it sends data in UDP datagrams or TCP packets that are routed to other computers. They contain a data such as: IP Address, port, payload (**data we are sending**) and other TCP or UDP additional fields. These are the things you have to know:

#### TCP

Before you communicate, you have to establish a connection. Everytime a TCP packet is send and it arrives, the receiver have to send information back that he indeed got the data. Implements a stream, so all data will be in exact order it was sent. It is slower than UDP because of its reliability, but in many cases TCP is enough for a slower paced game as latency won't be a big deal. In this tutorial I will use TCP. 

#### UDP

UDP datagrams are send and expected to be received. However we are not sure that our data has arrived at its destination. UDP is used in video streaming as one frame missing won't make a difference in a Shrek movie. There is a way to make UDP reliable, but we have to make our own protocol to track datagrams. 

#### Idea of a Multiplayer: Simultaneous Simulations

Useful article from practical point of view:
https://www.gamasutra.com/view/feature/131503/1500_archers_on_a_288_network_.php

Base idea goes like this:
Let's separate our game into two structures: 
- **game data** (player positions, level and health of the monsters) 
- **game application** (code that operates on some data)

So our game is just a set of some operations on data: movement is just a change in position, attacking is reducing someone's health. A simulation of some sort that takes input from the player. What if we reexecuted all the operations on all multiplayer clients that begin with the same starting **game data**?  

In game example:
When I cast a spell that heals my avatar, other players will see that my HP is no longer 10/100, but 100/100. We have to propagate those operation to other players. Of course, in client-server architectur we have to first communicate with the server. When and how we do that exactly? 

### 1.2 Solutions

If we take a look at my example with healing we can see that some solutions are better than others. Lets consider two ways we can do it:

#### A - local execution and sending
*I will change data in my game and send information about it to the server. Server will send information to other players...*

I heal my avatar locally, I set my health from 10/100 to 100/100. I send information to the server: "I've healed my hero!" and it updates its game state and sends this message to other players. My enemy attacked and killed me before he got the message that I've healed myself. In his eyes I am dead. He sends info that he leathally attacked me. Server takes information: reduces my health from 100 to 70 and I got consistent game data with the server but not with other player.
(Often the server **is** some third player that hosts our game) 

This solution may cause a lot problems in very sticky situations causing our game datas to differ, when they should be consistent among all players. Desychnronization is something we want to avoid at all cost, since as a player we would have to reload all game data and that would take a lot of time. Remember that human is an impatient creature.
 
#### B - server is authoritative
*I will send what I want to do with my game data and the server will resend all the operations again to me and other players...*

I heal my avatar, so I send it to the server. Server resends this order to me and my enemy. What if he tries to attack me and kill me? He also sends his order that he wants to attack me to the server. So our server got two messages and it will resend them in order they arrived so there is little chance that we both have different versions of game state.

This makes our client "stupid" while the server has the authority over everything. We don't even have to run our calculations on data locally as server will do it for us and we can only focus on displaying data. This is how browser multiplayer games are so lightweight. However I think we as a game developer want to make our players host a game for their friends or just play singleplayer.

There is a lot of variations of this "server is authoritative" idea, but I think you get what it is all about.

Any drawbacks? It is slower and gives some input lag. This is not a perfect solution if game takes place in realtime and there a lot of calculations which can be non-deterministic (floats). Consider doing client-side predictions. Any multiplayer solution you take you still have to handle situations where client data diverge from server data and even **authoritative server** can't make sure that everything would be consistent and would go smoothly. 

### 1.3 Summary?

In this tutorial I'll try to make a sketch for you how to make multiplayer game architecture with authoritative server using TCP as our protocol. 

## 2. Command Design Pattern

This tutorial used some words as: "commands". Am I some fan of strategy games to talk like that? Yes, but in this case I've already knew that you will get here. We will be using a Command Design Pattern to execute our changes in the game data. Shortly speaking it is very lovely way to structure your code, especially with multiplayer game. If you really understand it, you will never want to go back and write games in any other way, because it is a simple and beautiful design pattern..

### 2.1 What is Command Design Pattern?

Lets say we have interface of some Item: potion, scroll, bomb? You name it. And they all have something in common: they can be used. Potion heals you, scroll turns you into a cat, bomb makes an epic explosion. So we have our inventory of these items and evertime we click Z,X or C we use them and the game will just execute an interface method ``` Item.use() ```. That is a simple and good solution, but in Object Oriented Programming we can do more than that.

However there are some bugs in our game, so we have to log in file that we executed our ```Item.use()```. Moreover we want to add a feature to our game: replays. In our simple solution there would be a problem in doing this. Of course it would be doable, but there is a better way to execute a method. We can **encapsulate** triggers into objects.

In the simplest form of the Command Design pattern (or at least variation we are going to use) we have interfaces such as:

- ICommand - with method ```execute()``` 
- IExecutor - with method ```executeCommand(Command)```

So in our case: 
```
[Serializable]
public class ItemUseCommand : ICommand{
 Item item;
 
 public ItemUseCommand(Item item)
 {
  this.item = item;
 }

public void Execute(GameData gameData)
 {
  item.use(gameData); 
 }

}
```

Concrete Executor would just take an Command that inheirits from ICommand and will execute them. However there are many ways we can execute it. If this is a client, we have to send this ICommand to the server so the server can execute it first and since our method is encapsulated into an object it can be easily send via network!
If we are testing something we can make our concrete IExecutor to save every Command so we can track our game and all changes in GameData to find bugs. What about replay? We just save all Commands in an Array and their timestamp and reexecute them from the start.

**However, I made a mistake on purpose in above code. Notice that we can't send a reference via network and we have to map item so we can make sure that we are talking about the same item in the GameData.**
 
Command Design Pattern is a huge and powerful tool in structuring your code especially in networking as Commands can be serialized and send to other computers. You can modify your command with ```undo()``` that reverts changes to the GameData. Do you now know how CTRL+Z is implemented in programs like Photoshop? :)

### 2.2 Bigger picture

To make it more clear, I want to give you a simple example how this will work in a multiplayer game.

1) I press button that executes function that sends my command into my one IExecutor.
2) I execute Command C on my *ClientExecutor* CE that sends data to the server.
3) Server receives Command C as serialized JSON.
4) It has all the data needed to execute on *ServerExecutor* - executes it locally and sends Command C to all players.
5) We handle our data in such a way that we have our *ClientExecutor* and additional method that executes a Command but this time: locally without sending it to the server.
6) Our GameData has changed. Update our game: play a sound, change UI or do something else.


## 3. Implementation Idea

This tutorial won't be focused on programming, since I assume you know how to write code. If you find yourself unable to make a good architecture or just want to see what is the practical side of all I've spoken about, here it is:

### 3.1 Game State - all the game data.

Just because it is Unity, that doesn't mean you should mix everything together and use MonoBehaviours and scenes. That is the wisdom I want to tell you first. Of course it depends on game, but in many cases you shouldn't use MonoBehaviours for everything. I want you to see Unity just as a screen to display our game.

We are gonna make a class that stores **all the game related data** such as: items, monsers, map etc. This class should be serializable, because player should have a possibilty to late join current session. Not only that. Saving and loading game would be super easy too. So here is an example:

```
[Serializable]
public class GameState 
{
 [SerializeField]
 private int score;
 [SerializeField]
 private List<Monser> monsters;
 ...
 [SerializeField]
 private PlayerList playerList;
 etc...
}
```

You should also make a static class that serializes/deserializes GameState. From file or from data sent via network. See: **JsonUtility** in Unity. 

### 3.2 Commands, Commands and... Executors
Earlier I've gave you the way they works, so this should be fairly simple:

```
[Serializable]
public interface ICommand 
{
 void Execute(GameState gameState);
}

[Serializable]
public class SomeCommand : ICommand 
{
 [Serializable]
 int data;
 
 void Execute(GameState gameState)
 {
  gameState.someObject.someMethod(data);
 }
}

```
What is crucial in here: **don't make your Executions too complex.** All logic should be inside GameState objects. AttackCommand should specify: attacker, target and execute method like ```Attack(Entity target)```. We just need commands to do some atomic operations that will be sent using network.

Some upgrade I've come up with: ```bool Execute(GameState gameState)``` can be used for extra control. Sometimes we shouldn't resend some command that for example doesn't do anything because we are accessing something that is outside our world map.

We can't forget about making a Executor class, that will execute our commands.
Shortly:
```
public interface IExecutor 
{
 void Execute(ICommand command);
}

public class ServerExecutor : IExecutor 
{
.
.
.
 void Execute(ICommand command)
 {
  command.Execute(this.gameState);
  this.server.SendToAll(command);
 }
}

public class ClientExecutor : IExecutor {
 .
 .
 .
 
 void Execute(ICommand command) //This fires up when we do something locally
 {
  this.client.Send(command);
 }
 
 void UpdateGameState(ICommand command) //This fires up when we get command from the server
 {
  command.Execute(this.gameState)
 }
}

```
Remember to use **Observer Pattern** (C# Events) to notify your Views about changing GameState.

### 3.3 What command is this? Clever workaround.

As we send our data in serialized form, we don't know what type of object we just want to have deserialized. *Is this Command A, or Command B? I don't know!* So how we can fix this? Here is my solution, that would cost us an additional field in every command we send. 

Just make a class:
```
[Serializable]
public class SerializableClass
{
    [SerializeField]
    private string serializedClassName;

    public SerializableClass()
    {
        serializedClassName = this.GetType().ToString();
    }
}
```
And make sure that our particular ICommands also inheirit from SerializableClass.
Then in our Command deserializer add something like this:
```
public static ICommand StringToCommand(string msg)
{
    SerializableClass ctype = JsonUtility.FromJson<SerializableClass>(msg);
    Type t = Type.GetType(ctype.GetClassName());
    ICommand gc = (ICommand)JsonUtility.FromJson(msg, t);
    return gc;
}
```
This is based on a trick that we can deserialize serialized object A into object B if object A has all the fields that B have, at least when we talk about serializable fields.
Rule is: *Serializer/Deserializer doesn't care about object, only cares about fields of that object.*

## 4. Client-server

If you structured your code just like above now implementing multiplayer should be an easy task. You just need a simple TCP socket server and client class (see code in my repository to see how you can do this). Then add an internal protocol for serialized packets. Why we need that?

See:
```
Client:
		JumpCommand jc = new JumpCommand(10f); 
		=>
		Execute(jc)
		=>
		Send(jc)

		DrinkCommand dc = new DrinkCommand(DRINK.Beer); 
		=>
		Execute(dc)
		=>
		Send(dc)

Server:
		Two packets can bre received as two separate packets: "{serialized(jc)}" and "{serialized(dc)}"
			or
		Two packets can be received as one packet: "{serialized(jc)}{serialized(dc)}" 
```

Sometimes a serialized Command can be bigger than our NetworkStream, so we gotta handle those problems.

### 4.1 Solutions?
#### Control char
It is pretty simple. We would just put a char like **$** or **@** at the end of each "{serialized(command)}" that we send to the server. Basically: you should remember all data received until this control char goes into stream.

#### Pre-header
At the begining of each "{serialized(command)}" that we send to the server we would add a header that would contain all information about this payload. Header should be minimal in size. One integer that tells us about the length should be enough.

## 5. The end and additional resources

I am hopeful that this tutorial gave you an insight how one would go about implementing custom networking for Unity engine. In this repository there is an Unity project with an implementation that works fairly good (still needs some improvements). It is a great basis for starting off and making your own multiplayer game. I also leave here resources that can help you. Have a good deving!


