# Design
Rebus is a simulation of a dialogue between a _player\'s character_ and their _navigational microcomputer_. The player works with their character\'s microcomputer to issue orders to _spacecraft_ under their control.
## Spacecraft
All spacecraft are functionally identical.

Each may carry at most one unit of _cargo_ (<!-- either -->a physical _commodity_<!-- or a group of _passengers_ -->) at any moment. <!--Non-passenger cargo--> A player may order a spacecraft to _jettison_ its cargo at any time. Although each type of cargo is uniquely identified by its _weight_, this property does not affect a spacecraft\'s ability to carry it.
## Navigation
The game universe is a grid of _region of space_ which may be
- _empty_, containing no permanent features;
- _stellar_, each containing one or more _stars_; or
- _planetary_, each containing one or more _planets_.

Stars are impassible; spacecraft may not enter stellar regions. All other celestial bodies are categorized as planets. Within the context of the game and its codebase, the term \"planet\" broadly refers to astronomical planets, as well as moons, satellites, and asteroids.

The regions immediately
- _north_,
- _south_,
- _east_, and
- _west_

of a region are considered _adjacent_ to it.
### Exploration
To explore, a player instructs a spacecraft to travel into an adjacent region. Exploration is blind: Any prior information about a destination and its contents may be incomplete, unreliable, or outdated until a spacecraft enters and observes the reality of the situation.
### Autopilot
Once a player\'s spacecraft encounters a planet for the first time, the microcomputer memorizes its location and may chart a course and travel to it upon request using _autopilot_ technology.

Even while autopilot is enabled, a spacecraft must enter and exit each intermediate region along the course before reaching its intended destination.
## Interception
If a player\'s spacecraft enters a region occupied by an _enemy_ spacecraft (not controlled by that player), then the enemy _intercepts_ the player, whose spacecraft must stop. The autopilot is immediately disabled.
## Conflict
A player may order a stationary spacecraft to adopt a
- _passive_ stance, requiring it to take no action after intercepting an enemy, or a
- _defensive_ stance, requiring it to initate a conflict against every enemy spacecraft in the region.

If a player enters a region containing only passive spacecraft, they may order their spacecraft to undertake an _offensive_, initiating a conflict against the passive enemy.

During a state of conflict, a player _fleet_ is said to include all spacecraft belonging to the player currently within the region of conflict; all other spacecraft unite into the _opposing_ fleet. The _size_ of each fleet is the number of spacecraft it contains; the fleet with fewer is the _minor fleet_ and the other is the _major fleet_. If the player fleet and the opposing fleet are of the same size, a _standoff_ occurs: Neither a retreat nor a state combat commences.
### Retreat
The microcomputers of the spacecraft in the minor fleet will automatically attempt a retreat maneuver.

Beginning with north and searching south, east, and west, in that order, the microcomputer examines each adjacent region and retreats into the first non-stellar region that is either empty or occupied only by other spacecraft controlled by the same player (that is, the first passable region where no enemy is present).

If no satisfactory adjacent region exists, the minor fleet fails to retreat and combat commences. Otherwise, combat is avoided.
### Combat
During an invasion, each fleet loses a number of spacecraft equivalent to three-fourths of the minor fleet\'s size. The heaviest spacecraft from each fleet are destroyed first. The remaining spacecraft in the minor fleet are captured and join the major fleet.

Let`m`be the initial size of the minor fleet; let`M`be the initial size of the major fleet. The final size of the major fleet,`N` is given by the following formula:
```python
N = M - (0.75 * min(M, m))
```
## Commerce
A player with at least one spacecraft in a region may conduct _commerce_ with any planet within that region by exchanging _wealth_ (the game\'s currency) for cargo.

At an _inhabited_ planet, a player may
- _sell_ commodities being imported by the planet <!-- , --> and
- _purchase_ commodities being exported by the planet<!-- , -->.

<!--
- _accept_ groups of passengers of the planet\'s primary species, and
- _deposit_ groups of passengers of the planet\'s primary species.
-->

Each purchase increases the unit _selling_ price that the exporting planet requests in exchange for the commodity; each sale increases the unit purchasing price that the importing planet is willing to pay for the commodity.

The quantities supplied and demanded by each planet are unlimited. The amount that a player sells is limited only by their cargo; the amount that a player purchases is limited only by their wealth and the total capacity of all spacecraft in the region.

At an _uninhabited_ planet, a player may
- _deposit_ cargo<!--, --> or
- _harvest_ commodities available on the planet<!-- or -->.

<!-- deposit groups of passengers of any species, so long as all the passengers deposited are of the same species.
Each planet has a fixed _population size_. Accepting a group of passengers decreases the _population_ of the planet.

Depositing the first group of passengers onto an uninhabited planet converts it to an inhabited one with the species of the passengers becoming the planet\'s new primary species; accepting the final group of passengers from -->
## Interactions
Aside from out-of-game communication, players may broadcast information to all spacecraft within the region: They may transmit
- simple textual messages
- navigational information enabling recipients to autopilot to a planet or star.

## Future considerations
It is my intention to keep the software and game design as simple as possible in order to craft a polished prototype. However, I am currently evaluating certain additional features which, while entertaining, are not essential to the game\'s functioning.

While each inhabited planets is planned to contain zero or one cargo port, extra buildings under consideration include
- **terminals** offering passengers as an alternative form of cargo (this facilitates colonization -- players may accept passengers from inhabited planets and deposit them onto uninhabited ones to inhabit them or evacuate one planet and migrate its population to planet; population size may have an impact on the economy, and passengers would not be able to be jettisoned; passengers would be considered heavier commodities)
- **banks** allowing a player to accept or deposit wealth into a common pool for any player to collect (this facilitates contracts, transfers of wealth between players, and races between players to arrive at the bank and withdraw first),
- **shipyards** allowing players to purchase ships,
- **fuel stations** allowing players to purchase fuel required for movement, and
- **embassies**: allowing players to negotiate government intervention in the planet\'s economic system (price floors and ceilings).
