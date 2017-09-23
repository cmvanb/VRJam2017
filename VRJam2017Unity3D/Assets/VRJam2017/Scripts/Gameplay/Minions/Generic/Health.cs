using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    float health;

    [SerializeField]
    float maxHealth;

    void Damage(float amount)
    {
        health -= amount;

        if(health <= 0)
        {
            SendMessage("WantsToDie");
        }
    }

    public bool IsAtMaxHealth()
    {
        return health == maxHealth;
    }

    public void Heal(float amount)
    {
        health += amount;

        health = Mathf.Min(health, maxHealth);
    }
}