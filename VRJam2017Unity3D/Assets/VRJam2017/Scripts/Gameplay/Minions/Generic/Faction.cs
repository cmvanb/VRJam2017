using UnityEngine;

public class Faction : MonoBehaviour
{
    public enum FactionType
    {
        Hell,
        Heaven,
        Neutral
    }

    public FactionType CurrentFaction;


    public bool IsSameFaction(GameObject other)
    {
        return other.GetComponent<Faction>().CurrentFaction == CurrentFaction;
    }

    void Convert(FactionType newFaction)
    {
        CurrentFaction = newFaction;

        switch(newFaction)
        {
            case FactionType.Heaven:
                break;
            case FactionType.Hell:
                break;
            case FactionType.Neutral:
                break;
        }
    }

}