# Boy Scout Simulator
## Credits
- Austin: Wolf AI/pathfinding, lighting
- Brandon: Orienteering (map, compass), core gameplay
- Jared: Terrain and map design, scoutmaster voiceover/scripting
- Hyuk-Je: Sounds, UI, game logic, sketches

## Scripts Worked On
### Brandon Halig
- GameInteractions.cs 100%
    - Keeps track of player's orienteering objectives, plays radio commands, updates the player's notes list
- ArrowDirection.cs 100%
    - Points the compass in the North direction
- FirstPersonAudio.cs 10%
    - Added functionality for pacing footsteps

### Hyuk-Je Kwon
- UI_Management.cs 90%

### Austin Bushey
- CheckpointManager.cs 100% - Connected checkpoints to the UI
- Candy.cs 100% - Throws food on the ground to distract the wolves
- SlopeRotation.cs 100% - Matches the wolf's rotation to the slope of the ground
- WolfMove.cs 100% - AI For the wolf
- BearMove.cs 100% - AI For the grizzly bear
- BlackBearMove.cs 100% - AI For the black bear
- CubMove.cs 100% - AI For the cub's
- MamaBearMove.cs 100% - AI For the Mama bear's
- BearSlope.cs 100% - Matches the bears's rotation to the slope of the ground

## Game AI
- General idea is that if the player is within stalking distance or there is an unobstructed line of sight then the wolf or grizzly/black bear will walk towards the player at 5.0f speed.
- If the wolf or grizzly/black bear is within hearing distance of the player then it will then run towards the player at 15.0f speed.
- Otherwise they stay still and howl or roar. The wolf can be distracted while they're running at you by throwing meat on the ground.
- The grizzly bear will ignore you or run away if you stay still for at least a second while it's tracking you.
- The black bear can be scared away if you yell at it.
- The mama bear and cub act different than the other types of bears. The cub will wander around randomly and the mama will follow it around and protect it.
- If the players walks within the protecting distance of the cub, the mama will attack until the player leaves.
- Additionally, all the animals have a rotation script to rotate their mesh along the slope of the ground to make it look realistic.
- All movement paths are calculated using a navmesh.

## Sound Effects
- Player
    - Walking/Running:
    - Throwing sound ([whoosh](https://www.youtube.com/watch?v=woxWw37zRVc))
- Wolf
    - [Howl](https://www.youtube.com/watch?v=jJYwipRvS5Y) (Proximity)
    - [Growling](https://www.youtube.com/watch?v=E7Iia8DUxrc) (Attack noise)
    - [Whimper](https://www.youtube.com/watch?v=LUql_PGq3is) (When chasing after candy)
- Bear
    - [Roar](https://www.youtube.com/watch?v=FAglo3Ohpes) (Proximity)
    - [Grunt](https://www.youtube.com/watch?v=EL9AtDgfzNc) (When you stand still for long enough)
- UI
    - [Click](https://www.youtube.com/watch?v=vzfqwCu2hi4)
    - Scroll when moving sliders (just click but repeating when the value changes)

## MAP - Jared Starman
### MAP ORGANIZATION
The Map is divided into 4 sections
#### Tile 1/Tutorial
This section is where the player starts. They learn the basics of orineteering, with lightposts teaching them how to do it properly

#### Tile 2 - Forest
This section is for the players to do the orienteering between three objectives. They interact with radios to update their instructions

#### Tile 3 - Airport
In this section, players must press each of the 4 objectives (the trucks in the hangars and the plane). They also learn how to deal with the three times of animals

#### Tile 4 - Mountain
This is the final section, where players need to do both at the same time

### Unity Assets Cited (all modified with collision assets)
- [MILITARY VEHICLE](https://assetstore.unity.com/packages/3d/vehicles/land/military-vehicle-9225)
- [GAZ 66](https://assetstore.unity.com/publishers/2837)
- [Military Truck PBR](https://assetstore.unity.com/packages/3d/vehicles/land/military-truck-pbr-41450)
- [M3A1 Scout Car](https://assetstore.unity.com/packages/3d/vehicles/land/m3a1-scout-car-53149)
- [Old Hangar](https://assetstore.unity.com/packages/3d/environments/historic/old-hangar-119037)
- [Super Spitfire](https://assetstore.unity.com/packages/3d/vehicles/air/super-spitfire-53217)
- [Crashed Boeing c-17](https://assetstore.unity.com/packages/3d/environments/industrial/crashed-boeing-c-17-globemaster-iii-133633)
- [Small Town America](https://assetstore.unity.com/packages/3d/small-town-america-streets-free-59759)
- [RPG/FPS Game Assets for PC/Mobile (Industrial Set v2.0)](https://assetstore.unity.com/packages/3d/environments/industrial/rpg-fps-game-assets-for-pc-mobile-industrial-set-v2-0-86679)
- [Swamp Bridge](https://assetstore.unity.com/packages/3d/props/exterior/swamp-bridge-71515)
- [Towers PBR Pack](https://assetstore.unity.com/packages/3d/environments/towers-pbr-pack-95705)
- [Tents](https://assetstore.unity.com/packages/3d/environments/towers-pbr-pack-95705)
- [Secret Radio Room - The Swamp House](https://assetstore.unity.com/packages/3d/environments/secret-radio-room-the-swamp-house-155339)
- [Realistic Tree Pack Vol.1](https://assetstore.unity.com/packages/3d/vegetation/trees/realistic-tree-pack-vol-1-50418)
- [Terrain Asset Sample Pack](https://assetstore.unity.com/packages/3d/environments/landscapes/terrain-sample-asset-pack-145808)
- [Mini First Person Controller](https://assetstore.unity.com/packages/tools/input-management/mini-first-person-controller-174710)
- [Wolf Asset: Only the mesh and animations](https://assetstore.unity.com/packages/3d/characters/animals/wolf-animated-45505)
- [Bear Asset: Only animations](https://assetstore.unity.com/packages/3d/characters/animals/free-stylized-bear-rpg-forest-animal-228910)
- [RPG Food pack:](https://assetstore.unity.com/packages/3d/props/food/rpg-food-drinks-pack-121067)
- Consumer.cs - From asset store
- Rotator.cs - From asset store

## ALL VOICE ACTING - JARED STARMAN

