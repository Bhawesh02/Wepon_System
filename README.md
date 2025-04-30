# Game Interactables & Weapon Systems

This repository contains scripts for managing interactable objects and weapon systems in a Unity-based game. The code includes handling player movement, weapon switching, ammo tracking, and the interaction with pickable objects. 

## Features

- **Player Movement**: Handles player movement and camera rotation.
- **Weapon Switching**: Allows the player to switch between primary and secondary weapons with custom key bindings.
- **Weapon Ammo**: Displays the current ammo in the magazine and extra ammo.
- **Weapon Pickup**: Enables the player to pick up weapons, with the appropriate weapon models displayed.
- **Interactive Feedback**: Provides visual feedback for pickable interactables.

## Code Architecture

The code architecture is designed with modularity and extensibility in mind, following a clear separation of concerns. Here's a breakdown of the main components and their responsibilities:

### 1. **Player Class (`Player.cs`)**

The `Player` class manages the player's core functionalities:

- **Movement and Rotation**: Handles movement input for horizontal (`WASD` or arrow keys) and vertical axes, and handles camera rotation via mouse movement.
- **Weapon Handling**: Manages the player's weapons, including switching between primary and secondary weapons, firing, and reloading. The `WeaponController` class is used to control weapon behavior.
- **Interactables**: The player interacts with pickable objects (like weapons) by raycasting in the game world. When the player hovers over an interactable object and presses the interact key, an event is triggered to equip the weapon.


### 2. **Weapon Handling (`WeaponController.cs`)**

The `WeaponController` class manages the logic for firing, reloading, and switching between weapons. It also tracks the current weapon's ammo count and handles the firing rate, allowing for a smooth gameplay experience.

- **Weapon Switching**: The player can switch between primary and secondary weapons using key bindings, which are customizable through the `InputConfig` class.
- **Firing and Reloading**: The player can fire the weapon and reload using specific keys.

### 3. **Interactables (`PickableInteractable.cs` and `GunPickable.cs`)**

The `PickableInteractable` class provides a base class for all objects that the player can interact with (e.g., weapons). It manages the UI feedback for hovering and selecting objects.

- **Hover Feedback**
When the player hovers over an interactable object, a message is displayed to prompt the player to press the interact key. The message is displayed dynamically, showing the key to press for interaction, which can be customized in the `InputConfig` class.

- **Selection**
When the player selects an interactable object (such as picking up a weapon), the appropriate event is triggered, which typically leads to equipping the item or performing a corresponding action.

### 4. **Input Configuration (`InputConfig.cs`)**

The `InputConfig` class holds all customizable input configurations for player actions such as movement, firing, and switching weapons. This allows for easy modification of control schemes without modifying the core gameplay logic.

- **Movement**
Customizable axis names for horizontal and vertical movement.

- **Weapon Controls**
Customizable keys for firing, reloading, and switching weapons.


### 5. **Events (`GameplayEvents.cs`)**

The `GameplayEvents` class makes extensive use of events to notify different components of changes or actions in the game. This pattern is particularly useful for decoupling various systems and ensuring they can react to game events independently. By using events, the game can maintain modular and flexible components that communicate through a common system.

-**Weapon Pickup**
`GameplayEvents.OnWeaponPickedUp` is triggered when a weapon is picked up by the player. It passes the `WeaponData` object, which contains all the necessary data to equip and use the weapon.

-**Weapon Switch**
`GameplayEvents.OnWeaponSwitched` is triggered when the player switches weapons. This allows other components of the game to react and adjust the gameplay (e.g., updating UI or modifying weapon stats).

-**Interactables**
`GameplayEvents.OnInteractableHover` and `GameplayEvents.OnInteractableSelect` are used to notify systems about player interactions with pickable interactables, such as weapons, power-ups, or objects that can be examined. These events provide the necessary feedback to handle player input and update UI or gameplay accordingly.
### 6. **Weapon Data (`WeaponData.cs`)**

