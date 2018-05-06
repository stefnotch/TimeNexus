# TimeNexus

A platformer where you can scroll through time

## Project Structure

The solution consists of 3 projects

- [ReactiveProperty](https://github.com/runceel/ReactiveProperty) minus the parts that we don't need (GUI framework specific stuff)

- TimeNexus.Game, which is where the codez is

- TimeNexus.Windows launches TimeNexus.Game on Windows. Or something like that

## TimeNexus.Game

This project consists of the following parts:

- Debugging: Mostly empty, it has a DebugDraw.Line function which sorta works

- Effects: Contains all the shaders (e.g. The gun beam uses a fancy shader to look so cool)

- ExtensionMethods: A bunch of [extension methods](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods) 

  - IDisposableExtensions: Extension methods to dispose of IDisposeables when an Entity gets removed.

  - TransformComponentExtensions: Extension methods for `TransformComponent`s

- Gun: The gun beam and the gun controller, also changes the time.

- Keybindings, everyone respects them

- LevelManagement: Should be in a different project, but I can't do that. (Xenko doesn't support it properly)

  Anyways, it manages the different levels (Level with Gateway <--> Level with Gateway)

  - EntityComponentExtension: Used to get the current level

  - GameStudioSomethingBlabla: Game Studio specific magic (displaying the gateway's orientation in the game studio)

  - Gateway: An EntityComponent that is used to connect different levels (both levels need to have a gateway, which then are "connected")

  - Level: A wrapper around a Scene

  - LevelManager: Manages all the currently loaded levels

    Tip: Don't touch it unless you know what you are doing

  - LevelSettings: Every level has some settings
