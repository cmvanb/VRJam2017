using System.Collections.Generic;
using UnityEngine;
using AltSrc.UnityCommon.Patterns;
using VRTK;

public class GameManager : MonoSingleton<GameManager>
{
    public float MinionSpawnDistance = 10f;
    public int StartingMinions = 10;

    [HideInInspector]
    public Transform PlayArea;
    [HideInInspector]
    public Transform Headset;

    public void Start()
    {
        PlayArea = VRTK_DeviceFinder.PlayAreaTransform();
        Headset = VRTK_DeviceFinder.DeviceTransform(VRTK_DeviceFinder.Devices.Headset);
    }

    public void SpawnPlayer(Vector3 spawnPosition)
    {
        SetPlayerPosition(spawnPosition);

        SpawnStartingMinions(StartingMinions, spawnPosition);
    }

    public void SetPlayerPosition(Vector3 position)
    {
        // HACK - APPEARS TO BE UNNECESSARY NOW FOR [INSERT DARK MAGIC REASONS HERE].
        float playerHeightOffset = 0f;

        float terrainHeight = LevelHelpers.GetTerrainHeightAtWorldPos(position);

        PlayArea.position = new Vector3(position.x, terrainHeight + playerHeightOffset, position.z);
    }

    public void SpawnStartingMinions(int num, Vector3 centerPosition)
    {
        float interval = (2 * Mathf.PI) / num;

        for (int i = 0; i < num; ++i)
        {
            float angle = interval * i;

            float x = MinionSpawnDistance * Mathf.Cos(angle);
            float z = MinionSpawnDistance * Mathf.Sin(angle);
            float y = LevelHelpers.GetTerrainHeightAtWorldPos(new Vector3(x, 0f, z));

            Vector3 position = (new Vector3(x + centerPosition.x, y, z + centerPosition.z));

            MinionManager.Instance.SpawnMinion(position);
        }
    }
}
