using System.Collections.Generic;
using UnityEngine;
using AltSrc.UnityCommon.Patterns;
using VRTK;

public class GameManager : MonoSingleton<GameManager>
{
    public Vector3 SpawnPosition;

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

        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        SetPlayerPosition(SpawnPosition);

        SpawnStartingMinions(StartingMinions);
    }

    public void SetPlayerPosition(Vector3 position)
    {
        // HACK - APPEARS TO BE UNNECESSARY NOW FOR [INSERT DARK MAGIC REASONS HERE].
        float playerHeightOffset = 0f;

        float terrainHeight = LevelHelpers.GetTerrainHeightAtWorldPos(SpawnPosition);

        PlayArea.position = new Vector3(position.x, terrainHeight + playerHeightOffset, position.z);
    }

    public void SpawnStartingMinions(int num)
    {
        float interval = (2 * Mathf.PI) / num;

        for (int i = 0; i < num; ++i)
        {
            float angle = interval * i;

            float x = MinionSpawnDistance * Mathf.Cos(angle);
            float z = MinionSpawnDistance * Mathf.Sin(angle);
            float y = LevelHelpers.GetTerrainHeightAtWorldPos(new Vector3(x, 0f, z));

            Vector3 position = (new Vector3(x + SpawnPosition.x, y, z + SpawnPosition.z));

            MinionManager.Instance.SpawnMinion(position);
        }
    }
}
