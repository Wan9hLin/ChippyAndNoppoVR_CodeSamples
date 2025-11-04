# Chippy & Noppo VR – Code Samples

## Overview
This repository contains selected code samples from **“Chippy & Noppo VR,”** a two-player collaborative VR puzzle game developed in Unity using **Photon Unity Networking (PUN)** and the **XR Interaction Toolkit.  
The provided scripts highlight key systems that support seamless multiplayer collaboration, including real-time synchronization, block bonding, rotation, dyeing, and split-gate interactions.

## Technical Highlights
- **Modular Multiplayer Architecture** – Networking, interaction, and puzzle systems are separated for clarity and scalability.  
- **Real-Time Synchronization** – All player actions (movement, grabbing, rotation, coloring, splitting) are propagated via Photon RPCs with interpolation for smooth results.  
- **Physics-Based Interaction** – Uses Unity XR Interaction Toolkit and tuned Rigidbody constraints to ensure natural, stable manipulation.  
- **Collaborative Puzzle Logic** – Players must coordinate to modify and assemble blocks that dynamically bond, split, and recolor in shared space.  
- **VR Optimization** – Frame-rate stability and comfort improved through collision tuning, reduced motion discomfort, and texture optimization for standalone headsets.

## How to Run
1. **Requirements**
   - Unity **2021.3 LTS** or later  
   - **Photon Unity Networking (PUN)** package configured with a valid App ID  
   - **XR Interaction Toolkit** v2.4 or later  
   - A compatible VR headset (tested on **Pico XR** and **Meta Quest**)  

2. **Setup**
   - Import the provided `Scripts/` folders into a Unity project.  
   - Attach each script to its corresponding GameObject (refer to comments inside the scripts).  
   - Assign required tags (`CubeFace`, `cubeToy`) and materials (e.g., `redMaterial`).  
   - Configure the XR Rig and ensure Photon PUN is initialized with the correct region and App ID.  

3. **Execution**
   - Open the main demo scene and press **Play**.  
   - Use VR controllers to grab, rotate, dye, and split puzzle blocks collaboratively in real time.  
   - Follow the in-game logic to complete cooperative building challenges.

## System Architecture
The system is organized into four core modules:
- **Networking/** – Handles login, room management, spawning, and player synchronization.  
- **Interaction/** – Manages grabbing, rotation, and object instantiation.  
- **PuzzleBlocks/** – Controls alignment, bonding, and physics-based connections.  
- **PuzzleStations/** – Defines functional puzzle stations such as splitters, dye zones, and barriers.

These modules communicate through Photon RPC events and shared object ownership to maintain consistent world states across clients.

## Challenges & Solutions
| Challenge | Solution |
|------------|-----------|
| Adapting the original *Chippy & Noppo* mechanics to VR caused imbalance and player discomfort. | Adjusted character speed, height, and collider radius; removed jumping and handle rotation to enhance comfort and stability. |
| Grabbing and collision interactions were unstable due to physics inconsistencies. | Tuned Rigidbody and XR Grab Interactable properties (friction, constraints, movement type) for smoother manipulation. |
| Bonded blocks failed to initialize correctly when two players joined simultaneously. | Moved bonding logic from `Start()` to `OnPlayerEnteredRoom()`; invoked `InitializeBlockConnections()` only after both players joined. |
| Splitting blocks via joint detachment caused inconsistent multi-connections. | Reworked the split system to track objects inside the gate, recreate blocks, and reattach nearby ones safely. |
| Maintaining text readability and UI clarity in VR. | Applied Quick Outline plugin and tested multiple fonts for better contrast and visibility. |
| Visual quality dropped on standalone VR hardware. | Followed Pico developer optimization guidelines—enabled 4× AA, optimized textures, and improved text rendering. |

## License
This repository is for **academic and portfolio demonstration** only.  
© Henry Wang (Helin Wang). All rights reserved. Do not use commercially without permission.

## Links
- 🌐 [Portfolio Page](https://henrywang.online) – Full project breakdown and gameplay demo video  
