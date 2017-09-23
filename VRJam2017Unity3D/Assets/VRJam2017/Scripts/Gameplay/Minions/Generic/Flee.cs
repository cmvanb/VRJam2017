using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ActionQueue))]
public class Flee : MonoBehaviour
{
    GameObject currentlyFleeing;
    
    void VisibleObjectSpotted(GameObject visibleObject)
    {
        if(currentlyFleeing == visibleObject)
        {
            return;
        }

        if(currentlyFleeing != null)
        {
            return;
        }

        ActionQueue queue = GetComponent<ActionQueue>();

        bool sameFaction = gameObject.GetComponent<Faction>().CurrentFaction == visibleObject.GetComponent<Faction>().CurrentFaction;

        if(!sameFaction)
        {
            currentlyFleeing = visibleObject;
            
            queue.InsertBeforeCurrent(new FleeAction(gameObject, currentlyFleeing));
        }        
    }

}