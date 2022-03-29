# Ishan Pranav\'s REBUS
A student-developed multi-user space trading game.
## API Documentation
The public API is documented on the [repository website](https://ishanpranav.github.io/rebus/).
## License
This repository is licensed with the [MIT](LICENSE.txt) license.
## Attribution
This software uses third-party libraries or other resources that may be
distributed under licenses different than the software. Please see the third-party notices included [here](THIRD-PARTY-NOTICES.txt).
## References
- The implementation of the [recursive descent parser](src/Rebus/Parser.cs) was inspired by and based on [this](https://en.wikipedia.org/wiki/Recursive_descent_parser) Wikipedia article.
- The implementation of the [word wrap algorithm](src/Rebus.Server.Tcp/Wrapper.cs) was inspired by and based on [this](https://stackoverflow.com/questions/52605996/c-sharp-console-word-wrap) Stack Overflow question. Please see the full attribution in the code file [here](src/Rebus.Server.Tcp/Wrapper.cs).
- The implementations of the [Levenshtein](src/Rebus/EditDistances/LevenshteinEditDistance.cs) and [Damerau–Levenshtein edit distance algorithms](src/Rebus/EditDistances/DamerauLevenshteinEditDistance.cs) were inspired by and based on the Wagner–Fischer algorithm described in [this](https://en.wikipedia.org/wiki/Damerau%E2%80%93Levenshtein_distance) Wikipedia article.
- The implementation of the [hexagonal coordinate system](src/Rebus/HexPoint.cs) was inspired by and based on [this](https://www.redblobgames.com/grids/hexagons/) article by [Amit Patel](http://www-cs-students.stanford.edu/~amitp/).
- The implementations of [Dijkstra\'s algorithm](src/Rebus/Pathfinders/DijkstraPathfinder.cs) and the [A* search algorithm](src/Rebus/Pathfinders/AStarPathfinder.cs) were inspired by and based on [this](https://en.wikipedia.org/wiki/A*_search_algorithm) Wikipedia article.
