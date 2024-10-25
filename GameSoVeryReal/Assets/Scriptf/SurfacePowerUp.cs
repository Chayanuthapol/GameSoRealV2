using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfacePowerUp : MonoBehaviour
{
    public enum PowerUpType
    {
        Slippery,
        Sticky
    }

    [Header("Power-up Settings")]
    public PowerUpType type;
    public float effectDuration = 5f;
    public Material powerUpMaterial;
    public float frictionMultiplier = 1f; // Will be < 1 for slippery, > 1 for sticky

    [Header("Visual Effects (Optional)")]
    public ParticleSystem activationEffect;
    public bool rotateInPlace = true;
    public float rotationSpeed = 100f;

    private void Start()
    {
        // Set default friction multiplier based on type if not set
        if (frictionMultiplier == 1f)
        {
            frictionMultiplier = (type == PowerUpType.Slippery) ? 0.1f : 5f;
        }

        if (rotateInPlace)
        {
            StartCoroutine(RotatePowerUp());
        }
    }

    private IEnumerator RotatePowerUp()
    {
        while (true)
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if it's the white ball
        if (other.CompareTag("WhiteBall"))
        {
            // Start the power-up effect
            StartCoroutine(ApplyEffect());
            
            // Play particle effect if assigned
            if (activationEffect != null)
            {
                ParticleSystem effect = Instantiate(activationEffect, transform.position, Quaternion.identity);
                effect.Play();
                Destroy(effect.gameObject, effect.main.duration);
            }

            // Disable the power-up object
            gameObject.SetActive(false);
        }
    }

    private IEnumerator ApplyEffect()
    {
        // Find the floor
        GameObject floor = GameObject.FindGameObjectWithTag("Floor");
        if (floor == null)
        {
            Debug.LogError("Floor not found! Make sure it has the 'Floor' tag.");
            yield break;
        }

        // Get required components
        Renderer floorRenderer = floor.GetComponent<Renderer>();
        Collider floorCollider = floor.GetComponent<Collider>();

        if (floorRenderer == null || floorCollider == null || floorCollider.material == null)
        {
            Debug.LogError("Floor must have both Renderer and Collider with Physics Material!");
            yield break;
        }

        // Store original properties
        Material originalMaterial = floorRenderer.material;
        float originalFriction = floorCollider.material.dynamicFriction;

        // Apply power-up effect
        if (powerUpMaterial != null)
        {
            floorRenderer.material = powerUpMaterial;
        }

        // Apply physics changes
        floorCollider.material.dynamicFriction *= frictionMultiplier;
        floorCollider.material.staticFriction *= frictionMultiplier;

        // Wait for duration
        yield return new WaitForSeconds(effectDuration);

        // Restore original properties
        floorRenderer.material = originalMaterial;
        floorCollider.material.dynamicFriction = originalFriction;
        floorCollider.material.staticFriction = originalFriction;

        // Destroy the power-up object
        Destroy(gameObject);
    }
}