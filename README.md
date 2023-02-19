# Buildings and Baddies

This is mainly a proof-of-concept to play with a few ideas I have for a game.

It is basically a tower defense game, but with more freedom.
One of the main goals of the POC is to test pathfinding and crowd control.

## Current State

Working towards figuring out collision detection and pathing.
Clicking the mouse sets the target for all active creatures to the current mouse position.

Keys:

* C - remove all creatures from the game
* D - toggle debug mode (draws pathing on screen)
* G - create a guard bot at the next mouse click
* H - create a hover bot at the next mouse click
* T - create a guard bot at the next mouse click
* ESC - exit build mode
* X - exit the game

The only difference between the three is their image.


## Goal State

Baddies randomly spawn at the edge of the screen.
They pick a target, and go attack it.
The target could be a villager or building.
If there are no targets, they wander in random directions.

Certain keys initiate build mode, to create buildings and obstacles.
(In a "real" game, this would be nice menu.)

* H - build a hut, with N villagers. Provides shelter for the villagers.
* F - build a farm, where villagers can go work.
* G - build a gun turret, that will shoot at baddies using bullets.
* L - build a laser turret, that will shoot at baddies using lasers.

Other keys initiate behaviors from the villagers and/or baddies.

* W - villagers head to the nearest farm to work.
* S - villagers head to the nearest hut for shelter.
* V - spawn a villager at the current mouse position.
* B - spawn a baddie at the current mouse position.
* T - all baddies switch their target to the item under the cursor.


# Art and Sound Credits

* Bots - Stephen Challener (Redshrike) - [OpenGameArt](https://opengameart.org/content/roguelike-sprites-redshrike-mods)
* Interface sounds - [p0ss](https://opengameart.org/users/p0ss) - [Interface sounds starter pack](https://opengameart.org/content/interface-sounds-starter-pack)


# Links

* [Piskel](https://www.piskelapp.com/p/create/sprite) is a handy online sprite editor
* Look at [TexturePacker](https://www.codeandweb.com/texturepacker) for building sprite sheets - [monogame tutorial](https://www.codeandweb.com/texturepacker/tutorials/how-to-create-sprite-sheets-and-animations-with-monogame)
* Tutorial - [MonoGame: How to Use Sprite Sheets](https://www.industrian.net/tutorials/using-sprite-sheets/) - simple animation of sprites
* Tutorial - [Monogame - Drawing Text With Spritefonts](http://rbwhitaker.wikidot.com/monogame-drawing-text-with-spritefonts)
* Library - [Apos.Shapes](https://github.com/Apostolique/Apos.Shapes) - something to consider, to replace DrawLine helper?

