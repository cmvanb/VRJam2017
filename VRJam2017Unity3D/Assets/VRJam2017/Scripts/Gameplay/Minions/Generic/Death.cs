using UnityEngine;

public class Death : MonoBehaviour
{
    public bool Dead;

    void Start()
    {
        Dead = false;
    }

    void WantsToDie()
    {
        Dead = true;

        Destroy(gameObject, 2);
    }
}