using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ActionQueue))]
public class Attack : MonoBehaviour
{
    public enum Type 
    {
        Chase,
        Static
    }

    public Type AttackType;

    public float AttackRange;

    public float AttackPower;

    public float FireRate;

    GameObject currentlyAttacking;
    
    void VisibleObjectSpotted(GameObject visibleObject)
    {
        if(currentlyAttacking == visibleObject)
        {
            return;
        }

        if(currentlyAttacking != null)
        {
            return;
        }

        ActionQueue queue = GetComponent<ActionQueue>();

        bool sameFaction = gameObject.GetComponent<Faction>().CurrentFaction == visibleObject.GetComponent<Faction>().CurrentFaction;

        if(!sameFaction)
        {
            currentlyAttacking = visibleObject;

            switch(AttackType)
            {
                case Type.Chase:
                    if(!queue.HasActions())
                    {
                        queue.Add(new AttackAction(gameObject, currentlyAttacking));
                        queue.Add(new MovementAction(gameObject, transform.position));
                    }
                    else
                    {
                        queue.InsertBeforeCurrent(new AttackAction(gameObject, currentlyAttacking));
                    }
                    
                    break;
                case Type.Static:
                    queue.InsertBeforeCurrent(new StaticAttackAction(gameObject, currentlyAttacking));
                    break;
            }
            
        }        
    }

}