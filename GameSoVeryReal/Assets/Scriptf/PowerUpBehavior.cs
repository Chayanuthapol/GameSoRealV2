using UnityEngine;

public class PowerUpBehavior : MonoBehaviour
{
    public float rotationSpeed = 90f;
    public float bobSpeed = 1f;
    public float bobHeight = 0.1f;
    private Vector3 startPosition;
    
    [Header("Glow Effect")]
    public Color glowColor = new Color(1, 1, 1, 1);
    public Light powerupLight;
    public Rigidbody whiteballRb;
    public float ballMovingThreshold = 0.01f; // Minimum speed for white ball to activate power-up
    public float respawnDelay = 1f; // Delay before respawning power-up

    private bool isActivated = false;

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

    void Update()
    {
        float ballSpeed = whiteballRb.velocity.magnitude;

        // Rotate and bob the power-up
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        float newY = startPosition.y + Mathf.Sin(Time.time * bobSpeed * 2 * Mathf.PI) * bobHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        // Check if white ball is moving and activate power-up if needed
        if (ballSpeed >= ballMovingThreshold && !isActivated)
        {
            isActivated = true;
        }

        // Check if white ball has stopped and respawn power-up if needed
        if (isActivated && ballSpeed <= ballMovingThreshold)
        {
            Destroy(gameObject);
            isActivated = false;
           
        }
    }
}