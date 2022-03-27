# Ishan Pranav\'s REBUS
A student-developed multi-user space trading game.
## Design
Ishan Pranav\'s REBUS simulates a dialogue between a player\'s character and their microcomputer assistant. The player works with their character\'s microcomputer to issue orders to spacecraft under their control.

Although pseudo-random procedural generation may initialize the in-game universe, randomness is not used thereafter. The game is deterministic with a fixed seed and contains no construct intended to simulate the passage of time.
## Implementation
The game is implemented with a server (either users communicating with a Discord bot or a TCP/IP server) and a database provider (SQLite). Players may join and leave the game at will, collaborating and competing with one another via orders sent simultaneously or asynchronously. 

The software tokenizes user input, parses the tokens to construct an abstract syntax tree, and executes the given instructions. The interpreter responds with well-formed English sentences that describe the situations that player-controlled spacecraft face and provide context to aid the player\'s decision-making.
