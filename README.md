# YellFighter
TasJam 2015

Yell Fighter
A Tasjam 2015 entry by Team Konami Code. We decided to take the theme of voices quite literally by implementing a game with voice controls.

Yell Fighter is a modern take on the classic beat 'em up. Instead of mashing buttons, you and a friend can yell your commands at your character, causing them to punch, kick, jump and throw fireballs at each other. Or not as the case may be most of the time. In addition scores are routed out via an arduino to a physical life meter, complete with a 'bell' that rings at the start and end of the round.

So it turns out that building a networked multi player game with voice controls and a physical hardware interface is a bit of a challenge to pull of in 32 hours. So what we've got here is a very rough first prototype of the game. Voice controls are mostly implemented, the hardware interface works too and in theory the network part works on a local host. But we've not had time to test any of these features, there are bugs galore and the game may not work in it's uploaded state. We've submitted really just to show we did something across the weekend, but the game is probably unplayable in its current state. Hey - we would rather fail running at full speed than succeed walking slowly.

With some luck and a bit of spare time we're keen to finish the game off - or atleast get it to a point where is plays as an actual game. Yelling at a screen in the wee hours of the morning is surprisingly relaxing, so we think we're onto something here.

Team Konami Code is : Troy Merritt, Cameron Yates, Chris Lawrence, John Kendall and Ryder Boynton. All trusted professionals.

Install instructions
Download, unzip.

Chances are the voice recognition server won't launch with the game (it should launch when you start the server, but if it doesn't then it's been included also so you can start the voice recognition server manually.

As there is no config options for the network at this stage, you can really only run the game by starting two copies, one as server and one as client. You'l then be able to jump around and have some fun yelling at the screen - I iterate again - it's quite fun.

Oh yeah - Windows only. the easiest to implement library for LOCAL voice recognition was the windows UDP server, so that's what we used.

Also, if you want to have a bash with the physical scoreboard... well, I guess email me and I'll send you a copy of the code and some instructions on how to build your own.



So - If it works and the server starts - just start saying commands into a microphone. The basic words are : Left, Right, Jump, Punch, Kick, Fire Ball, Jump Forward, Jump Back. There are also some more, but we forget.
