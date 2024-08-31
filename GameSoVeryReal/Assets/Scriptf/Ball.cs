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
    public float ballStopThreshold = 0.5f;// LineRenderer สำหรับเส้นทิศทาง
    

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        // หา BilliardsManager เพื่อเรียกใช้เมื่อลูกบอลลงหลุม
        
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

