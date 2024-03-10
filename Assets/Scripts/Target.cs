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

    public GameObject deathParticlesPrefab; 

    void Start()
    {
        FindAllRenderers(transform);
        flashRate = 0.05f;
        duration = 2f;
        IsFlashing = false;
    }


    void FindAllRenderers(Transform parent)
    {
        MeshRenderer parentRenderer = parent.GetComponent<MeshRenderer>();
        if (parentRenderer != null)
        {
            allRenderers.Add(parentRenderer);
        }

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

        if(deathParticlesPrefab != null)
        {
            GameObject deathEffectInstance = Instantiate(deathParticlesPrefab, transform.position, Quaternion.identity);
            ParticleSystem deathParticles = deathEffectInstance.GetComponent<ParticleSystem>();
        
            if (deathParticles != null)
            {
                deathParticles.Play();
                Destroy(deathEffectInstance, deathParticles.main.duration + deathParticles.main.startLifetime.constantMax);
            }
            else
            {
                Debug.LogError("No ParticleSystem found on the instantiated death particles prefab.");
            }
        }
            
        else
        {
            Debug.LogError("Death particles prefab not assigned.");
        }
        StartCoroutine(InvisibleAfterDelay(0.2f));
    }

    IEnumerator InvisibleAfterDelay(float delay)
    {
        // Make the game object invisible
        yield return new WaitForSeconds(delay);
        SetVisibility(false);
        yield return new WaitForSeconds(8f);
        SetVisibility(true);
    }

    void SetVisibility(bool isVisible)
    {
        foreach (MeshRenderer renderer in allRenderers)
        {
            renderer.enabled = isVisible;
        }

        foreach (MeshRenderer renderer in allRenderers)
        {
            renderer.enabled = isVisible;
        }   
    }
    void Focus()
    {

    }
}

