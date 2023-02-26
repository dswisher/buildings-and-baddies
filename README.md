# Buildings and Baddies

This is mainly a proof-of-concept to play with a few ideas I have for a game.

It is basically a tower defense game, but with more freedom.
One of the main goals of the POC is to test pathfinding and crowd control.

## Current State

Working towards figuring out collision detection and pathing.
Right-clicking the mouse sets the target for all active creatures to the current mouse position.

In the building mode, left-click to build an item.

Keys:

* B - enter building build mode (hit again to cycle between buildings)
* C - remove all creatures from the game
* D - toggle debug mode (draws pathing on screen)
* F - toggle FPS display
* G - enter guard bot building mode
* H - enter hover bot building mode
* P - toggle display of the path grid
* T - enter tread bot building mode
* X - exit the game
* ESC - exit build mode
* SPACE - pause the game

Hover bots are faster than guard bots, and guard bots are faster than tread bots.


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


# TODO List

* Add Pathfinding
  * Add a unit that is smaller than the current ones, add a unit that is larger than the current ones
  * Implement simple A* - when destination specified, units calc path to destination and follows that path, clearance ignored
  * Enhance to use clearance-based pathing
  * If unit is "stuck", recalculate path
  * Multiple units going to same destination should follow-the-leader
* Add some sort of selection mode (select all hover bots, etc)
* Add a "build menu", so one key opens a "dialog" where user can choose what to build
* Refactor `Game1.cs` so it is a little more manageable
* Buildings should snap to grid
* Add baddies
* Add health bars
* Add targeting
* Add projectiles and damage


# Art and Sound Credits

* Bots - Stephen Challener (Redshrike) - [OpenGameArt](https://opengameart.org/content/roguelike-sprites-redshrike-mods)
* Buildings - [Kenney.nl](https://www.kenney.nl/) - [OpenGameArt](https://opengameart.org/content/sci-fi-rts-120-sprites)
* Interface sounds - [p0ss](https://opengameart.org/users/p0ss) - [Interface sounds starter pack](https://opengameart.org/content/interface-sounds-starter-pack)
* Impact sounds - [Iwan 'qubodup' Gabovitch](http://opengameart.org/users/qubodup) - [impact](https://opengameart.org/content/impact)


# Links

* [Piskel](https://www.piskelapp.com/p/create/sprite) is a handy online sprite editor
* Pathfinding
  * Red Blob Games: [Intro to A*](https://www.redblobgames.com/pathfinding/a-star/introduction.html) - [A* Implementation](https://www.redblobgames.com/pathfinding/a-star/implementation.html)
  * [A* Pathfinding for Beginners](https://web.archive.org/web/20100212030833/http://www.policyalmanac.org/games/aStarTutorial.htm) (via Wayback Machine)
  * Dotnet [Priority Queue](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.priorityqueue-2?view=net-6.0) - [Example](https://dotnetcoretutorials.com/2021/03/17/priorityqueue-in-net/)
* Collision libraries
    * [MonoGame.Extended.Collisions](https://www.monogameextended.net/docs/features/collision/collision) - part of [MonoGame.Extended](https://github.com/craftworkgames/MonoGame.Extended)
* Look at [TexturePacker](https://www.codeandweb.com/texturepacker) for building sprite sheets - [monogame tutorial](https://www.codeandweb.com/texturepacker/tutorials/how-to-create-sprite-sheets-and-animations-with-monogame)
* Tutorial - [MonoGame: How to Use Sprite Sheets](https://www.industrian.net/tutorials/using-sprite-sheets/) - simple animation of sprites
* Tutorial - [Monogame - Drawing Text With Spritefonts](http://rbwhitaker.wikidot.com/monogame-drawing-text-with-spritefonts)
* Tutorials - Oyyou - [youtube](https://www.youtube.com/playlist?list=PLV27bZtgVIJqoeHrQq6Mt_S1-Fvq_zzGZ) - [github](https://github.com/Oyyou/MonoGame_Tutorials)
* Library - [Apos.Shapes](https://github.com/Apostolique/Apos.Shapes) - something to consider, to replace DrawLine helper?
* UI libraries to try out someday:
    * [Myra](https://github.com/rds1983/Myra) - UI Library for MonoGame, FNA and Stride
    * [MGUI](https://github.com/Videogamers0/MGUI) - UI framework for MonoGame game engine.
    * [Apos.Gui](https://github.com/Apostolique/Apos.Gui) - UI library for MonoGame - minimalist - inspired by IMGUI
* More Art
    * Terrain - [Kenney.nl](https://www.kenney.nl/) - [OpenGameArt](https://opengameart.org/content/tower-defense-300-tilessprites)
