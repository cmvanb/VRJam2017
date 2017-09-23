
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class TerrainAction : UnitAction
{
    public Vector3 Position = Vector3.zero;

    public TerrainAction(GameObject owner, Vector3 position) : base(owner)
    {
        Position = position;
    }

    public override IEnumerator Perform()
    {
        
        if(Owner.GetComponent<NavMeshAgent>() != null && Owner.GetComponent<NavMeshObstacle>() == null)
        {
            Owner.GetComponent<ActionQueue>().Insert(new MovementAction(Owner, Position));
        }

        if(Owner.GetComponent<BuildQueue>() != null)
        {
            Owner.SendMessage("SetWaypoint", Position);
        }

        Owner.SendMessage("ActionComplete");

        yield return 0;
    }
}