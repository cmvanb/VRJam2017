using UnityEngine;
using System.Collections;

public class GoblinVillage : MonoBehaviour
{
    [SerializeField]
    GameObject goblin;

    public void Start()
    {
        int amount = Random.Range(2, 5);

        float radius = 15;

        for(int i=0; i<amount; i++)
        {
            Vector2 offset = radius * Random.insideUnitCircle;
            Vector3 spawnPos = transform.position;
            spawnPos.x = spawnPos.x + offset.x;
            spawnPos.z = spawnPos.z + offset.y;
            spawnPos.y = LevelHelpers.GetTerrainHeightAtWorldPos(spawnPos);

            GameObject.Instantiate(goblin, spawnPos, Quaternion.identity);
        }
    }
}