The `WeaponData` class holds all the necessary data for individual weapons, such as weapon type, ammo capacity, damage, and model. This data is used to configure and manage weapon behavior throughout the game.

- **Ammo**
The class tracks the weapon’s ammo capacity (`magazineSize`) and the maximum amount of ammo the player can hold (`maxMagnizeNumberToHold`).
Ammo is crucial for managing the weapon's firing and reloading mechanics.

- **Weapon Type**
The `WeaponData` class supports different weapon types, such as primary and secondary weapons, which are stored in the `WeaponType` enum.
This helps the game differentiate between different classes of weapons, enabling functionalities such as weapon switching.
### 7. **Weapon Equip (`EquippedWeaponData.cs`)**

The `EquippedWeaponData` class is responsible for tracking the current state of the weapon that is equipped by the player. This includes details such as ammo count, the equipped index, and the weapon's type (whether it is a primary or secondary weapon). It is essential for handling weapon switching, ammo tracking, and UI updates related to the weapon's state.

- **Weapon Equip Type**
The class distinguishes between **primary** and **secondary** weapons, allowing the player to manage both types independently.
The `WeaponType` enum is used to define whether the weapon is a primary or secondary weapon.

- **Ammo Tracking**
 The `EquippedWeaponData` class tracks the ammo in the magazine (`currentAmmo`) and any extra ammo the player has in reserve (`extraAmmo`).
This tracking allows the game to handle actions like firing, reloading, and displaying ammo counts.

## Code Design Patterns

The following design patterns are used throughout the codebase to improve flexibility, maintainability, and scalability.

### 1. **Observer Pattern**
The **Observer Pattern** is employed through the event system in `GameplayEvents.cs`. This allows different components of the game to listen for and respond to events in a decoupled manner. For example:
- **Weapon Pickup:** When a weapon is picked up, `GameplayEvents.OnWeaponPickedUp` is triggered.
- **Weapon Switch:** `GameplayEvents.OnWeaponSwitched` is triggered when the player switches between primary and secondary weapons.
- **Interactables:** Events like `GameplayEvents.OnInteractableHover` and `GameplayEvents.OnInteractableSelect` notify other systems when the player interacts with pickable objects.

The event-driven architecture ensures that different systems are independent and can react to game changes without tight coupling between components.

### 2. **Factory Pattern**
The **Factory Pattern** is used in the creation and configuration of weapons. Both the `WeaponConfig` and `WeaponData` classes leverage the factory pattern to manage weapon creation. 
- This ensures that each weapon is instantiated with the correct attributes (e.g., ammo capacity, damage, weapon type) without directly depending on specific implementations.
- A **Weapon Factory** would typically handle the logic of creating and initializing weapons, which is highly extensible and helps in managing complex setups for various weapon types.

### 3. **Strategy Pattern**
The **Strategy Pattern** is used in weapon switching, allowing the equipped weapon to be dynamically swapped based on player input. This provides the following advantages:
- **Flexibility:** The player can switch between primary and secondary weapons without modifying core logic.
- **Extensibility:** The weapon switching mechanism can be easily extended to support additional weapon types without modifying existing systems. This can be achieved by adding new strategies for weapon switching and weapon behaviors.

The strategy pattern in weapon switching allows the game to respond dynamically to player actions, making the system more adaptable and easier to scale.

## Working video

[![]([https://github.com/Bhawesh02/Mesh-Deformation/blob/master/Mesh%20Deform%20Test/Assets/Extra/Mesh%20Deformer.gif])((https://www.youtube.com/watch?v=jnKlDJ_W1Xw))

## Contributions

This tool represents my individual development efforts but thrives on community engagement. Feedback, suggestions, and collaborative contributions are highly encouraged. 

## Contact

You can connect with me on LinkedIn: [Bhawesh Agarwal](https://www.linkedin.com/in/bhawesh-agarwal-70b98b113). Feel free to reach out if you're interested in discussing the game's mechanics, and development process, or if you simply want to talk about game design and development.

---
