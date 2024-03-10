using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Target : MonoBehaviour
{
    public Material originalMaterial; // Assign in the inspector
    public Material flashMaterial; // Assign a bright or white material in the inspector
    private List<MeshRenderer> allRenderers = new List<MeshRenderer>(); // List to hold all MeshRenderers
    private float flashRate;
    private float duration;
    public bool IsFlashing;
    void Start()
    {
        // Recursively find all MeshRenderers in this GameObject's children and sub-children
        FindAllRenderers(transform);
        flashRate = 0.05f;
        duration = 2f;
        IsFlashing = false;
    }


    void FindAllRenderers(Transform parent)
    {
        // First, check the parent itself (this ensures it works even if the GameObject has no children)
        MeshRenderer parentRenderer = parent.GetComponent<MeshRenderer>();
        if (parentRenderer != null)
        {
            allRenderers.Add(parentRenderer);
        }

        // Then, recursively check all children
        foreach (Transform child in parent)
        {
            FindAllRenderers(child);
        }
    }


    public void Flash()
    {
        StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        IsFlashing = true;
        for (int i = 0; i < duration / flashRate; i++)
        {
            if (i % 2 == 0)
            {
                foreach (var renderer in allRenderers)
                {
                    renderer.material = flashMaterial;
                }
            }
            else
            {
                foreach (var renderer in allRenderers)
                {
                    renderer.material = originalMaterial;
                }
            }

            yield return new WaitForSeconds(flashRate);
        }

        yield return new WaitForSeconds(0.01f); // Wait for 0.01 seconds
        IsFlashing = false;
    }

    public void MakeInvisibleAndBack()
    {
        // Add any additional logic here (e.g., play animations, sound effects)
        StartCoroutine(InvisibleAfterDelay(0.5f));
    }

    IEnumerator InvisibleAfterDelay(float delay)
    {
        // Make the game object invisible
        SetVisibility(false);

        // Wait for 8 seconds while the object is invisible
        yield return new WaitForSeconds(8f);

        // Then make the game object visible again
        SetVisibility(true);
    }

    void SetVisibility(bool isVisible)
    {
        // Assuming allRenderers has been populated by FindAllRenderers method beforehand
        foreach (MeshRenderer renderer in allRenderers)
        {
            renderer.enabled = isVisible;
        }
    }
    void Focus()
    {

    }
}

