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
        }
        
        int pathPoints = 10;        

        Transform target = GameManager.Instance.Headset;

        Vector3 pos = target == null ? new Vector3(500,0,500) : target.position;

        for(int i=1; i<=pathPoints; i++)
        {
            Vector3 path = pos - transform.position;

            path *= (float)i/pathPoints;

            float angleRange = 90;

            Quaternion quat = Quaternion.Euler(0, (i == pathPoints) ? 0 :  (angleRange/2 - Random.value * angleRange), 0);

            path = quat * path;

            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.transform.position = transform.position + path;

            foreach(GameObject hero in currentHeroes)
            {
                UnitAction action = new TerrainAction(hero, transform.position + path);

                hero.GetComponent<ActionQueue>().Add(action);
            }

            
        }
    }

}