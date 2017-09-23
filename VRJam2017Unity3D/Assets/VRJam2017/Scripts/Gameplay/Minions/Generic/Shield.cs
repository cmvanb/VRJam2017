using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour
{
    [SerializeField]
    float shield;

    [SerializeField]
    float maxShield;

    [SerializeField]
    float regenerationRate;

    void WantsToDamage(float amount)
    {
        shield -= amount;

        if(shield <= 0)
        {
            amount = -shield;
            
            shield = 0;

            SendMessage("Damage", amount);
        }
    }

    void Start()
    {
        StartCoroutine(RegenerateShield());
    }

    IEnumerator RegenerateShield()
    {
        while(true)
        {
            yield return new WaitForSeconds(1.0f);
            
            shield += regenerationRate;

            shield = Mathf.Min(maxShield, shield);
        }
        
    }
}