# Migration Plan: Xenko → Flax

The goal is to migrate the game in it's current form from Xenko to Flax. This plan outlines the current status and what is to be expected in the future.

**Explanation of the huge discrepancy between Xenko and Flax development speed:**

- The Flax Editor starts in < 8 seconds

  The Xenko Editor takes at least ~27 seconds. The startup time of the Xenko editor will become worse in the future (when working on level one), because Xenko scans every single asset before starting. This is actually quite important, since both editors crash every once in a while.

- Starting a game using Flax takes <3 second. Super awesome for rapid prototyping.

  Starting a game using Xenko takes ~25 seconds. 

- The Flax editor can actually be used while playing the game! (Awesome for debugging and testing) This allows you to inspect every object in the game, change the state of the objects, add/remove objects and look around the world (with a fly-camera).

  ![Flax Editor](./Flax%20Editor.png)

  This is not possible in Xenko. In Xenko, you cannot use the editor while playing.

  

- The Flax developer(s) and community is quite active and will respond very quickly when you run into an issue.

  Xenko on the other hand is mostly dead. (There is a chance of the company continuing the project.)

- Neither of them have a very complete documentation, however, since the Flax community is quite active, this is only an issue with Xenko.

- Effects with Flax take far less time to create. Flax has a simple, but powerful visual programming tool with realtime previewing.

  Xenko invented their own shading language, which is rather powerful but takes painfully long to compile. It's also badly documented.

- The Flax editor is very open to modification.

  Xenko's Editor is closed source.

- Stefan prefers Flax over Xenko, which leads to him working more with Flax than he would work with Xenko. (In his free time)

# Current Status

### Flax:

- Player

  - Camera, including a special effect when the player is near an edge

  - Tachyonen-Insignifikator (Gun, used to change the time)

- Pausing (no menu yet)

- Interacting with objects

  - GUI pops up

  - Player can press the "interact key" to interact

- **Regarding level loading, this won't be an issue with Flax since Flax supports it really well out of the box**

- **Menus are working, however they are unfinished**

- **1st level (took at least 8 hours to create, using Xenko, it would take far longer)**

  - **A large cave with a chasm and a bridge**

  - **Rocks blocking the way --> Gun must be used to remove them**

  - **Interactive door**

  - **Better looking effects/graphics**

### Xenko:

- Player

  - Movement

  - FPS-Camera, including a special effect when the player is near an edge

  - Tachyonen-Insignifikator (Gun, used to change the time)

- Pausing (no menu yet)

- Interacting with objects

  - GUI pops up

  - Player can press the "interact key" to interact

- Level loading (Note: No level has been finished so far)

  - Trigger areas which cause the next level to get loaded

  - Nice game studio integration

- Started work on a the menus (main menu, pause menu)

# This week

## Goals:

- Remote Desktop or some other solution for Arno

- Arno being familiar with Flax

- Stefan working on the game, however, his main focus will be getting Arno up to date.

## Today:

- Getting Flax: Done

  - Engine & Editor: Done

  - Joining the Flax community: Done

- Arno running Flax at home: Done

- Remote Desktop Test (Arno): Done, successful

## Tomorrow:

- Testing the remote desktop stuff at school (2nd period): Done

  This will require the following equipment:

  - Lan Cable: Arno will bring one

  - USB Type C <--> LAN: Stefan will bring one

- Cloning the Flax Time Nexus repository and opening it (opening the Flax implementation of the game)

## Thursday:

- Arno familiarizes himself with Flax

## Friday:

- Resolving potential issues (remote desktop): Done, not needed
- Setting a goal for the weekend (who should achieve what by Tuesday): Done
- Writing a tiny status report and sending it to Prof. Bauer: Done

# 5th June - expected status

## Flax

- Arno should bring a LAN cable

- Stefan should bring a USB Type C to LAN adapter

- Arno should be able to work on the project

- The main menu and pause menu should be implemented:

  - Main menu: `Play` and `Exit` buttons: Done

  - Pause menu: `Resume/Unpause`, `Exit` buttons. Placeholders for other buttons such as `Save/Load`, `Settings`: Done

## Xenko

Pause menu with the 2 buttons. (`Resume/Unpause`, `Exit`) . A main menu with 2 buttons `Play` and `Exit`. I'm really not expecting anything better than that, since using Xenko, quite a bunch of the menu stuff has to be implemented manually.

# 12th June - expected status

## Both

- Deadly projectiles (trap): Mostly done

- Checkpoints (Not to be confused with actually saving anything to the disk): Mostly done

- **Health bar: TODO**

- **The basics of level 2's environment: Started**

## Flax

- Working on level 2 using the features above. Since they won't take as long to implement in Flax, a few other features can be implemented as well:

- An altar where the player can pick up the time gun will be added to level 1: Done

- **A better checkpoints menu (with buttons for each checkpoint): Maybe at the end, a settings menu is a higher priority**

- Far better time scrolling transitions: Done

## Xenko

- Starting to work on level one
  - Importing 3D models
  - Creating the level geometry
  - Painstakingly adding and modifying physics colliders (major issue in Xenko)

# 19th June - expected status

## Flax

- Finishing level 2

- Working on level 3

- Tutorial (text)

- Sounds

## Xenko

- Finishing level 1

- Working on level 2

- Fixing/working around bugs - will take a while

# 26th June - expected status

## Flax

- At least 4 levels with all the things listed above

## Xenko

- 2 low quality levels
