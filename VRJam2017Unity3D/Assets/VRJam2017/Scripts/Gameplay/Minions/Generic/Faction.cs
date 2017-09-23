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

    public bool IsEnemy(GameObject other)
    {
        return !IsSameFaction(other) && other.GetComponent<Faction>().CurrentFaction != FactionType.Neutral;
    }

    public bool IsScary(GameObject other)
    {
        return !IsSameFaction(other) && CurrentFaction == FactionType.Neutral;
    }

    void Convert(FactionType newFaction)
    {
        CurrentFaction = newFaction;

        GetComponent<Attack>().enabled = newFaction != FactionType.Neutral;

        GetComponent<Flee>().enabled = newFaction == FactionType.Neutral;
    }

}