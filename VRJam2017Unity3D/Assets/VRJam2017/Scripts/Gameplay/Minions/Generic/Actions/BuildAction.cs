
using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(BuildQueue))]
public class BuildAction : UnitAction
{
    public Vector3 Position = Vector3.zero;

    public GameObject GameObject;

    public float BuildTime;

    public UnitAction PostBuildAction;

    public BuildAction(GameObject owner, GameObject gameObject, Vector3 position, UnitAction postBuildAction) : base(owner)
    {
        GameObject = gameObject;

        Position = position;

        PostBuildAction = postBuildAction;
    }

    public override IEnumerator Perform()
    {
        ObjectDefinition buildDefinition = GameObject.GetComponent<ObjectDefinition>();

        ResourceResolver resources = Owner.GetComponent<ResourceResolver>();

        BuildQueue queue = Owner.GetComponent<BuildQueue>();

        queue.OnBuildComplete += BuildComplete;

        if(resources.CanAfford(buildDefinition))
        {
            resources.Spend(buildDefinition);

            queue.Add(GameObject, Position, buildDefinition.BuildTime, PostBuildAction);
        }

        yield return 0;
    }

    void BuildComplete()
    {
        Owner.GetComponent<BuildQueue>().OnBuildComplete -= BuildComplete;

        Owner.SendMessage("ActionComplete");
    }
}