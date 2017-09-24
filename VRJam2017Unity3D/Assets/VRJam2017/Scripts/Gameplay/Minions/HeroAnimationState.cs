using UnityEngine;

public class HeroAnimationState : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        
        Play("idle");
    }

    void StartMoving()
    {
        Play("walk");
    }

    void StopMoving()
    {
        Play("idle");
    }

    void Heal(float amount)
    {
        Play("healing");
    }

    void Attack()
    {
        Play("swingsword");
    }

    void Dig()
    {
        Play("digging");
    }

    void WantsToDie()
    {
        Play("dieing");
    }

    void Play(string name)
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);

        if(!info.IsName(name) || name == "walk")
        {
            animator.Play(name);
        }
        
    }
}