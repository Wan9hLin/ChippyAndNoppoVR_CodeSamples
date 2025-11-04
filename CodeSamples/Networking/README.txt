# Networking System

Focus:
Handles all multiplayer networking logic for the VR collaborative environment, including authentication, room lifecycle, player spawning, and real-time synchronization across clients.  
Ensures stable and consistent multiplayer sessions through Photon Network integration and smooth avatar updates.

Key Scripts:
- LoginManager.cs – Manages player authentication and connection to Photon servers; supports both anonymous and named login and transitions to the main scene upon successful connection.  
- RoomManager.cs – Oversees room creation, joining, and matchmaking using Photon; dynamically updates room occupancy and manages seamless scene transitions.  
- SpawnManager.cs – Controls avatar instantiation at designated spawn points; ensures synchronization and prevents overlapping during player joins.  
- PlayerNetworkSetup.cs – Configures local and remote player settings, avatar initialization, and teleportation; supports dynamic avatar customization.  
- MultiplayerVRSynchronization.cs – Synchronizes player and avatar transforms (position/rotation) across clients with interpolation for smooth, low-latency movement.
