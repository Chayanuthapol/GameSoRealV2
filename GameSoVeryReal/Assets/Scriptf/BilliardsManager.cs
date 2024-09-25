using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic; // สำหรับใช้ List
using TMPro; // สำหรับ TextMeshPro

public class BilliardsManager : MonoBehaviour
{
    public List<GameObject> allowedBalls; // รายการลูกบอลที่สามารถชนได้และนับคะแนน
    public int[] playerScores = { 0, 0 }; // คะแนนของผู้เล่น
    private int currentPlayer = 0; // ผู้เล่นปัจจุบัน (0 หรือ 1)
    private Ball Ball;
    public static BilliardsManager Instance;

    // อ้างอิง UI
    public TextMeshProUGUI playerScoreText1; // Text แสดงคะแนนผู้เล่น 1
    public TextMeshProUGUI playerScoreText2; // Text แสดงคะแนนผู้เล่น 2
    public TextMeshProUGUI turnText;         // Text แสดงเทิร์นของผู้เล่นปัจจุบัน
    public TextMeshProUGUI winText; // Text สำหรับแสดงผลว่าใครชนะ

    // Particle System
    public ParticleSystem pocketParticle;    // พาร์ติเคิลที่จะแสดงเมื่อมีลูกบอลตกลงหลุม
    public float particleDuration = 2f;      // ระยะเวลาที่พาร์ติเคิลจะแสดง
    
    // Cue Ball and Respawn Point
    public GameObject cueBallPrefab;         // Prefab ของลูกบอลสีขาว
    public Transform cueBallSpawnPoint;      // จุดที่ใช้ spawn ลูกบอลสีขาว

    private void Start()
    {
        
        // ซ่อน winText เมื่อเริ่มเกม
        winText.gameObject.SetActive(false);
        
        // อัปเดต UI เริ่มต้น
        UpdateUI();
        PlayerUI();
    }

    public bool IsBallAllowed(GameObject ball)
    {
        // ตรวจสอบว่าลูกบอลอยู่ในรายการ allowedBalls หรือไม่
        return allowedBalls.Contains(ball);
    }

    public void BallPocketed(GameObject ball, Vector3 pocketPosition)
    {
        // ตรวจสอบว่าลูกบอลที่ตกเป็นลูกบอลสีขาวหรือไม่
        Ball ballScript = ball.GetComponent<Ball>(); // สมมติว่ามีสคริปต์ Ball อยู่

        if (ballScript != null && ballScript.isCueBall)
        {
            // ถ้าเป็นลูกบอลสีขาว ให้ทำการย้ายลูกบอลไปที่จุด spawn
            Debug.Log("Cue ball pocketed! Respawning at designated point.");
            RespawnCueBall();
        }
        else
        {
            // เพิ่มคะแนนให้ผู้เล่นปัจจุบัน
            playerScores[currentPlayer]++;
            Debug.Log("Player " + (currentPlayer + 1) + " scored! Current Score: " + playerScores[currentPlayer]);

            // ซ่อนลูกบอลที่ลงหลุม
            ball.SetActive(false);

            // เรียกใช้พาร์ติเคิลที่ตำแหน่งของหลุม
            ShowPocketParticle(pocketPosition);

            // อัปเดต UI หลังจากมีการเพิ่มคะแนน
            UpdateUI();
        }
    }

    private void RespawnCueBall()
    {
        // ย้ายลูกบอลสีขาวไปยังตำแหน่งที่กำหนดโดย GameObject cueBallSpawnPoint
        cueBallPrefab.transform.position = cueBallSpawnPoint.position;
        cueBallPrefab.SetActive(true); // แสดงลูกบอลสีขาวใหม่
    }

    private void ShowPocketParticle(Vector3 position)
    {
        // ย้ายพาร์ติเคิลไปที่ตำแหน่งของหลุม
        pocketParticle.transform.position = position;

        // เริ่มการแสดงพาร์ติเคิล
        pocketParticle.Play();

        // หยุดพาร์ติเคิลหลังจากระยะเวลาที่กำหนด
        Invoke("StopPocketParticle", particleDuration);
    }

    private void StopPocketParticle()
    {
        // หยุดการทำงานของพาร์ติเคิล
        pocketParticle.Stop();
    }

    public void EndTurn(bool foul = false)
    {
            // เปลี่ยนเทิร์นปกติ
            currentPlayer = (currentPlayer + 1) % 2;
            Debug.Log("Switching to Player " + (currentPlayer + 1));
            
            // อัปเดต UI เมื่อเปลี่ยนเทิร์น
            UpdateUI();
            PlayerUI();
        
    }

    private void UpdateUI()
    {
        // อัปเดต Text ของคะแนนผู้เล่น
        playerScoreText1.text = "Player 1: " + playerScores[0];
        playerScoreText2.text = "Player 2: " + playerScores[1];

        // ตรวจสอบว่าผู้เล่นคนใดมีคะแนนครบ 8
        if (playerScores[0] >= 8)
        {
            ShowWinMessage(1); // ผู้เล่น 1 ชนะ
        }
        else if (playerScores[1] >= 8)
        {
            ShowWinMessage(2); // ผู้เล่น 2 ชนะ
        }
    }

    private void PlayerUI()
    {
        // อัปเดต Text ของเทิร์นปัจจุบัน
        turnText.text = "Current Turn: Player " + (currentPlayer + 1);
    }
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    
    private void ShowWinMessage(int playerNumber)
    {
        // แสดงผลข้อความว่าใครชนะ
        winText.text = "Player " + playerNumber + " Wins!";
        winText.gameObject.SetActive(true); // เปิดการแสดงผลข้อความ

        // หยุดเกม (อาจใช้วิธีปิดการควบคุมต่าง ๆ หรือหยุดการทำงานอื่น ๆ)
        Time.timeScale = 0f; // หยุดเวลาของเกม
    }
}
