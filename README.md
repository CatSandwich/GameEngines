# GameEngines PLAR Portfolio

This readme file is the entire PLAR portfolio submission for Game Engines 2 - Advanced Scripting.

## Overview

Below are all of the course learning outcomes (CLOs) with the names I will be referring to them by.

**1. Explain concepts:** Explain the concepts of generics, inheritance, polymorphism, interfaces, extension methods, co-routines and delegates for the purpose of applying the advanced features of C# programming.  
**2. Use concepts:** Utilize the advanced C# programming constructs to efficiently code professional games within the Unity game engine.   
**3. Use managers:** Enhance code design using a game manager and a scene manager in order to create professional games.  
**4. Use source control:** Correctly implement source control software to manage versions of game code and effectively share their work with team members.  
**5. Collaborate:** As part of a team, apply collaborative skills to build small games according to industry standards.  

## Explain concepts

*The student should demonstrate each individual item noted with a code sample and documentation 
accompanied which explains each item. I would expect at least one paragraph for each component 
of why the decision was made to use this architectural component and why it was appropriate for 
the chosen use in a game.*

Below are links to one-script projects demonstrating (with comments) all of the concepts. I included 
them in the order that I believe they make most sense rather than the order they appear in the CLO.

- [Inheritance](https://github.com/CatSandwich/GameEngines/blob/master/Theory/Inheritance/Program.cs)  
- [Interfaces](https://github.com/CatSandwich/GameEngines/blob/master/Theory/Interfaces/Program.cs)
- [Polymorphism](https://github.com/CatSandwich/GameEngines/blob/master/Theory/Polymorphism/Program.cs)  
- [Generics](https://github.com/CatSandwich/GameEngines/blob/master/Theory/Generics/Program.cs)  
- [Extension Methods](https://github.com/CatSandwich/GameEngines/blob/master/Theory/ExtensionMethods/Program.cs)  
- [Delegates](https://github.com/CatSandwich/GameEngines/blob/master/Theory/Delegates/Program.cs)  
- [Co-routines](https://github.com/CatSandwich/GameEngines/blob/master/UnityTheory/Assets/Coroutines.cs)  

## Use concepts

*This should tie into CLO1, essentially these above architectural constructs / patterns should be 
demonstrated in games made using the Unity game engine specifically.*

### Inheritance

Often in games, I use inheritance to allow for shared code between GameObjects. There are some cases where shared code simply doesn't work as a separate component, so it needs to be built into multiple components through inheritance. Below are some examples:

- [Interactable objects shared code](https://github.com/CatSandwich/Metroidvania/blob/master/Assets/Scripts/Entity/PlayerInteractable.cs#L8)
  - [Implementation](https://github.com/CatSandwich/Metroidvania/blob/60b0e490dd9cfa493059d841914417afe0eaf4e3/Assets/Scripts/Entity/Portals/PortalController.cs#L29) - Portal that can be activated while close enough
- [Hitbox Receiver](https://github.com/CatSandwich/Metroidvania/blob/master/Assets/Scripts/Entity/Hitbox/HitboxReceiver.cs) / [Hitbox Sender](https://github.com/CatSandwich/Metroidvania/blob/master/Assets/Scripts/Entity/Hitbox/HitboxSender.cs) for 2D combat system.
  - [Implementation](https://github.com/CatSandwich/Metroidvania/blob/60b0e490dd9cfa493059d841914417afe0eaf4e3/Assets/Scripts/Entity/Player/PlayerSenderSword.cs#L7) - Player's sword hitbox
  - [Implementation](https://github.com/CatSandwich/Metroidvania/blob/60b0e490dd9cfa493059d841914417afe0eaf4e3/Assets/Scripts/Entity/Enemy/EnemyReceiver.cs#L5) - Enemy's receiving hitbox

While most of these could have been separated into separate components with `UnityEvent`s to expose Hit or Interact events, I didn't know about `UnityEvent`s at the time, so I achieved the same functionality with inheritance and overriding.

### Interfaces

I use interfaces to allow for polymorphism in my code. If there are multiple different objects that need similar functionality, often I'll use interfaces. In addition to this, I like to use interfaces when working in a team to have code contracts that multiple developers need to work with. This helps each person know what the others expect.

- [IPlayerReactive](https://github.com/CatSandwich/Squares/blob/master/Assets/Scripts/IPlayerReactive.cs) - interface for objects that react to the player coming close enough.
  - [Follower](https://github.com/CatSandwich/Squares/blob/master/Assets/Scripts/Entities/FollowerEntity.cs)
  - [Scared](https://github.com/CatSandwich/Squares/blob/master/Assets/Scripts/Entities/ScaredEntity.cs)
- [IAttack](https://github.com/CatSandwich/PewPew/blob/master/Assets/Scripts/Attack/Interfaces/IAttack.cs)
  - This interface was created as a form of communication between my code and one of my partners'. I was writing the player code, including attacks, and he was writing the enemy code, including receiving damage. This interface served as a contract of what functionality was required on my end for his end to support it.
  - [Implementation](https://github.com/CatSandwich/PewPew/blob/f4dd5dea4c70725f38edfa5ae93e0618c162b151/Assets/Scripts/Attack/AttackBase.cs#L10)
    - As above, I probably should have avoided inheritance here and separated functionality into separate components, but it worked all the same. Because all of my attacks derived from a class which implements the interface, all of them were able to interact with my partner's code.

### Polymorphism

I mentioned above that I use interfaces and inheritance a lot for shared code. The way this shared code gets used is by having all derived types of the interface or base class downcasted to the shared type. This allows them all to be used together with their shared code.

- [HitboxSender polymorphism](https://github.com/CatSandwich/Metroidvania/blob/master/Assets/Scripts/Entity/Hitbox/HitboxSender.cs#L51) - Here the sender checks for a `HitboxReceiver` component on the triggered object. This uses `GetComponent` in a polymorphic way: it looks for any component type that is or derives from HitboxReceiver. This downcasts the actual type to the base type that has the shared functionality.
- [IPlayerReactive useage](https://github.com/CatSandwich/Squares/blob/master/Assets/Scripts/PlayerController.cs#L25) - finds all components derived from IPlayerReactive then stores them as their shared, downcasted interface. They can then be [used together](https://github.com/CatSandwich/Squares/blob/master/Assets/Scripts/PlayerController.cs#L43) despite being different types.

### Generics

I use generics often when writing systems, as generics often improve modularity - they reduce limitations by allowing things to be used with multiple types. I also use them for what I call "wrapper types" - types that add simple functionality to a pre-existing type, in a similar way to Nullable<T>.

- [Upgrades](https://github.com/CatSandwich/PewPew/blob/master/Assets/Scripts/Player/Upgrades/Upgrade.cs#L9) - definition for player upgrades where the value upgraded is generic.
  - Allows for scalable and modular upgrade management within the codebase, with no downsides such as losing type safety or efficiency that may come with the alternative of boxing/unboxing.
- [Networking](https://github.com/CatSandwich/Game-of-Life/blob/master/Unity/Assets/Scripts/Network/ClientExtensions.cs#L11) - listens for a message with a given id, then deserializes the data according to the type parameter.
- [CalculatedValue<T>](https://github.com/CatSandwich/CalculatedValue/blob/master/CalculatedValue.cs) - wrapper type. This type wraps another type and lets the programmer create a recipe to calculate a value through a chain of delegates. As the type doesn't require any functionality from the wrapped type, other than 'can be modified by code', I made it generic to remove any limitations there.

### Extension methods

I often use extension methods when there's functionality I need from an external library that isn't provided.

- [Networking](https://github.com/CatSandwich/Game-of-Life/blob/master/Unity/Assets/Scripts/Network/ClientExtensions.cs#L11) - same link as above, but it fits this section too. The library used is event-based, but I was looking for the functionality to wait for a message. This extension converts the waiting functionality into event-based. 
  - Because I'm using the library's class, `UnityClient`, I couldn't modify it - I had to extend it through an extension method.
- [Rect splitting](https://github.com/CatSandwich/UnityHelpers/blob/main/Editor/EditorUtils.cs#L26) - As I'm often working with custom editors, I wanted a quick way to split `Rect`s into grids for many reasons. Unity didn't provide a quick way to do this, so I extended `Rect` to allow this functionality.

### Delegates

I use delegates everywhere. I like event-based programming, which has delegates at its core. Through event-based programming and delegates, I have achieved functionality such as auto-updating UI, reactive AI, and incremental gameplay calculation. I also use them for modularity (passing code as a variable has more freedom than passing data).

- [CalculatedValue<T>](https://github.com/CatSandwich/CalculatedValue/blob/master/CalculatedValue.cs) - uses delegates to control the calculation of the value.
  - One way to look at delegates is passing functions or code as variables. This class gives more finite control over the calculation of a value as it takes code in the calculation rather than just values. This allows for a delegate that changes the value based on a random, or other similar non-static bonuses.
- [StatChanged](https://github.com/CatSandwich/Idle/blob/master/Assets/Scripts/Stats.cs#L14) - event for when a stat is changed. This event was mainly used to notify UI elements that they need to update.
  - [Usage](https://github.com/CatSandwich/Idle/blob/master/Assets/Scripts/UI/GenericLabel.cs#L15) - label subscribes its local `_updateText()` method to react to stat changes.

### Co-routines

Co-routines are the go-to for any problem in Unity that involves running code over multiple frames. I often use these for code surrounding animations. I've used it before for phased boss fights (wait until X% health to run some function).
  
- [Death animation](https://github.com/CatSandwich/Metroidvania/blob/master/Assets/Scripts/Entity/Enemy/EnemyController.cs#L76) - triggers the animator's death animation, waits until it's over, then destroys self. Requires waiting for the animation, therefore a co-routine excels.
- [Phased boss fights](https://github.com/BeyondTheEcho/Solaris_Defensive/blob/Alpha_Prep_Branch/Assets/Scripts/Boss/Boss.cs#L23) - this co-routine runs over the lifetime of the boss and checks for health thresholds to be reached. Once they are, the threshold runs the corresponding delegate.
  - [Implementation](https://github.com/BeyondTheEcho/Solaris_Defensive/blob/Alpha_Prep_Branch/Assets/Scripts/Boss/The%20Construct/TheConstruct.cs#L16) - boss spawns a number of enemies at each threshold.
  
## Use managers

*These should feature singleton game managers with functions, variables and states applicable 
to both managers listed. Game Manager should be detailed for the specific game made therefor 
individual game managers.*
  
I use a game manager/controller in nearly all of my projects. It's great to have one source of truth that enables communication between sub-systems. I use scene controllers in some situations (not all games need much scene management).
  
- [PewPew](https://github.com/CatSandwich/PewPew/blob/master/Assets/Scripts/Singletons/GameManager.cs) - this game manager stores all of the references to upgrades as well as the currency. This data is used across the entire game so it makes sense to have it accessible in the singleton manager.
- [Game of Life asset controller](https://github.com/CatSandwich/Game-of-Life/blob/master/Unity/Assets/Scripts/AssetController.cs) - similar to the above, it's one source of truth to store references to assets.
- [Solaris scene manager](https://github.com/BeyondTheEcho/Solaris_Defensive/blob/master/Assets/Scripts/SceneManager.cs) - allows for simpler loading of scenes, as well as [data transfer between scenes](https://github.com/BeyondTheEcho/Solaris_Defensive/blob/master/Assets/Scripts/SceneManager.cs#L15-L19) and [callbacks for when the selected scene is over](https://github.com/BeyondTheEcho/Solaris_Defensive/blob/master/Assets/Scripts/SceneManager.cs#L68).
  
## Use source control

*This should demonstrate a GitHub repository online with branch based merged into a main or master 
branch. This should illustrate a history of pull requests, code reviews and merges with the 
master/main in a group dynamic with multiple users editing the same code base with overlapping 
work in the same code files. Students should document files which multiple (at least 2-3) 
programmers have edited the same file.*

In any group project I've ever worked on, I always used Git (GitHub or GitLab hosted). Using branches to isolate each person's (or feature's) code is incredibly important. An unstable/untested feature shouldn't cause issues for other people working on the project, so said feature is left on a branch until it's suitable for the master branch.
  
In a lot of cases, work done on one branch conflicts with work on another. I've gotten better at 2 skills over the years to combat this:
- Avoiding conflicts preemptively
  - Communicate with team members to avoid working on the same files where possible, if not, stay in isolated parts of the file
  - Proper separation of the codebase
    - The more files it's split into, the less likely a conflict will be
- Resolving conflicts efficiently
  - Communication with team members to merge conflicting code into one system that suits both needs
  - Proper usage of merge conflict tools
    - I use [rider's conflict resolution tool](https://www.jetbrains.com/help/rider/Resolving_Conflicts.html#vcs-resolve-conflicts)
  
Some examples of source control usage:
  
- [Solaris-Defensive](https://github.com/BeyondTheEcho/Solaris_Defensive): This was a group project with 6 contributors. With this many contributors, branching is necessary. We all used our own branch, occasionally more branches depending on what features we were working on.
  - [PlayerController](https://github.com/BeyondTheEcho/Solaris_Defensive/commits/master/Assets/Scripts/Player.cs): This is a rather central file that 4 people modified over the project's lifetime. Through communication and merge conflict resolution, we avoided wasting time resolving preventable issues.
- [PewPew](https://github.com/CatSandwich/PewPew): A project I worked on with two buddies. Our goal was to make a finished product in a week. With such a heavy time constraint, we were often working at the same time. Through effective communication and proper merge conflict resolution, we avoided wasting time.
  - [GameManager](https://github.com/CatSandwich/PewPew/commits/master/Assets/Scripts/Singletons/GameManager.cs): Game managers are central and interact with most systems. Due to this, we all touched it at some point.
  - [Assets](https://github.com/CatSandwich/PewPew/blob/master/Assets/Scripts/Singletons/Assets.cs): Similar to above - manages assets which are used everywhere.
  - [EnemyScript](https://github.com/CatSandwich/PewPew/blob/master/Assets/Scripts/Enemy/AbstractEnemyScript.cs): Was worked on by 2, if not all 3 of us. Doesn't show due to renaming from 'EnemyScript'.
    - This one was often worked on at the same time, so I resolved conflicts through rider's merge conflict resolution: \[[1](https://cdn.discordapp.com/attachments/878881756491694081/880725885966225438/unknown.png)\] - \[[2](https://cdn.discordapp.com/attachments/878881756491694081/880726723866554379/unknown.png)\]
  
## Collaborate

*This would be demonstrated using the same criteria as CLO4 however this adds the requirement 
for it to be multiple games. Ideally the student should have 3+ games on their public GitHub 
portfolio to showcase.*
