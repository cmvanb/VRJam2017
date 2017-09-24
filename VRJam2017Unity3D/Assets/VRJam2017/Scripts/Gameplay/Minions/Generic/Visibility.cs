using UnityEngine;

public class Visibility : MonoBehaviour
{
    public enum State
    {
        Visible,
        Cloaked
    }

    public State CurrentState;

    void Start()
    {
        VisibilityRegister.Instance.Register(this);
    }

    void OnDestroy()
    {
        VisibilityRegister.Instance.Deregister(this);
    }
}