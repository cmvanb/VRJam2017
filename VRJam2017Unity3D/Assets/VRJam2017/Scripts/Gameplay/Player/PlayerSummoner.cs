using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSummoner : MonoBehaviour 
{
    // SUMMON
    public float SummonInterval = 0.5f;

    [HideInInspector]
    public List<GameObject> SummonedMinions = new List<GameObject>();
    [HideInInspector]
    public bool IsSummoning = false;

    public void Summon()
    {
        Debug.LogWarning("SUMMON");
        IsSummoning = true;

        StartCoroutine(DelayedSummon());
    }

    private IEnumerator DelayedSummon()
    {
        while (IsSummoning)
        {
            yield return new WaitForSeconds(SummonInterval);

            SummonOneMinion();
        }

        yield return null;
    }

    public void StopSummon()
    {
        StopAllCoroutines();

        Debug.LogWarning("STOP SUMMON");

        IsSummoning = false;
    }

    private void SummonOneMinion()
    {
        GameObject minion = MinionManager.Instance.SummonOne(GameManager.Instance.Headset.gameObject);

        if (minion != null)
        {
            SummonedMinions.Add(minion);
            Debug.Log(SummonedMinions.Count + " minions summoned");
        }
        else
        {
            Debug.LogWarning("no more minions nearby");
        }
    }

}
