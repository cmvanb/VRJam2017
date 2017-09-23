using UnityEngine;

public class MinionAnimationState : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        
        animator.Play(GetIdle());
    }

    void StartMoving()
    {
        animator.Play("walk");
    }

    void StopMoving()
    {
        animator.Play(GetIdle());
    }

    void Attack()
    {
        //Debug.Log("Attack");
    }

    string GetIdle()
    {
        return Random.value > 0.5 ? "idle" : "idle2";
    }
}