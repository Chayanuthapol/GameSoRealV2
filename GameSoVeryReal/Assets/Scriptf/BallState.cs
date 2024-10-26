using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class BallState : MonoBehaviour
{
    [Header("Particle Effects")]
    [SerializeField] private GameObject freezeParticlePrefab;
    [SerializeField] private Color freezeParticleColor = new Color(0.7f, 0.9f, 1f, 1f); // Ice blue
    [SerializeField] private float particleScale = 2f; // Control overall particle size
    
    private Material originalMaterial;
    private float originalFriction;
    private PhysicMaterial physicsMaterial;
    private MeshRenderer meshRenderer;
    private Coroutine freezeCoroutine;
    private ParticleSystem activeParticles;
    private ParticleSystem burstParticles;
    
    public bool IsNormal { get; private set; } = true;
    
    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        originalMaterial = meshRenderer.material;
        
        var collider = GetComponent<Collider>();
        if (collider != null)
        {
            if (collider.material == null)
            {
                physicsMaterial = new PhysicMaterial("BallPhysics");
                collider.material = physicsMaterial;
            }
            else
            {
                physicsMaterial = collider.material;
            }
            originalFriction = physicsMaterial.dynamicFriction;
        }
    }
    
    public void FreezeBall(Material freezeMaterial, float duration, float frozenFriction)
    {
        if (freezeCoroutine != null)
        {
            StopCoroutine(freezeCoroutine);
            if (activeParticles != null)
            {
                Destroy(activeParticles.gameObject);
            }
            if (burstParticles != null)
            {
                Destroy(burstParticles.gameObject);
            }
        }
        
        freezeCoroutine = StartCoroutine(FreezeEffect(freezeMaterial, duration, frozenFriction));
    }
    
    private void CreateFreezeParticles()
    {
        if (freezeParticlePrefab != null)
        {
            // Create continuous ice aura
            GameObject particleObj = Instantiate(freezeParticlePrefab, transform.position, Quaternion.identity);
            particleObj.transform.SetParent(transform);
            
            activeParticles = particleObj.GetComponent<ParticleSystem>();
            if (activeParticles != null)
            {
                ConfigureMainParticleSystem(activeParticles);
            }

            // Create initial burst effect
            GameObject burstObj = Instantiate(freezeParticlePrefab, transform.position, Quaternion.identity);
            burstObj.transform.SetParent(transform);
            
            burstParticles = burstObj.GetComponent<ParticleSystem>();
            if (burstParticles != null)
            {
                ConfigureBurstParticleSystem(burstParticles);
            }
        }
    }

    private void ConfigureMainParticleSystem(ParticleSystem ps)
    {
        var main = ps.main;
        main.startColor = freezeParticleColor;
        main.startSize = 0.5f * particleScale;
        main.startLifetime = 2f;
        main.simulationSpace = ParticleSystemSimulationSpace.World;

        var emission = ps.emission;
        emission.rateOverTime = 30;

        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 1f * particleScale;
        
        var colorOverLifetime = ps.colorOverLifetime;
        colorOverLifetime.enabled = true;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { 
                new GradientColorKey(freezeParticleColor, 0.0f),
                new GradientColorKey(freezeParticleColor, 1.0f) 
            },
            new GradientAlphaKey[] { 
                new GradientAlphaKey(1.0f, 0.0f),
                new GradientAlphaKey(0.0f, 1.0f)
            }
        );
        colorOverLifetime.color = gradient;

        // Add trails
        var trails = ps.trails;
        trails.enabled = true;
        trails.ratio = 0.5f;
        trails.lifetime = 0.25f;
    }

    private void ConfigureBurstParticleSystem(ParticleSystem ps)
    {
        var main = ps.main;
        main.startColor = freezeParticleColor;
        main.startSize = 0.7f * particleScale;
        main.startSpeed = 5f;
        main.startLifetime = 1f;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        main.loop = false;

        var emission = ps.emission;
        emission.SetBursts(new []
        {
            new ParticleSystem.Burst(0f, 50)
        });

        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 0.1f;

        // Add trails for burst particles
        var trails = ps.trails;
        trails.enabled = true;
        trails.ratio = 1f;
        trails.lifetime = 0.2f;
    }
    
    private IEnumerator FreezeEffect(Material freezeMaterial, float duration, float frozenFriction)
    {
        IsNormal = false;
        
        // Visual effects
        meshRenderer.material = freezeMaterial;
        CreateFreezeParticles();
        
        // Physics changes
        if (physicsMaterial != null)
        {
            physicsMaterial.dynamicFriction = frozenFriction;
            physicsMaterial.staticFriction = frozenFriction;
        }
        
        yield return new WaitForSeconds(duration);
        
        // Restore original state
        meshRenderer.material = originalMaterial;
        if (physicsMaterial != null)
        {
            physicsMaterial.dynamicFriction = originalFriction;
            physicsMaterial.staticFriction = originalFriction;
        }
        
        // Clean up particles
        if (activeParticles != null)
        {
            var emission = activeParticles.emission;
            emission.enabled = false;
            
            yield return new WaitForSeconds(2f);
            Destroy(activeParticles.gameObject);
        }
        
        if (burstParticles != null)
        {
            Destroy(burstParticles.gameObject);
        }
        
        IsNormal = true;
        freezeCoroutine = null;
    }
    
    private void OnDestroy()
    {
        if (physicsMaterial != null)
        {
            Destroy(physicsMaterial);
        }
        if (activeParticles != null)
        {
            Destroy(activeParticles.gameObject);
        }
        if (burstParticles != null)
        {
            Destroy(burstParticles.gameObject);
        }
    }
}