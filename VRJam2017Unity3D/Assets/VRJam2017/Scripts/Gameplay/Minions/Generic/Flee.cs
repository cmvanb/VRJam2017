using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ActionQueue))]
public class Flee : MonoBehaviour
{
    GameObject currentlyFleeing;
    
    void VisibleObjectSpotted(GameObject visibleObject)
    {
        if(gameObject.GetComponent<Faction>().CurrentFaction != Faction.FactionType.Neutral)
        {
            return;
        }

        if(currentlyFleeing == visibleObject)
        {
            return;
        }

        if(currentlyFleeing != null)
        {
            return;
        }

        ActionQueue queue = GetComponent<ActionQueue>();

        bool isScary = gameObject.GetComponent<Faction>().IsScary(visibleObject);

        if(isScary && queue.IsCurrentInteruptable())
        {
            currentlyFleeing = visibleObject;
            
            queue.InsertBeforeCurrent(new FleeAction(gameObject, currentlyFleeing));
        }        
    }

}