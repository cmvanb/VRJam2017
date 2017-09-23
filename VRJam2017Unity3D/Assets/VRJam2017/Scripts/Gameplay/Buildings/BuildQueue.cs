using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildQueue : MonoBehaviour
{
    public delegate void BuildComplete();

    public BuildComplete OnBuildComplete;


    class BuildItem
    {
        public GameObject Prefab;
        
        public Vector3 Position;

        public float BuildTime;

        public UnitAction PostBuildAction;

        public BuildItem(GameObject prefab, Vector3 position, float buildTime, UnitAction postBuildAction)
        {
            Prefab = prefab;

            Position = position;

            BuildTime = buildTime;

            PostBuildAction = postBuildAction;
        }
    }
    
    Factory factory;

    Queue<BuildItem> queue;

    protected Player player = null;

    BuildItem currentBuild = null;

    void Start()
    {
        queue = new Queue<BuildItem>();

        factory = (Factory)FindObjectOfType(typeof(Factory));

        currentBuild = null;
    }

    public void Add(GameObject prefab, Vector3 position, float buildTime, UnitAction postBuildAction)
    {
        queue.Enqueue(new BuildItem(prefab, position, buildTime, postBuildAction));

        if(currentBuild == null)
        {
            StartCoroutine(BuildNext());
        }
    }

    IEnumerator BuildNext()
    {
        if(currentBuild == null && queue.Count > 0)
        {
            currentBuild = queue.Dequeue();

            yield return new WaitForSeconds(currentBuild.BuildTime);

            GameObject spawned = factory.build(currentBuild.Prefab, currentBuild.Position);
            
            spawned.GetComponent<Owner>().CurrentOwner = player;

            if(currentBuild.PostBuildAction != null)
            {
                currentBuild.PostBuildAction.Owner = spawned;

                spawned.GetComponent<ActionQueue>().Add(currentBuild.PostBuildAction);
            }

            OnBuildComplete.Invoke();

            currentBuild = null;
            
            StartCoroutine(BuildNext());
        }

    }
}