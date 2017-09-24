using UnityEngine;
using System.Collections;

public class Summoner  : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(SummonEverySecond());
    }

    IEnumerator SummonEverySecond()
    {
        while(true)
        {
            yield return new WaitForSeconds(1);

            var minion = MinionManager.Instance.SummonOne(gameObject);

            Debug.Log(minion);
        }
    }
}