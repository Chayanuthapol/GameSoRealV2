using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class FeedbackManager : MonoBehaviour
{
    public TextMeshProUGUI feedbackText; // ลิงก์ไปยัง Text ใน UI
    private Color originalColor; // เก็บสีเดิมของข้อความ

    void Start()
    {
        originalColor = feedbackText.color; // เก็บสีเดิมของข้อความ (รวมถึงค่า alpha)
        feedbackText.text = ""; // ตั้งค่าให้ข้อความเริ่มเป็นว่างเปล่า
    }

    public void ShowFeedback(string message)
    {
        feedbackText.text = message; // ตั้งค่าข้อความที่จะแสดง
        feedbackText.color = originalColor; // รีเซ็ตสีข้อความ (กรณีที่ข้อความก่อนหน้ากลายเป็นโปร่งใส)
        StartCoroutine(FadeOutText(2f)); // เรียกฟังก์ชันทำให้ข้อความหายไปใน 2 วินาที
    }

    IEnumerator FadeOutText(float duration)
    {
        float elapsedTime = 0f;
        Color currentColor = feedbackText.color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration); // ค่อยๆ ลดค่า alpha
            feedbackText.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha); // ปรับค่า alpha ของสี
            yield return null;
        }

        feedbackText.text = ""; // เมื่อข้อความหายไปแล้ว ตั้งค่าให้เป็นว่างเปล่าอีกครั้ง
    }
}
