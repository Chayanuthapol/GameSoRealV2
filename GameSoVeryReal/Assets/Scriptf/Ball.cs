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
    public float ballStopThreshold ;
    private BilliardsManager billiardsManager;
    
    public ParticleSystem explosionEffect;  
    public float countdownTime = 3f;        
    private bool _isCountingDown = false;    
    private Renderer _ballRenderer;          
    private Collider _ballCollider;
    private float _triggerForce = 0.5f;
    private float _explosionRadius = 5;
    private float _explosionForce = 100;
    public float dragValue = 0.5f;  // ปรับค่า Drag ตามความเหมาะสม
    public float angularDragValue = 0.5f;  // ปรับค่า Angular Drag
    public bool isCueBall = true;  // ตรวจสอบว่าเป็นลูกบอลสีขาว
    private bool isMouseReleased = false; 

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        _ballRenderer = GetComponent<Renderer>();
        _ballCollider = GetComponent<Collider>();
        rb.drag = dragValue;
        rb.angularDrag = angularDragValue;

        // หา BilliardsManager เพื่อเรียกใช้เมื่อลูกบอลลงหลุม
        billiardsManager = FindObjectOfType<BilliardsManager>();
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
        float currentVelocity = rb.velocity.magnitude;
        Debug.Log("Current velocity: " + currentVelocity);
        
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
            //Debug.Log("Ball is moving");
            HideCue();
            
        }
        else if (!isMoving)
        {
            //Debug.Log("Ball has stopped");
            ShowCue();
            
        }
        
        
        // ตรวจสอบการปล่อยเมาส์ขวา
        if (Input.GetMouseButtonUp(0))
        {
            StartCoroutine(DelayMouseRelease()); // เริ่ม Coroutine สำหรับดีเลย์ 1 วินาที
        }

        // ตรวจสอบความเร็วของลูกบอลหลังจากปล่อยเมาส์ขวา
        if (isMouseReleased && rb.velocity.magnitude <= ballStopThreshold)
        {
            billiardsManager.EndTurn();
            isMouseReleased = false; // รีเซ็ตสถานะเมาส์เพื่อไม่ให้ฟังก์ชันถูกเรียกซ้ำ
        }
        //Debug.Log(rb.velocity.magnitude);
        Debug.Log(isMouseReleased);
    }

    
    // Coroutine ที่จะดีเลย์ 1 วินาทีก่อนตั้งค่า isMouseReleased = true;
    IEnumerator DelayMouseRelease()
    {
        yield return new WaitForSeconds(1f); // รอ 1 วินาที
        isMouseReleased = true;
    }
    
    
    // Apply force to hit the ball
    public void Hit(Vector3 force)
    {
        
        rb.AddForce(force, ForceMode.Impulse);
    }

    public bool IsBallMoving()
    {
        float velocityMagnitude = rb.velocity.magnitude;
        
        return rb.velocity.magnitude > ballStopThreshold;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pocket"))
        {
            // ตรวจสอบว่าลูกบอลสีขาวตกลงหลุมหรือไม่
            if (isCueBall)
            {
                // หยุดการเคลื่อนที่ของลูกบอลสีขาว
                StopBall();

                // แจ้งไปที่ BilliardsManager ให้ทำการ spawn ลูกบอลสีขาวใหม่
                Vector3 pocketPosition = other.transform.position;
                billiardsManager.BallPocketed(gameObject, pocketPosition);
            }
            else
            {
                // ตรวจสอบว่าลูกบอลนี้อยู่ในรายการที่อนุญาตให้ชนหรือไม่
                if (billiardsManager.IsBallAllowed(gameObject))
                {
                    // แจ้งไปที่ BilliardsManager ว่าลูกบอลนี้ได้ลงหลุม พร้อมกับส่งตำแหน่งของหลุม
                    Vector3 pocketPosition = other.transform.position;
                    billiardsManager.BallPocketed(gameObject, pocketPosition);
                }
            }
        }
    }

    private void StopBall()
    {
        // หยุดการเคลื่อนที่ของลูกบอลสีขาวโดยการตั้งค่า velocity และ angularVelocity เป็น 0
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
