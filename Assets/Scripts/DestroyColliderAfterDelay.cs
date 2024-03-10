using UnityEngine;

public class DestroyColliderAfterDelay : MonoBehaviour
{
    public float delay = 5.0f; // Time delay before destroying the GameObject
    //new: death effect 
    private void Start()
    {
       Destroy(gameObject, delay);
    }
        // Schedule the destruction of the GameObject after the specified delay
}

