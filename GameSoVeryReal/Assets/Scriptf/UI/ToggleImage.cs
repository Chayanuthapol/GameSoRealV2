using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleImage : MonoBehaviour
{
    public Image targetImage; // ลาก Image ที่ต้องการมาใส่ใน Inspector
    public Button closeButton; // ลากปุ่มปิดมาใส่ใน Inspector


    private bool isVisible = false; // สถานะการแสดงผลของ Image

    // ฟังก์ชันนี้จะถูกเรียกเมื่อกดปุ่ม
    public void ToggleVisibility()
    {
        isVisible = true; // สลับสถานะ
        targetImage.gameObject.SetActive(true); // เปิดหรือปิด Image ตามสถานะ
        closeButton.gameObject.SetActive(true); // ปิดปุ่มปิด
    }
    public void CloseImage()
    {
        isVisible = false; // ตั้งค่าสถานะให้เป็นปิด
        targetImage.gameObject.SetActive(false); // ปิด Image
        closeButton.gameObject.SetActive(false); // ปิดปุ่มปิด
    }
}
