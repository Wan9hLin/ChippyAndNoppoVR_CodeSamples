# PuzzleStations System

Focus:
Handles all station-based puzzle mechanics that modify or influence block states during gameplay.  
Includes systems for block dyeing, splitting, and environmental barriers, ensuring synchronized visual and gameplay effects across all players.

Key Scripts:
- DyeZone_Photon.cs – Changes block materials when entering dye zones; applies synchronized updates to maintain consistent appearances across clients.  
- SplitterGate_Photon.cs – Manages block splitting logic as blocks pass through the split gate; synchronizes block replacement, material updates, and UI states.  
- BarrierManager.cs – Controls visibility of barriers based on player count; removes barriers once multiple players join and synchronizes state changes via RPC.
