using UnityEngine;
using System;
using System.Collections.Generic;

public class UnityMainThreadDispatcher : MonoBehaviour
{
    private static UnityMainThreadDispatcher instance;
    private Queue<Action> actionQueue = new Queue<Action>();

    // Property to access the instance
    public static UnityMainThreadDispatcher Instance
    {
        get { return instance; }
        set { instance = value; }
    }

    // Enqueue method to add actions to be executed on the main thread
    public void Enqueue(Action action)
    {
        lock (actionQueue)
        {
            actionQueue.Enqueue(action);
        }
    }

    // Update method to execute enqueued actions on the main thread
    void Update()
    {
        lock (actionQueue)
        {
            while (actionQueue.Count > 0)
            {
                actionQueue.Dequeue().Invoke();
            }
        }
    }
}
