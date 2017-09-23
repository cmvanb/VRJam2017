using System.Collections.Generic;
using UnityEngine;
using AltSrc.UnityCommon.Patterns;
using VRTK;

public class GameManager : MonoSingleton<GameManager>
{
    public Vector3 SpawnPosition;

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

        SpawnStartingMinions(4);
    }

    public void SetPlayerPosition(Vector3 position)
    {
        // NOTE: HACK - COULDNT FIGURE OUT HOW TO CALCULATE EXACT PLAYER HEIGHT IN VRTK
        float hackOffset = 1.15f;

        float playerHeightOffset = hackOffset;

        float terrainHeight = LevelHelpers.GetTerrainHeightAtWorldPos(SpawnPosition);

        PlayArea.position = new Vector3(position.x, terrainHeight + playerHeightOffset, position.z);
    }

    public void SpawnStartingMinions(int num)
    {
        float interval = 360f / num;

        for (int i = 0; i < num; ++i)
        {
            float deg = interval * i;
            float distance = 10f;

            float x = distance * Mathf.Cos(deg);
            float z = distance * Mathf.Sin(deg);
            float y = LevelHelpers.GetTerrainHeightAtWorldPos(new Vector3(x, 0f, z));

            Vector3 position = new Vector3(x, y, z) + SpawnPosition;

            MinionManager.Instance.SpawnMinion(position);

            Debug.LogWarning(position);
        }
    }
}
