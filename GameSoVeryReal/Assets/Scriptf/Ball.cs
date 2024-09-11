using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Ball : MonoBehaviour
{
    public Rigidbody rb;
    public bool isMoving;
    public Transform cue;                  // ตัวไม้คิว
    public Ball ball;                      // ลูกบอล
    public LineRenderer aimLineRenderer; 
    public float ballStopThreshold = 0.2f;
    
    public ParticleSystem explosionEffect;  
    public float countdownTime = 3f;        
    private bool _isCountingDown = false;    
    private Renderer _ballRenderer;          
    private Collider _ballCollider;
    private float _triggerForce = 0.5f;
    private float _explosionRadius = 5;
    private float _explosionForce = 100;
        

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        _ballRenderer = GetComponent<Renderer>();
        _ballCollider = GetComponent<Collider>();
        // หา BilliardsManager เพื่อเรียกใช้เมื่อลูกบอลลงหลุม
        
    }
    void OnCollisionEnter(Collision collision)
    {
        // Check if the collision object is a sphere (or has a tag "Sphere")
        if (collision.gameObject.CompareTag("Items") && !_isCountingDown)
        {
            // Make the sphere disappear
            collision.gameObject.SetActive(false);
           
            // Start the countdown and explosion routine
            StartCoroutine(CountdownAndExplode());
        }
    }
    IEnumerator CountdownAndExplode()
    {
        _isCountingDown = true;
        var surroundingObjects = Physics.OverlapSphere(transform.position, _explosionRadius);

        // Countdown from 3 to 1
        for (int i = (int)countdownTime; i > 0; i--)
        {
            Debug.Log(i);  
            yield return new WaitForSeconds(1f);  
        }

        foreach (var obj in surroundingObjects)
        {
            var rb = obj.GetComponent<Rigidbody>();
            if (rb == null) continue;
            rb.AddExplosionForce(_explosionForce, transform.position, _explosionRadius);
        }
        Instantiate(explosionEffect, ball.transform.position, ball.transform.rotation);
        explosionEffect.Play();
        
        
    }
    private void Update()
    {
        // Check if the ball is moving
        isMoving = rb.velocity.magnitude > 0.1f;
        if (rb.velocity.magnitude > ballStopThreshold)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        if (isMoving)
        {
            HideCue();
        }
        else if (!isMoving)
        {
            ShowCue();
        }
        //Debug.Log(rb.velocity.magnitude);
    }

    // Apply force to hit the ball
    public void Hit(Vector3 force)
    {
        rb.AddForce(force, ForceMode.Impulse);
        
    }

    private void HideCue()
    {
        cue.gameObject.SetActive(false); // ซ่อนไม้คิว
        aimLineRenderer.enabled = false;
    }

    private void ShowCue()
    {
        cue.gameObject.SetActive(true); // แสดงไม้คิว
        aimLineRenderer.enabled = true; // เปิดใช้งานเส้นทิศทางอีกครั้ง
    }
}

