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

    public bool IsCapturable(GameObject other)
    {
        return other.GetComponent<Faction>().CurrentFaction == FactionType.Neutral;
    }

    void Convert(FactionType newFaction)
    {
        if(newFaction == CurrentFaction)
        {
            return;
        }

        if(GetComponent<MinionRegister>() != null)
        {
            if(newFaction == MinionManager.Instance.FactionType)
            {
                MinionManager.Instance.Register(gameObject);
            }
            else if(CurrentFaction == MinionManager.Instance.FactionType)
            {
                MinionManager.Instance.Deregister(gameObject);
            }
        }

        CurrentFaction = newFaction;
    }
}