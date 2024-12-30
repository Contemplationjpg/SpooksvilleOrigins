Spooksville Origins is a roguelite, turn-based rpg based on the not-so-fleshed-out game Spooksville.

The original Spooksville included elements of rng in resource collection to fight in a turn-based battle where the player's health is candy and source of damage is candy. So what better idea than to use that as a premise for a rogue-lite game (and )


Features:
- Sugar System: Sugar = Weapon Durability = Meter = Damage = Health(?)
- Weapons, Perks, Partners, (eventually) Characters add fun variety to gameplay feel  
- Story Pieces collectable through gameplay

Notes:
- Doing damage should feel good
- Reward proper resource management while not being too harsh on durability
- Make choices feel meaningful

Progress Summary: 
Save system that stores inventory as IDs for weapons and items into json file.  
Item Systems that use ItemIDs and similar systems for entities, encounters, and weapons created.
Working on turn-based battle system...
    - so far its bare-bones with only a simple hop animation for attacking and enemy AI that only attacks
    - currently using damage calculation formula
    - limitation of system now is that the number of enemy slots for the battlefield is modular to work with any amount but amount has to be set by the developer
    - able to queue encounters 
    - player has multiple actions per turn 
    - after picking a weapon, player can toggle for normal or special attack
    - player has option to eat weapon

To-Do: (* meaning big item)
*- sugar/duability system
- battle UI
- animation for dealing damage (probably add screen-shake dependent on raw damage number or percent of damage done to enemy)
- items that do things (ex. heal, increase sugar, increase stat)
- stat modifier system
*- perk system 
- enemy AI that does things other than attacking (ex. spawn other enemies based on condition, heal self/allies, multiple types of attacks, explode)
    - probably make enemy AI part of EntityDatabase as new type "NPCAI" with corresponding number as the entity in EntityDatabase
    - ideas for different AI functions: regular(thing(s) done by default like attacking or status), last stand (does something when below certain percent of health), revenge (does thing after ally dies)
- healthbar colors to indicate when things like last stand go off (kinda like being in yellow for torrent/overgrow/blaze boost in pokemon)
- textbox system

- think of potentially more efficient search system for item/entity/encounter (probably put entity ID on entity type itself for ease of searching for corresponding AI)