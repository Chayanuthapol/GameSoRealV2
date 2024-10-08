using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BallManager : MonoBehaviour
{
    public List<Balls2> balls; // รายการลูกบอลทั้งหมด
    public Transform ballsRemainingUIParent; // ตัว Parent ของ UI ที่จะแสดงลูกบอล
    public GameObject ballImagePrefab; // Prefab ของ UI รูปภาพสำหรับลูกบอลแต่ละลูก

    private List<GameObject> ballImages = new List<GameObject>(); // เก็บรายการของรูปภาพที่สร้างขึ้น

    void Start()
    {
        if (ballsRemainingUIParent != null && ballImagePrefab != null && balls != null)
        {
            // กำหนดระยะห่างระหว่างรูปภาพแต่ละลูกบอล (ปรับค่าตามที่ต้องการ)
            float spacing = 50f;  // ความห่างระหว่างแต่ละรูปภาพ
            Vector2 startPosition = new Vector2(-350, 0);  // จุดเริ่มต้นของการจัดเรียง (แกน X,Y)

            for (int i = 0; i < balls.Count; i++)
            {
                // สร้างรูปภาพของลูกบอล
                GameObject ballImage = Instantiate(ballImagePrefab, ballsRemainingUIParent);

                // เข้าถึง RectTransform ของรูปภาพ
                RectTransform rectTransform = ballImage.GetComponent<RectTransform>();

                // กำหนดตำแหน่งใหม่ให้รูปภาพ แต่ละลูกบอลจะถูกเลื่อนไปทางขวา (แกน X) ด้วย spacing
                rectTransform.anchoredPosition = new Vector2(startPosition.x + (i * spacing), startPosition.y);

                // เพิ่มรูปภาพลงใน List ถ้าจำเป็น
                ballImages.Add(ballImage);
            }
        }
    }

    void Update()
    {
        UpdateBallsRemainingUI();
    }

    void UpdateBallsRemainingUI()
    {
        // วนลูปเพื่อตรวจสอบสถานะของแต่ละลูกบอลและอัปเดต UI
        for (int i = 0; i < balls.Count; i++)
        {
            if (balls[i].isPocketed)
            {
                // ถ้าลูกบอลลงหลุมแล้ว ให้ซ่อนรูปภาพ
                ballImages[i].SetActive(false);
            }
            else
            {
                // ถ้ายังไม่ลงหลุม ให้แสดงรูปภาพ
                ballImages[i].SetActive(true);
                // ปรับให้แน่ใจว่ารูปภาพที่แสดงสอดคล้องกับลูกบอล (ถ้ามีรูปที่แตกต่าง)
                Image imageComponent = ballImages[i].GetComponent<Image>();
                imageComponent.sprite = balls[i].ballSprite; // สมมติว่าแต่ละลูกบอลมี Sprite ของตนเอง
            }
        }
    }
}
