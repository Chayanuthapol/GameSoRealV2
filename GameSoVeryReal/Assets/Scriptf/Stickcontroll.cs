using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StickController : MonoBehaviour
{
    public Transform cue; // ตัวไม้คิว
    public Ball ball; // ลูกบอล
    public Camera mainCamera; // กล้องหลัก
    public float followDistance; //ระยะกล้อง
    public Slider powerSlider; // Slider สำหรับปรับค่าแรง
    public TextMeshProUGUI powerText; // Text สำหรับแสดงค่าความแรง
    public LineRenderer aimLineRenderer; // LineRenderer สำหรับเส้นทิศทาง
    public float maxAimDistance = 5f; // ระยะสูงสุดของเส้นทิศทาง


    private Vector3 hitDirection;
    private bool isBallMoving = false;

    private void Update()
    {
        if (ball != null)
        {
            if (!isBallMoving)
            {
                FollowBall();
                AimCue();
                DrawAimLine();

                if (Input.GetMouseButtonDown(0))
                {
                    HitBall();
                }

                // อัปเดตค่าความแรงที่แสดงใน UI Text (ถ้ามี)
                if (powerText != null)
                {
                    powerText.text = "Power: " + powerSlider.value.ToString("0.0");
                }
            }



        }
    }

    private void FollowBall()
    {
        Vector3 direction = (cue.position - ball.transform.position).normalized;
        cue.position = ball.transform.position - direction * followDistance;
    }

    private void AimCue()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            hitDirection = (ball.transform.position - hit.point).normalized;
            cue.position = ball.transform.position - hitDirection * followDistance;
            cue.LookAt(ball.transform);
        }
    }

    private void DrawAimLine()
    {
        aimLineRenderer.SetPosition(0, ball.transform.position);
        Vector3 aimEndPosition = ball.transform.position + hitDirection * maxAimDistance;
        aimLineRenderer.SetPosition(1, aimEndPosition);
        aimLineRenderer.enabled = true;
    }

    private void HitBall()
    {
        // ตีลูกบอลไปในทิศทางที่ถูกเล็ง พร้อมกับแรงที่ปรับจาก Slider 
        ball.Hit(hitDirection * powerSlider.value);
    }


}
