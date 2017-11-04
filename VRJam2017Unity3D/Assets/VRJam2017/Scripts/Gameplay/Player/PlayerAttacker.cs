using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    // ATTACKING
    [HideInInspector]
    public bool IsAttacking = false;

    public void Attack()
    {
        // Debug.LogWarning("ATTACK");

        IsAttacking = true;
    }

    public void StopAttack()
    {
        // Debug.LogWarning("STOP ATTACK");

        IsAttacking = false;
    }
}
