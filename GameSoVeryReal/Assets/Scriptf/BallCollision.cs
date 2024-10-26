using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCollision : MonoBehaviour
{
    public AudioSource audioSource; // ตัวแปรเก็บ AudioSource

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // ตรวจสอบการชนกับ GameObject อื่น
        if (collision.gameObject.CompareTag("Ball"))
        {
            // เล่นเสียงเมื่อเกิดการชน
            audioSource.Play();
        }
    }
}
