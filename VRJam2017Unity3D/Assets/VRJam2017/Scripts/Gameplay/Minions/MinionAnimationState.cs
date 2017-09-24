using UnityEngine;

public class MinionAnimationState : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        
        Play(GetIdle());
    }

    void StartMoving()
    {
        Play("walk");
    }

    void StopMoving()
    {
        Play(GetIdle());
    }

    void Attack()
    {
        Play("digging");
    }

    void Dig()
    {
        Play("digging");
    }

    void WantsToDie()
    {
        Play("dieing");
    }

    void DoneDigging()
    {
        Play(GetIdle());
    }

    string GetIdle()
    {
        return Random.value > 0.5 ? "idle" : "idle2";
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