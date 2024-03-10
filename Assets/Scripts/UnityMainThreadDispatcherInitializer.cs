using UnityEngine;

public class UnityMainThreadDispatcherInitializer : MonoBehaviour
{
    void Awake()
    {
        // Ensure there is only one instance of UnityMainThreadDispatcher
        UnityMainThreadDispatcher[] dispatchers = FindObjectsOfType<UnityMainThreadDispatcher>();
        if (dispatchers.Length == 0)
        {
            GameObject dispatcherObject = new GameObject("UnityMainThreadDispatcher");
            UnityMainThreadDispatcher dispatcher = dispatcherObject.AddComponent<UnityMainThreadDispatcher>();
            UnityMainThreadDispatcher.Instance = dispatcher;
        }
        else if (dispatchers.Length == 1)
        {
            UnityMainThreadDispatcher.Instance = dispatchers[0];
        }
        else
        {
            Debug.LogError("Multiple instances of UnityMainThreadDispatcher found. Please ensure there's only one.");
        }
    }
}
