
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class MovementAction : UnitAction
{
    NavMeshAgent agent;

    public Vector3 Position = Vector3.zero;


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

        if(XZDifference.sqrMagnitude > epsilon)
        {
            Owner.SendMessage("StartMoving");
    
            agent.SetDestination(destination);
        }
    }

    protected bool hasReachedTarget()
    {
        if(agent.pathPending)
        {
            return false;
        }

        float dist = agent.remainingDistance;

        bool validPath = (agent.pathStatus == NavMeshPathStatus.PathComplete || agent.pathStatus == NavMeshPathStatus.PathPartial);

        return dist != Mathf.Infinity && validPath && dist == 0;
    }

    public override void Stop()
    {
        if(!agent.isStopped)
        {
            agent.Stop();

            agent.ResetPath();

            Owner.SendMessage("StopMoving");
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