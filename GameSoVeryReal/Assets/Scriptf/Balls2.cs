using UnityEngine;

public class Balls2 :  MonoBehaviour
{
    private BilliardsManager billiardsManager;
    public Sprite ballSprite;
    public bool isPocketed = false; // สถานะลูกบอลว่าได้ถูกแทงลงหลุมหรือยัง
    public FeedbackManager feedbackManager; // ลิงก์ไปยัง FeedbackManager
    public GameObject audioSourceObject; // อ้างอิงไปยัง GameObject ที่มี AudioSource
    private AudioSource audioSource;  // ตัวแปรเก็บ AudioSource
   


    private void Start()
    {
        // หา BilliardsManager เพื่อเรียกใช้เมื่อลูกบอลลงหลุม
        billiardsManager = FindObjectOfType<BilliardsManager>();
        audioSource = audioSourceObject.GetComponent<AudioSource>(); // ดึง AudioSource จาก GameObject ที่กำหนด
        if (audioSource == null)
        {
            Debug.LogError("AudioSource is missing! Please add an AudioSource to the object.");
        }

       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pocket")) // ตรวจสอบว่าลูกบอลชนกับ Trigger ของหลุม
        {
            isPocketed = true;
            if (audioSource != null && audioSource.clip != null)
            {
                audioSource.Play(); // เล่นเสียงเมื่อบอลลงหลุม
            }
            else
            {
                Debug.LogError("AudioSource or Audio Clip is not assigned!");
            }

            feedbackManager.ShowFeedback("You scored! +1 Point"); // แสดงข้อความ Feedback ว่าได้คะแนน
            
            
        }
    }
    
}