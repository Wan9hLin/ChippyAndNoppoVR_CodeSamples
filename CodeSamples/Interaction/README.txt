# Interaction System

Focus: 
Manages all interactive behaviors between players and objects within the VR environment.  
Covers object grabbing, rotation, spawning, and automatic block combination, ensuring consistent states and ownership synchronization across all networked clients.

Key Scripts:
- NetworkedGrabbing.cs – Enables players to grab and release objects; handles Photon ownership transfer and synchronizes object states across clients.  
- BlockRotator.cs – Controls block rotation within a designated zone along Y/Z axes; synchronizes rotation data to maintain visual consistency.  
- BlockSpawner.cs – Spawns block prefabs at predefined positions based on player selection; ensures all spawned instances are network-synchronized.  
- InitialBlockCombiner_Photon.cs – Automatically detects and connects adjacent blocks when players join; uses RPC calls to synchronize attachments and maintain consistent block structures.
