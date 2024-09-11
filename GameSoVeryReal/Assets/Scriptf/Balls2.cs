using UnityEngine;

public class Balls2 : MonoBehaviour
{
    private BilliardsManager billiardsManager;
    public bool isPocketed = false; // สถานะลูกบอลว่าได้ถูกแทงลงหลุมหรือยัง

    private void Start()
    {
        // หา BilliardsManager เพื่อเรียกใช้เมื่อลูกบอลลงหลุม
        billiardsManager = FindObjectOfType<BilliardsManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pocket")) // ตรวจสอบว่าลูกบอลชนกับ Trigger ของหลุม
        {
            isPocketed = true;
        }
    }
}