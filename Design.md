# Design
The game is a dialogue between a _player_ and their simulated _navigational computer_. Each player\'s character works with the computer to issue orders to _spacecraft_ under their control.
## Spacecraft
All spacecraft are functionally identical.

Each may carry at most one unit of _cargo_ (either a physical _commodity_ or a group of _passengers_) at any moment.
## Navigation
The game universe is a grid of _sectors_ categorized as
- _empty_, containing no permanent features;
- _stellar_, each containing one or more _stars_; or
- _planetary_, each containing one or more _planets_.

Stars are impassible; spacecraft may not enter stellar sectors.

Planets are any celestial bodies not categorized as stars. Within the context of the game and its codebase, the term \"planet\" broadly refers to astronomical planets, as well as moons, satellites, and asteroids.

The sectors immediately _north_, _south_, _east_, and _west_ of a sector are considered _adjacent_ to it.
### Exploration
To explore, a player instructs a spacecraft to travel into an adjacent sector. Exploration is blind: Any prior information about a destination and its contents may be incomplete, unreliable, or outdated until a spacecraft enters and observes the reality of the situation.
### Autopilot
Once a player\'s spacecraft encounters a planet for the first time, the navigational computer memorizes its location and may chart a course and travel to it upon request using _autopilot_ technology.

Even while autopilot is enabled, a spacecraft must enter and exit each intermediate sector along the course before reaching its intended destination.
## Interception
If a player\'s spacecraft enters a sector occupied by an _enemy_ spacecraft (not controlled by that player), then the enemy _intercepts_ the player, whose spacecraft must stop. The autopilot is immediately disabled.
## Conflict
A player may order a stationary spacecraft to adopt a _passive_ (default) or a _defensive_ interception policy.

Passive spacecraft allow intercepted enemies to enter and share the sector without intervention.

Defensive spacecraft implement a blockade, initiating a conflict whenever an enemy enters the sector.

If a player enters a sector containing only passive spacecraft, they may order their spacecraft to undertake an _offensive_, initiating a conflict against the passive enemy.

Conflict is not possible between fleets containing the same number of spacecraft.
### Retreat
When a conflict begins, if the player has fewer spacecraft in the sector than the enemy, then the navigational computer will automatically attempt to retreat.

Beginning with north and searching south, east, and west, in that order, the computer examines each adjacent sector and retreats into the first non-stellar sector that is empty or occupied only by other spacecraft controlled by the same player (the first passable sector where no enemy is present).

If no satisfactory adjacent sector exists, the smaller fleet fails to retreat and an invasion occurs.
### Invasion
During an invasion, the size of each fleet is reduced by three-fourths of the number of spacecraft in the smaller fleet. The heaviest spacecraft are destroyed first.

Let`N0`be the number of enemy spacecraft before the conflict; let`P0`be the number of the player\'s spacecraft before the conflict. The post-conflict sizes of the player fleet,`P1`, and of the enemy fleet,`N1`, respectively, are given by the formulae:
```python
N1 = N0 - (0.75 * min(N0, P0))
P1 = P1 - (0.75 * min(N0, P0))
```
## Commerce
### Planetary
A player with at least one spacecraft in a sector may conduct _commerce_ with any planet within that sector.

At an _inhabited_ planet, a player may
- sell unlimited quantities any commodities being imported by the planet,
- purchase unlimited quantities any commodities being exported by the planet, and
- accept passengers (each inhabited planet contains a fixed quantity).

Purchasing each unit of a planet\'s export increases the planet\'s unit selling price for that commodity. Selling each unit of a planet\'s import decreases the unit purchasing price  for that commodity.

At an _unhabited_ planet, a player may:
- accept commodities available on the planet (each unhinabited contains a fixed quantity) and
- deliver passengers (making the planet inhabited).

With each purchase of an export, the decrease in supply causes an increase in that export\'s unit price.
