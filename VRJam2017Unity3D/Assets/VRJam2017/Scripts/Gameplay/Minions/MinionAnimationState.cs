using UnityEngine;

public class MinionAnimationState : MonoBehaviour
{
    Animator animator;

    public void Start()
    {
        animator = GetComponentInChildren<Animator>();
        
        Play(GetIdle());
    }

    public void StartMoving()
    {
        Play("walk");
    }

    public void StopMoving()
    {
        Play(GetIdle());
    }

    public void Attack()
    {
        Play("digging");
    }

    public void Dig()
    {
        Play("digging");
    }

    public void WantsToDie()
    {
        Play("dieing");
    }

    public void DoneDigging()
    {
        Play(GetIdle());
    }

    public void Flail()
    {
        // NOTE: Spelling error.
        Play("flayling");
    }

    public string GetIdle()
    {
        return Random.value > 0.5 ? "idle" : "idle2";
    }

    public void Play(string name)
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);

        if (!info.IsName(name) || name == "walk")
        {
            animator.Play(name);
        }
        
    }
}