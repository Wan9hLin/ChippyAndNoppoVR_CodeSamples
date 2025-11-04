# Chippy & Noppo VR – Code Samples

## Overview
This repository contains selected code samples from “Chippy & Noppo VR,” a two-player collaborative VR puzzle game developed in Unity using Photon Unity Networking (PUN) and the XR Interaction Toolkit.  
The provided scripts highlight key systems that support seamless multiplayer collaboration, including real-time synchronization, block bonding, rotation, dyeing, and split-gate interactions.


## Technical Highlights
- Multiplayer Architecture – Networking, interaction, and puzzle systems are separated for clarity and scalability.  
- Real-Time Synchronization – All player actions (movement, grabbing, rotation, coloring, splitting) are propagated via Photon RPCs with interpolation for smooth results.  
- Physics-Based Interaction – Uses Unity XR Interaction Toolkit and tuned Rigidbody constraints to ensure natural, stable manipulation.  
- Collaborative Puzzle Logic – Players must coordinate to modify and assemble blocks that dynamically bond, split, and recolor in shared space.  
- VR Optimization – Frame-rate stability and comfort improved through collision tuning, reduced motion discomfort, and texture optimization for standalone headsets.


## How to Use
This repository contains non-runnable code samples extracted from a full Unity VR project.  
They are intended for technical review to demonstrate architecture, code quality, and system design.  
Each script is documented with comments describing its function and integration points within the game.


## System Architecture
The system is organized into four core modules:
- Networking/ – Handles login, room management, spawning, and player synchronization.  
- Interaction/ – Manages grabbing, rotation, and object instantiation.  
- PuzzleBlocks/ – Controls alignment, bonding, and physics-based connections.  
- PuzzleStations/ – Defines functional puzzle stations such as splitters, dye zones, and barriers.
These modules communicate through Photon RPC events and shared object ownership to maintain consistent world states across clients.


## Challenges & Solutions

- Issue: Adapting original “Chippy & Noppo” mechanics to VR caused imbalance and player discomfort.  
  Solution: Adjusted player height, speed, and collider radius; removed jumping and handle rotation to improve comfort and stability.

- Issue: Grabbing and collision interactions were unstable due to inconsistent physics responses.  
  Solution: Tuned Rigidbody and XR Grab Interactable settings (friction, constraints, movement type) for smoother manipulation.

- Issue: Bonded blocks failed to initialize correctly when two players joined simultaneously.  
  Solution: Moved bonding logic from `Start()` to `OnPlayerEnteredRoom()` and initialized connections only after both players joined.

- Issue: Splitting blocks via joint detachment produced inconsistent multi-connections.  
  Solution: Reworked split logic to track objects inside the gate, recreate affected blocks, and reattach those within valid proximity.

- Issue: Visual quality dropped significantly on standalone headsets.  
  Solution: Followed Pico optimization guidelines—enabled 4× MSAA, optimized textures, and improved text rendering for clarity and performance.


## Links
- 🌐 [Portfolio Page](https://www.henrywang.online/barlog) – Full project breakdown and gameplay demo video  
