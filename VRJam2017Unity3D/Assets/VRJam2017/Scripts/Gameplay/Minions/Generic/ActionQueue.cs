using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionQueue : MonoBehaviour
{   
    List<UnitAction> queue = new List<UnitAction>();

    private UnitAction current = null;

    private IEnumerator currentRoutine = null;

    private bool captured = false;

    void Start()
    {
    }

    public void Add(UnitAction action)
    {
        if(captured)
        {
            return;
        }
        
        queue.Add(action);
    }

    public void Insert(UnitAction action)
    {
        if(captured)
        {
            return;
        }

        queue.Insert(0, action);
    }

    public void Set(UnitAction action)
    {
    
        StopCurrent();
        
        queue.Clear();

        Add(action);
    }

    void PerformNext()
    {
        if(queue.Count > 0)
        {
            current = queue[0];

            queue.RemoveAt(0);

            SendMessage("Action", current, SendMessageOptions.DontRequireReceiver);

            currentRoutine = current.Perform();

            StartCoroutine(currentRoutine);
        }

    }

    void Update()
    {
        if(current == null)
        {
            PerformNext();
        }
    }

    void ActionComplete()
    {
        current = null;

        PerformNext();
    }

    void Place()
    {
        if(current != null)
        {
            current.Place();
        }
    }

    public void StopCurrent()
    {
        if(current != null)
        {
            current.Stop();

            StopCoroutine(currentRoutine);

            current = null;
        }
    }

    public void InsertBeforeCurrent(UnitAction newAction)
    {
        if(current != null)
        {
            UnitAction next = current;

            StopCurrent();

            Insert(next);
        }
        
        Insert(newAction);
    }

    public bool HasActions()
    {
        return queue.Count > 0 || current != null;
    }

    void Capture()
    {
        StopCurrent();

        queue.Clear();

        captured = true;
    }

    void Release()
    {
        captured = false;
    }
}