using UnityEngine;

public class Faction : MonoBehaviour
{
    public enum FactionType
    {
        Square,
        Triangle,
        Circle
    }

    public FactionType CurrentFaction;


    public bool IsSameFaction(GameObject other)
    {
        return other.GetComponent<Faction>().CurrentFaction == CurrentFaction;
    }

    void Convert(FactionType newFaction)
    {
        CurrentFaction = newFaction;
    }

}