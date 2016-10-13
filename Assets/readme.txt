3rd person controller with orbit camera and fly mode

Version:

1.1

Description:

This package provides a basic setup for a third person player controller. Includes scripts for player movement, camera orbit and a Mecanim animator controller, containing basic locomotion - walk, run, sprint, strafe, and also an extra fly mode. The scripts also implements camera collision detection and aim mode. A basic demo scene is included.

Important:

It is necessary to define Input Manager settings for the following keys:

Run (default: mouse 0)
Aim (default: mouse 1)
Sprint (default: left shift)
Fly (default: e)

The package inputSettings, included in this project, contains the custom keys settings listed above.

Usage:

Add the PlayerControl script on the player game object. Add The ThirdPersonOrbitCam script on the camera. Add the CharacterController to the player animator controller. Attach a capsule collider, and a rigidbody component to the player. Also certify to drop the player game object on the public reference of the camera script. An image for the crosshair in aim mode is also necessary for this script to work properly.

Note that the ThirdPersonOrbitCam script is tied to the PlayerControl script. Modifications are necessary in this script to work independently.

It is also mandatory to ensure that the player's mesh will not surpass the bottom of it's capsule collider, otherwise the movement script will not work properly and the player may not move.

Tested with Unity 5

Link:

Author's page: www.dcc.ufmg.br/~allonman
