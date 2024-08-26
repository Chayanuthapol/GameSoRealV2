using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Ball : MonoBehaviour
{
    public Rigidbody rb;
    public bool isMoving;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Check if the ball is moving
        isMoving = rb.velocity.magnitude > 0.1f;
        if (!isMoving)
        {
            Debug.Log("not move");
        }
    }

    // Apply force to hit the ball
    public void Hit(Vector3 force)
    {
        rb.AddForce(force, ForceMode.Impulse);
        Debug.Log(isMoving);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (rb.detectCollisions)
        //{
          //  Hit(Vector3.forward);
        //}
    }
}

