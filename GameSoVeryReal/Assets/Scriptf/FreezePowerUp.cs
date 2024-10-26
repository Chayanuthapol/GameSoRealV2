using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

// Attach this to the freeze ball powerup
public class FreezePowerup : MonoBehaviour
{
    [SerializeField] private Material freezeMaterial;
    [SerializeField] private float freezeDuration = 5f;
    [SerializeField] private float frozenFriction = 0.1f; // Lower value = more slippery
    [SerializeField] private GameObject freezeParticlePrefab;
    
    public float rotationSpeed = 90f;
    public float bobSpeed = 1f;
    public float bobHeight = 0.1f;
    private Vector3 startPosition;
    
    [Header("Glow Effect")]
    public Color glowColor = new Color(1, 1, 1, 1);
    public Light powerupLight;
    public Rigidbody whiteballRb;
    public float ballMovingThreshold = 0.01f; // Minimum speed for white ball to activate power-up
    public float respawnDelay = 0.01f; // Delay before respawning power-up
    void Start()
    {
        GameObject whiteBall = GameObject.FindGameObjectWithTag("WhiteBall");
        whiteballRb = whiteBall.GetComponent<Rigidbody>();
       
        startPosition = transform.position;
        if (powerupLight == null)
        {
            GameObject lightObj = new GameObject("light");
            lightObj.transform.parent = transform;
            lightObj.transform.localPosition = Vector3.zero;
            powerupLight = lightObj.AddComponent<Light>();
            powerupLight.type = LightType.Point;
            powerupLight.color = glowColor;
            powerupLight.range = 2f;
            powerupLight.intensity = 1.5f;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WhiteBall"))
        {
            TriggerFreezePowerup();
            // Optionally destroy the powerup
            Destroy(gameObject);
        }
    }
    
    private void TriggerFreezePowerup()
    {
        // Find all colored balls in the scene
        var coloredBalls = GameObject.FindGameObjectsWithTag("Ball")
            .Where(ball => ball.GetComponent<BallState>()?.IsNormal == true)
            .ToList();
                                   
        // Randomly select 3 balls to freeze
        int ballsToFreeze = Mathf.Min(3, coloredBalls.Count);
        var selectedBalls = coloredBalls.OrderBy(x => Random.value).Take(ballsToFreeze);
        
        foreach (var ball in selectedBalls)
        {
            var ballState = ball.GetComponent<BallState>();
            if (ballState != null)
            {
                ballState.FreezeBall(freezeMaterial, freezeDuration, frozenFriction);
            }
        }
    }

    void Update()
    {
        float ballSpeed = whiteballRb.velocity.magnitude;
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        float newY = startPosition.y + Mathf.Sin(Time.time * bobSpeed * 2 * Mathf.PI) * bobHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}