using UnityEngine;
using System.Collections.Generic;

public class Faction : MonoBehaviour
{

    public enum FactionType
    {
        Hell = 0,
        Heaven = 1,
        Neutral = 2
    }

    [SerializeField]
    Renderer[] skinMaterials;

    [SerializeField]
    Renderer[] eyeMaterials;

    public FactionType CurrentFaction;

    void Start()
    {
        SetColors();
    }

    void SetColors()
    {
        foreach(Renderer renderer in skinMaterials)
        {
            renderer.material.color = (CurrentFaction == FactionType.Hell) 
                ? new Color32(0xFF, 0x5A, 0x00, 0xFF) : new Color32(255,255,255,255);
        }

        foreach(Renderer renderer in eyeMaterials)
        {
            foreach(Material material in renderer.materials)
            {
                Color32 color = material.color;
                material.color = (CurrentFaction == FactionType.Hell) 
                    ? new Color32(0x00, 0x00, 0x00, color.a) : new Color32(255,255,255,color.a);
            }
        }
    }


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
        return CurrentFaction != FactionType.Neutral && other.GetComponent<Faction>().CurrentFaction == FactionType.Neutral;
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

        SetColors();

        CurrentFaction = newFaction;
    }
}