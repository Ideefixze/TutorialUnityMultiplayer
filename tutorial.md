# Unity Multiplayer and Architecture
## Introduction

With growing intrest in multiplayer games, many of you, game developers may stumble upon a problem: *how do I make a multiplayer that works?* Unity no longer supports UNET and it is a small challenge to make your own networking. However your solution will be the best, because you, as developer, know what your game needs. Also it is crucial that if you want to have a multiplayer game, you gotta think about it from the start. Singleplayer is just one player multiplayer, but multiplayer can't be a singleplayer for many players. Of course if we are talking about playing via the Internet.

## But... How multiplayer works?
Simply speaking, we have to know how computers communicate. As your PC is connected to the Internet it sends data in UDP datagrams or TCP packets that are routed to other computers. They contain a data such as: IP Address, port, payload (**data we are sending**) and other TCP or UDP additional fields. I am not going to explain everything about TCP and UDP here, but these things you have to know:

#### TCP

Before you communicate, you have to establish a connection. Everytime a TCP packet is send and it arrives, the receiver have to send information back that he indeed got the data. Implements a stream, so all data will be in exact order it was sent. It is slower than UDP because of its reliability, but in many cases TCP is enough for a slower paced game as latency won't be a big deal. In this tutorial I will use TCP. 

#### UDP

UDP datagrams are send and expected to be received. However we are not sure that our data has arrived at its destination. UDP is used in video streaming as one frame missing won't make a difference in a Shrek movie. There is a way to make UDP reliable, but we have to make our own protocol to track datagrams. 

#### Idea behind multiplayer


