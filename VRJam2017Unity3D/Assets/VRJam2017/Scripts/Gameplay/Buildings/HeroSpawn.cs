using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class HeroSpawn : MonoBehaviour
{
    [SerializeField]
    GameObject[] heroes;

    [SerializeField]
    float waveSpawnTime = 5;


    List<GameObject> currentHeroes;

    void Start()
    {
        currentHeroes = new List<GameObject>();

        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while(true)
        {
            currentHeroes.RemoveAll(item => item == null);

            yield return new WaitForSeconds(waveSpawnTime);

            if(currentHeroes.Count == 0)
            {
                SpawnWave();
            }
        }
    }

    void SpawnWave()
    {
        foreach(GameObject hero in heroes)
        {
            GameObject spawned = Instantiate(hero, transform.position, transform.rotation);

            currentHeroes.Add(spawned);

            Vector3 pos = new Vector3(0,0,0);
		
            UnitAction action = new TerrainAction(spawned, pos);

            spawned.GetComponent<ActionQueue>().Add(action);
        }
    }

}