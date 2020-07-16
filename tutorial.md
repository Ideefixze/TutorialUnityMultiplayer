# Unity Multiplayer and Architecture
## Introduction

With growing intrest in multiplayer games, many of you, game developers may stumble upon a problem: *how do I make a multiplayer that works?* Unity no longer supports UNET and it is a small challenge to make your own networking. However your solution will be the best, because you, as developer, know what your game needs. Also it is crucial that if you want to have a multiplayer game, you gotta think about it from the start. Singleplayer is just one player multiplayer, but multiplayer can't be a singleplayer for many players. Of course if we are talking about playing via the Internet.

## But... How multiplayer works?
Simply speaking, we have to know how computers communicate. As your PC is connected to the Internet it sends data in UDP datagrams or TCP packets that are routed to other computers. They contain a data such as: IP Address, port, payload (**data we are sending**) and other TCP or UDP additional fields. I am not going to explain everything about TCP and UDP here, but these things you have to know:

#### TCP

Before you communicate, you have to establish a connection. Everytime a TCP packet is send and it arrives, the receiver have to send information back that he indeed got the data. Implements a stream, so all data will be in exact order it was sent. It is slower than UDP because of its reliability, but in many cases TCP is enough for a slower paced game as latency won't be a big deal. In this tutorial I will use TCP. 

#### UDP

UDP datagrams are send and expected to be received. However we are not sure that our data has arrived at its destination. UDP is used in video streaming as one frame missing won't make a difference in a Shrek movie. There is a way to make UDP reliable, but we have to make our own protocol to track datagrams. 

#### Idea of a Multiplayer: Simultaneous Simulations

https://www.gamasutra.com/view/feature/131503/1500_archers_on_a_288_network_.php

Let's separate our game into two: 
- **game data** (player positions, level and health of the monsters) 
- **game application** (code that operates on some data)

So our game is just a set of some operations on data: movement is just a change in position, attacking is reducing someone's health. A simulation of some sort that takes input from the player. What if we reexecuted all the operations on all multiplayer clients that start with the same **game data**? 

When I cast a spell that heals my avatar, other players will see that my HP is no loger 10/100, but 100/100.

### Solutions

If we take a look at my example with healing we can see that some solutions are better than others. Lets consider two ways we can do it:

#### A - local execution and sending
*I will change data in my game and send information about it to the server. Server will send information to other players..*

I heal my avatar locally, I set my health from 10/100 to 100/100. I send information to the server: "hey, I've healed my guy!" and it updates its game state and sends this message to other players. My enemy attacked me before he got the message that I've healed myself and he sees it as his victory. He slains me and sends info that he leathally attacked me. Server takes information: reduces my health from 100 to 70 and I got it. 

This solution may cause a lot problems in very sticky situations causing our game datas to differ, when they should be consistent among all players.
 
#### B - server is authoritative
*I will send what I want to do with my game data and the server will resend all the operations again to me and other players*

I heal my avatar, so I send it to the server. Server resends this order to me and my enemy. What if he tries to attack me and kill me? He also sends his order that he wants to attack me to the server. So our server got two messages and it will resend them in order they arrived so there is little chance that we both have different versions of game state.

This makes our client "stupid" while the server has the authority over everything. We don't even have to run our calculations on data locally as server will do it for us and we can only focus on displaying data. This is how browser multiplayer games are so lightweight. However I think we as a game developer want to make our players host a game for their friends or just play singleplayer.

### Summary?

In this tutorial I'll try to teach you how to make multiplayer game architecture with authoritative server using TCP as our protocol. 

## Commands? Executions?



  


