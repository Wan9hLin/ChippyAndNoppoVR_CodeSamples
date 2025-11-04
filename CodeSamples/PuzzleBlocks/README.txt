# PuzzleBlocks System

Focus:
Defines the core logic and network behavior of puzzle blocks, including alignment detection, attachment, and position correction.  
Ensures that block connections are physically accurate and visually consistent across all clients in the multiplayer environment.

Key Scripts:
- BlockController_Photon.cs – Manages block behavior and attachment using `FixedJoint`; recalculates offsets and synchronizes positions across the network for seamless assembly.  
- BlockFace_Photon.cs – Detects proximity between block faces, provides alignment feedback, and triggers attachment logic when positional thresholds are met.
