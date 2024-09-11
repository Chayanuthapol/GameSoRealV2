using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    public List<Balls2> balls; // รายการลูกบอลทั้งหมด
    public TextMeshProUGUI ballsRemainingText; // UI Text ที่จะแสดงลูกบอลที่ยังไม่ได้แทงลงหลุม

    void Update()
    {
        UpdateBallsRemainingUI();
    }

    void UpdateBallsRemainingUI()
    {
        List<string> remainingBalls = new List<string>();
        
        foreach (Balls2 ball in balls)
        {
            if (!ball.isPocketed)
            {
                remainingBalls.Add(ball.gameObject.name); // สมมุติว่าใช้ชื่อของ GameObject เป็นตัวระบุ
            }
        }

        if (remainingBalls.Count > 0)
        {
            ballsRemainingText.text = string.Join(", ", remainingBalls);
        }
        else
        {
            ballsRemainingText.text = "All balls pocketed!";
        }
    }
}
