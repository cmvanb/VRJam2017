
using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class DigAction : MovementAction
{
    Vector2 digPosition;

    NavMeshAgent agent;

    public DigAction(GameObject owner, Vector2 position) 
    : base(owner, LevelHelpers.WorldPosFromTilePos((int)position.x, (int)position.y) + new Vector3(LevelHelpers.TileSize/2, 0, LevelHelpers.TileSize/2))
    {
        stoppingDistance = LevelHelpers.TileSize/2;
        digPosition = position;

        agent = owner.GetComponent<NavMeshAgent>();
    }

    public override IEnumerator Perform()
    {
        Digger digger = Owner.GetComponent<Digger>();

        if(!digger.AutoDig)
        {
            setDestination(Position);

            while(!hasReachedTarget())
            {
                yield return 0;     
            }

            Stop();
        }

        Owner.SendMessage("Dig", SendMessageOptions.DontRequireReceiver);

        LevelController.Instance.Dig((int)digPosition.x, (int)digPosition.y);

        yield return new WaitForSeconds(digger.DigTime);

        Owner.SendMessage("DoneDigging", SendMessageOptions.DontRequireReceiver);

        Owner.SendMessage("ActionComplete");
    }
}