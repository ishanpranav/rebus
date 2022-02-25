# Design
Rebus simulates a dialogue between a _player\'s character_ and their _navigational microcomputer_. The player works with their character\'s microcomputer to issue orders to _spacecraft_ under their control.
## Spacecraft
All spacecraft are functionally identical.

Each may carry at most one unit of _cargo_ (<!-- either -->a physical _commodity_<!-- or a group of _passengers_ -->) at any moment. <!--Non-passenger cargo--> A player may order a spacecraft to _jettison_ its cargo at any time. Although each type of cargo is uniquely identified by its _mass_, this property does not affect a spacecraft\'s ability to carry it.
## Navigation
The game universe is a grid of _regions of space_ which may be
- _empty_, containing no permanent features;
- _stellar_, containing one or more _stars_; or
- _planetary_, containing one or more _planets_.

Stars are _impassible_; spacecraft may not enter stellar regions.

All other celestial bodies are categorized as planets. The term \"planet\" broadly refers to astronomical planets, as well as moons, satellites, and asteroids.
### Exploration
To explore, a player instructs a spacecraft to travel to one of up to six regions adjacent to it.

Exploration is blind: Any prior information about a destination and its contents may be incomplete, unreliable, or outdated until a spacecraft enters and observes the region.
### Autopilot
When a player\'s spacecraft encounters a planet for the first time, the microcomputer memorizes its location and may chart a course to travel to it upon request using _autopilot_ technology.

Even while autopilot is enabled, a spacecraft must enter and exit each intermediate region along the course before reaching its intended destination.
## Interception
If a player\'s spacecraft enters a region occupied by an _enemy_ spacecraft (not controlled by that player), then the enemy _intercepts_ the player, whose spacecraft must stop. The autopilot is immediately disabled.

A player may order a stationary spacecraft to adopt a
- _passive_ stance, requiring it to take no action upon intercepting an enemy, or a
- _defensive_ stance, requiring it to initiate a conflict upon intercepting an enemy.
## Conflict
If a player\' spacecraft enters a region containing only passive enemy spacecraft, the player may order their spacecraft to undertake an _offensive_, initiating a conflict against the passive enemy.

During a conflict, the _player fleet_ includes all the player\'s spacecraft currently in the region of the conflict; all other spacecraft in the region unite into the _opposing_ fleet. Thus, the opposing fleet can include spacecraft controlled by several enemy players.

The _size_ of each fleet is the number of spacecraft it contains. The fleet with fewer is the _minor fleet_; the other is the _major fleet_.

If the player fleet and the opposing fleet are of the same size, a _standoff_ occurs: The conflict terminates.
### Retreat
The microcomputer assisting the player controlling the minor fleet automatically attempts a retreat maneuver when combat begins.

The microcomputer successively examines adjacent regions and retreats into the first non-stellar region that is either empty or occupied only by other spacecraft controlled by the same player.

If no passable adjacent region lacking enemy spacecraft exists, the minor fleet fails to retreat and engages in combat instead. Otherwise, the conflict terminates.
### Combat
During combat, each fleet loses a number of spacecraft equivalent to three-fourths of the minor fleet\'s size.

The most massive spacecraft from each fleet are destroyed first. The remaining spacecraft in the minor fleet are captured and join the major fleet. During combat, the major fleet always captures at least one spacecraft. As a result, the player controlling the major fleet always emerges from combat controlling all spacecraft in the region.

Let `m` be the initial size of the minor fleet; let `M(0)` be the initial size of the major fleet. The final size of the major fleet, `M(1)` is given by the following formula:
```python
M(1) = M(0) - max(1, 0.25 * m)
```
## Commerce
A player with a spacecraft in a region may conduct _commerce_ with any planet within that region by exchanging _wealth_ for cargo. Wealth is a game currency stored virtually within the microcomputer.

At an _inhabited_ planet, a player may
- _sell_ commodities being imported by the planet <!-- , --> and
- _purchase_ commodities being exported by the planet<!-- , -->.

<!--
- _accept_ groups of passengers of the planet\'s primary species, and
- _deposit_ groups of passengers of the planet\'s primary species.
-->

Each purchase increases the unit _selling price_ that the exporting planet requests in exchange for the commodity; each sale decreases the unit _purchasing price_ that the importing planet is willing to pay for the commodity.

The quantities supplied and demanded by each planet are unlimited. The amount that a player sells is limited only by their cargo; the amount that a player purchases is limited only by their wealth and the total capacity of their fleet in the region.

At an _uninhabited_ planet, a player may
- <!-- accept -->_accept_ cargo or
- _deposit_ cargo<!--, --><!-- or -->.

<!-- deposit groups of passengers of any species, so long as all the passengers deposited are of the same species.
Each planet has a fixed _population size_. Accepting a group of passengers decreases the _population_ of the planet.

Depositing the first group of passengers onto an uninhabited planet converts it to an inhabited one with the species of the passengers becoming the planet\'s new primary species; accepting the final group of passengers from -->
## Interactions
Aside from out-of-game communication, players may order a spacecraft to broadcast information to all other spacecraft within its region: They may transmit
- simple textual messages
- navigational information enabling recipients to autopilot to a planet or star.

## Future considerations
I intend to keep the software and game design as simple as possible to craft a polished prototype. However, I am currently evaluating certain additional features which, while entertaining, are not essential.

Potential planetary facilities under consideration include
- **terminals** offering passengers as an alternative form of cargo (this facilitates colonization -- players may accept passengers from inhabited planets and deposit them onto uninhabited ones to inhabit them or evacuate one planet and migrate its population to another; population size may have an impact on the economy, and passengers cannot be jettisoned; passengers have zero mass)
- **banks** that allow a player to accept or deposit wealth into a common pool for any player to collect (this facilitates contracts, transfers of wealth between players, and races between players to arrive at the bank to withdraw first),
- **shipyards** that allow players to purchase spacecraft,
- **fuel stations** that allow players to purchase fuel required for movement, and
- **embassies** that allow players to negotiate government intervention in the planet\'s economic system (via price floors and ceilings).
