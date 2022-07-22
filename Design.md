# Design
Ishan Pranav\'s REBUS simulates a dialogue between a _player\'s character_ and their _microcomputer_ assistant. The player works with their character\'s microcomputer to issue _orders_ to _spacecraft_ under their control.
## Spacecraft
All spacecraft are functionally identical.

Each has the _capacity_ to carry at most one unit of _cargo_  at any moment.

A player may order a group of spacecraft (_fleet_) to _jettison_ its cargo at any time. Jettisoned units of cargo are destroyed.

Although each type of cargo is uniquely identified by its _mass_, this property does not affect a spacecraft\'s ability to carry it.
## Navigation
The game universe is a grid of hexagonal _regions of space_ which may be
- _empty_, containing no permanent features;
- _stellar_, containing a _star_; or
- _planetary_, containing an _inhabited_ or an _uninhabited planet_.

Stars are _impassible_: Spacecraft may not enter stellar regions.

All other celestial bodies are planets [[1](#1)].
### Exploration
To explore, a player instructs a spacecraft to travel to one of six regions adjacent to it.

_Wealth_ is a game currency stored virtually within the microcomputer. Every time wealth is added to a player\'s negative balance, ten percent of the income is levied as an interest penalty. Banker\'s rounding is used to approximate interest amounts to the nearest integer.

Attempting to move a spacecraft into a region incurs a fuel cost measured in wealth.

Exploration is blind: Any prior information about a destination and its contents may be incomplete, unreliable, or outdated until a spacecraft enters and observes the region. A star may be observed from any region adjacent to its own. [[2](#2)]
### Autopilot
A player may use _autopilot technology_ to automate journeys to regions or planets which the player has previously explored. The autopilot always uses the shortest route through non-stellar regions already visited by the player. If multiple routes of equal length exist, then one is selected arbitrarily but deterministically. 

Data saved in the microcomputer function as a transferable currency: _navigational information_ [[3](#3)].

Even while autopilot is enabled, a spacecraft must enter and exit each intermediate region on the route before reaching its intended destination.
## Interception
When a player\'s spacecraft attempts to enter a region _occupied_ by an _adversary\'s_ spacecraft (all other players are considered adversaries), then the adversarial spacecraft _intercepts_ the player\'s, which must stop. The autopilot is immediately disabled.

Each maintains an _interception stance_ assigned by the owning player. These orders are _asynchronous_: They remain in effect even while the player is disconnected from the game server. When a player enters an adversary-occupied region, the occupant\'s regional stance determines the consequences. [[4](#4)]

### Passive stance
A player may order a spacecraft to remain _passive_ by designating an adjacent non-stellar _sanctuary_ region to which they will _retreat_ if an adversary attempts to enter and occupy the region.

If the sanctuary region is occupied by the same or another adversary, then the attempt to retreat fails and the passive spacecraft defaults to _protective_ behavior, joining any other spacecraft in the region that the player did not order to retreat.

### Protective stance
Spacecraft ordered to protect their region attempt to prevent adversaries from entering it.

The _sizes_ of the _occupying_ and _entering_ fleets are given by the number of spacecraft they contain. The fleet with fewer is the _minor_ fleet; the other is the _major_ fleet.

#### Standoff
If the occupying fleet and the entering fleet are of the same size, a _standoff_ occurs: The entering fleet holds its position and may not enter the adversary-occupied region.

#### Conflict
If no standoff occurs, each fleet loses a number of spacecraft equaling three-fourths of the minor fleet\'s size.

Spacecraft are destroyed in descending order based on the mass of their cargo [[5](#5)].

The remaining spacecraft in the minor fleet are captured and join the major fleet. During combat, the major fleet always captures at least one spacecraft [[6](#6)].
## Commerce
A player with a spacecraft in a region may conduct _commerce_ with any planet within that region by exchanging wealth for _commodities_ stored as cargo. [[7](#7)]. 

At an inhabited planet, a player may
- _sell_ commodities being imported by the planet <!-- , --> and
- _purchase_ commodities being exported by the planet<!-- , -->.

Each purchase increases the unit _selling price_ that the exporting planet requests in exchange for the commodity; each sale decreases the unit _purchasing price_ that the importing planet is willing to pay for the commodity.

The quantities supplied and demanded by each planet are unlimited. The amount that a player sells is limited only by their cargo; the amount that a player purchases is limited only by their wealth and the total capacity of their fleet. 

At an uninhabited planet, a player may
- _accept_ cargo,
- _deposit_ cargo,
- _download_ navigational information, or
- _upload_ navigational information.

## Notes
These comments are not essential to the specification of the game design but seek to provide clarifying examples and emergent functionality.
##### 1
Within the software and its design documentation, the term \"planet\" is defined more broadly than in astronomy. It may refer to moons, dwarf planets, space stations, satellites, and asteroids.
##### 2
Empty spacecraft may be used as scouts to avoid entering traps. 
##### 3
Navigational information is valuable since the vast universe size may be exploited by weaker players to escape stronger ones\' areas of control. A \"hide-and-seek\" mechanic emerges.
##### 4
No feature exists formalizing alliances. Only one player may occupy a region at a time. Inteception stances are indiscriminate: They do not respect informal agreements made by players outside of the game. Thus, they must be pre-arranged and deception is possible.
##### 5
Since planets and players determine demand, the mass of a unit of cargo is not necessarily associated with its value. Since spacecraft carrying the most massive cargo are destroyed first, it may be effective to reserve some spacecraft to carry less expensive, more massive, cargo to avoid the destruction of more valuable commodities.
##### 6
Let `m` be the initial size of the minor fleet, let `M(0)` be the initial size of the major fleet, let `c` be the number of spacecraft captured by the major fleet, and let `d` be the number of spacecraft destroyed. The final size of the major fleet, `M(1)`, is given by the following process:
```python
c = max(1, m / 4).
d = m - c.
M(1) = M(0) - d + c
     = M(0) - (m - c) + c
     = M(0) - m + (2 * max(1, m / 4)).
```
The final size of the minor fleet is always zero.

This system exhibits no randomness and can indirectly facilitate cargo exchanges.
##### 7
No feature exists allowing one player to directly transfer wealth to another. Trades may be simulated via cargo deposits on uninhabited planets. This offers an incentive to discover remote uninhabited planets for undisturbed bartering.

Piracy is possible by accepting unclaimed units of cargo before their intended recipient. A \"treasure hunt\" mechanic emerges. Such meetings also encourage traps and betrayals, limiting excessive player cooperation in favor of fairer competition (see [note 4](#4)).
## Future considerations
I intend to keep the software and game design as simple as possible to craft a polished prototype. However, I am currently evaluating certain additional features which, while entertaining, are not essential.

Potential planetary facilities under consideration include
- **terminals** offering passengers as an alternative form of cargo (this facilitates colonization -- players may accept passengers from inhabited planets and deposit them onto uninhabited ones to inhabit them or evacuate one planet and migrate its population to another; population size may have an impact on the economy, and passengers cannot be jettisoned; passengers have zero mass)
- **banks** that allow a player to accept or deposit wealth into a common pool for any player to collect (this facilitates contracts, transfers of wealth between players, and races between players to arrive at the bank to withdraw first),
- **shipyards** that allow players to purchase spacecraft, and
- **embassies** that allow players to negotiate government intervention in the planet\'s economic system (via price floors and ceilings).
