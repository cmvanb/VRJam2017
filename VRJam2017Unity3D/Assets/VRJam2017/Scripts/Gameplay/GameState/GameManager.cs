using System.Collections.Generic;
using UnityEngine;
using AltSrc.UnityCommon.Patterns;

public class GameManager : MonoSingleton<GameManager>
{
    public Vector3 SpawnPosition;
    public GameObject PlayerCameraRig;

    public void Start()
    {
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        float y = LevelHelpers.GetTerrainHeightAtWorldPos(SpawnPosition);

        PlayerCameraRig.transform.position = new Vector3(SpawnPosition.x, y, SpawnPosition.z);
    }
}
