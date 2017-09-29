# VRJam2017

## Useful links:

* Trello: https://trello.com/b/x2lICgT7/vr-jam-2017  
* Slack: https://vrjam2017.slack.com  

## Unity + Git:

Setup smart merge to make sure Git can merge unity scenes and prefabs. This requires editing `.git/config` in the git repository.  
* Smart Merge: https://docs.unity3d.com/Manual/SmartMerge.html  

## VRTK:

* Documentation: https://vrtoolkit.readme.io/docs/  
* Git repo: https://github.com/thestonefox/VRTK  

To make the VR mode work in any scene: 
1. Add `Assets/VRJam2017/Prefabs/VRTK/VRTK_SDKManager.prefab` to the scene.
2. Add `Assets/VRJam2017/Prefabs/VRTK/VRTK_SteamVRSetup.prefab` to the scene.

To make sure the VR mode is correctly disabled (it will mess with the mainCamera if you don't):
1. Add `Assets/VRJam2017/Prefabs/VRTK/DisableVR.prefab` to the scene.



