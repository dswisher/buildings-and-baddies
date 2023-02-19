# Buildings and Baddies

This is mainly a proof-of-concept to play with a few ideas I have for a game.

It is basically a tower defense game, but with more freedom.
One of the main goals of the POC is to test pathfinding and crowd control.

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


# Art Credits

* Bots - Stephen Challener (Redshrike) - [OpenGameArt](https://opengameart.org/content/roguelike-sprites-redshrike-mods)


# Links

* [Piskel](https://www.piskelapp.com/p/create/sprite) is a handy online sprite editor
* Look at [TexturePacker](https://www.codeandweb.com/texturepacker) for building sprite sheets - [monogame tutorial](https://www.codeandweb.com/texturepacker/tutorials/how-to-create-sprite-sheets-and-animations-with-monogame)

