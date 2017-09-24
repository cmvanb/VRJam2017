
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class MovementAction : UnitAction
{
    protected NavMeshAgent agent;

    public Vector3 Position = Vector3.zero;

    protected float stoppingDistance = 2.5f;


    public MovementAction(GameObject owner, Vector3 position) : base(owner)
    {
        Position = position;
    }

    public override IEnumerator Perform()
    {
        setDestination(Position);

        while(!hasReachedTarget())
        {
            yield return 0;     
        }

        Stop();

        Owner.SendMessage("ActionComplete");
    }

    protected void setDestination(Vector3 destination)
    {
        findAgent();

        Vector3 XZDifference = agent.destination - destination;

        XZDifference.y = 0;
        
        float epsilon = 0.01f;

        if(XZDifference.sqrMagnitude > epsilon && agent.isOnNavMesh)
        {
            Owner.SendMessage("StartMoving", SendMessageOptions.DontRequireReceiver);
    
            agent.isStopped = false;
            
            agent.SetDestination(destination);
        }
    }

    protected bool hasReachedTarget()
    {
        if(agent.pathPending || !agent.isOnNavMesh)
        {
            return false;
        }

        float dist = agent.remainingDistance;

        bool validPath = (agent.pathStatus == NavMeshPathStatus.PathComplete || agent.pathStatus == NavMeshPathStatus.PathPartial);

        return dist != Mathf.Infinity && validPath && dist <= stoppingDistance;
    }

    public override void Stop()
    {
        if(agent.isOnNavMesh && !agent.isStopped)
        {
            agent.ResetPath();

            agent.isStopped = true;

            Owner.SendMessage("StopMoving", SendMessageOptions.DontRequireReceiver);
        }
    }

    void findAgent()
    {
        if(agent == null)
        {
            agent = Owner.GetComponent<NavMeshAgent>();
        }
    }
}