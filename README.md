# Ishan Pranav\'s REBUS
A student-developed multi-user space trading game.
## Design
Ishan Pranav's REBUS simulates a dialogue between a _player\'s character_ a microcomputer assistant. The player works with their character\'s microcomputer to issue orders to spacecraft under their control.

Although pseudo-random procedural generation may initialize the in-game universe, randomness is not used thereafter. The game is deterministic with a fixed seed and contains no construct intended to simulate the passage of time.

Please refer to the detailed game documentation [here](Design.md).
## Implementation
The game is implemented with a server (either users communicating with a Discord bot or a TCP/IP server) and a database provider (SQLite). Players may join and leave the game at will, collaborating and competing with one another via orders sent simultaneously or asynchronously. 

A [recursive-descent parser](https://en.wikipedia.org/wiki/Recursive_descent_parser) tokenizes user input, constructs an abstract syntax tree, and executes the given instructions. The interpreter responds with well-formed English sentences that describe the situations that player-controlled spacecraft face and provide context to aid the player\'s decision-making.
## License
This repository is licensed with the [MIT](LICENSE) license.
## Attribution
This software uses third-party libraries or other resources that may be
distributed under licenses different than the software. Please see the third-party notices included [here](THIRD-PARTY-NOTICES.txt).
