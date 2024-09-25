using UnityEngine;

public class Balls : MonoBehaviour
{
    private BilliardsManager billiardsManager;
    public Rigidbody rb;
    public float dragValue = 0.5f;  // ปรับค่า Drag ตามความเหมาะสม
    public float angularDragValue = 0.5f;  // ปรับค่า Angular Drag
  
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.drag = dragValue;
        rb.angularDrag = angularDragValue;
        // หา BilliardsManager เพื่อเรียกใช้เมื่อลูกบอลลงหลุม
        billiardsManager = FindObjectOfType<BilliardsManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pocket"))
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