using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic; // สำหรับใช้ List
using TMPro;
using Unity.VisualScripting; // สำหรับ TextMeshPro

public class BilliardsManager2 : MonoBehaviour
{
    public List<GameObject> allowedBalls; // รายการลูกบอลที่สามารถชนได้และนับคะแนน
    public int[] playerScores = { 0, 0 }; // คะแนนของผู้เล่น
    private int currentPlayer = 0; // ผู้เล่นปัจจุบัน (0 หรือ 1)
    private Ball Ball;
    public static BilliardsManager2 Instance;

    public Transform image, image2,image3,image4;
    // อ้างอิง UI
    public TextMeshProUGUI playerScoreText1; // Text แสดงคะแนนผู้เล่น 1
    public TextMeshProUGUI playerScoreText2; // Text แสดงคะแนนผู้เล่น 2
    public TextMeshProUGUI turnText;         // Text แสดงเทิร์นของผู้เล่นปัจจุบัน

    // Particle System
    public ParticleSystem pocketParticle;    // พาร์ติเคิลที่จะแสดงเมื่อมีลูกบอลตกลงหลุม
    public float particleDuration = 2f;      // ระยะเวลาที่พาร์ติเคิลจะแสดง
    
    // Cue Ball and Respawn Point
    public GameObject cueBallPrefab;         // Prefab ของลูกบอลสีขาว
    public Transform cueBallSpawnPoint;      // จุดที่ใช้ spawn ลูกบอลสีขาว
    

    private void Start()
    {
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

            if (currentPlayer == 0)
            {
                image.gameObject.SetActive(true);
                image2.gameObject.SetActive(false);
            }
            else
            {
                image.gameObject.SetActive(false);
                image2.gameObject.SetActive(true);
            }
            
            if (currentPlayer == 1)
            {
                image3.gameObject.SetActive(false);
                image4.gameObject.SetActive(true);
            }
            else
            {
                image3.gameObject.SetActive(true);
                image4.gameObject.SetActive(false);
            }
            
            
        
    }

    private void UpdateUI()
    {
        // อัปเดต Text ของคะแนนผู้เล่น
        playerScoreText1.text = "Player 1: " + playerScores[0];
        playerScoreText2.text = "Player 2: " + playerScores[1];
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
}
