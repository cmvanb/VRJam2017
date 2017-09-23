using UnityEngine;

public class Visibility : MonoBehaviour
{
    public enum State
    {
        Visible,
        Cloaked
    }

    public State CurrentState;
}