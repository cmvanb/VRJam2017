using UnityEngine;

public class MinionRegister : MonoBehaviour
{

    void Start()
    {
        if(GetComponent<Faction>().CurrentFaction == MinionManager.Instance.FactionType)
        {
            MinionManager.Instance.Register(gameObject);
        }
    }

    void OnDestroy()
    {
        if (MinionManager.Instance)
        {
            MinionManager.Instance.Deregister(gameObject);
        }
    }
}