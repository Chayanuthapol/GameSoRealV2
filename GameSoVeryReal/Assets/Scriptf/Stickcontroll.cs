using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StickController : MonoBehaviour
{
    public Transform cue; // ตัวไม้คิว
  
    public Camera mainCamera; // กล้องหลัก
    public float followDistance; // ระยะกล้อง
    public Slider powerSlider; // Slider สำหรับปรับค่าแรง
    public TextMeshProUGUI powerText; // Text สำหรับแสดงค่าความแรง
    public LineRenderer aimLineRenderer; // LineRenderer สำหรับเส้นทิศทาง
    public float maxAimDistance = 5f; // ระยะสูงสุดของเส้นทิศทาง
    public float powerChargeSpeed = 1f; // ความเร็วในการเพิ่มค่าความแรงของ Slider
    public ParticleSystem powerParticle; // พาร์ติเคิลที่จะแสดงเมื่อความแรงมากกว่า 0.7
    private BilliardsManager BilliardsManager;
    public Ball ball; // ลูกบอล
    public Ball cueBall;
    
    private Vector3 hitDirection;
    private bool isBallMoving = false;
    private bool isChargingPower = false; // กำลังชาร์จค่าความแรงหรือไม่


    private void Start()
    {
        
    }


    private void Update()
    {
        
        if (!isBallMoving)
        {
            FollowBall();
            AimCue();
            DrawAimLine();

            // เริ่มการชาร์จค่าความแรงเมื่อกดเมาส์ค้าง
            if (Input.GetMouseButton(0))
            {
                isChargingPower = true;
                ChargePower();
            }

            // เมื่อต้องการหยุดชาร์จและตีลูกบอล (เมื่อปล่อยเมาส์ซ้าย)
            if (Input.GetMouseButtonUp(0))
            {
                HitBall();
                ResetPower();
                isChargingPower = false;
                powerParticle.Stop(); // หยุดพาร์ติเคิลหลังจากตีลูกบอล
                // ตรวจสอบความเร็วของลูกบอล
                
            }

            // อัปเดตค่าความแรงที่แสดงใน UI Text
            if (powerText != null)
            {
                powerText.text = "Power: " + powerSlider.value.ToString("0.0");
            }

            // แสดงพาร์ติเคิลเมื่อค่า Power มากกว่า 0.7
            if (powerSlider.value > 0.7f && !powerParticle.isPlaying)
            {
                powerParticle.Play(); // เปิดใช้งานพาร์ติเคิล
            }
            else if (powerSlider.value <= 0.7f && powerParticle.isPlaying)
            {
                powerParticle.Stop(); // หยุดพาร์ติเคิลเมื่อค่า Power ต่ำกว่า 0.7
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

    private void ChargePower()
    {
        // ค่อยๆ เพิ่มค่าใน Slider ตามความเร็วการชาร์จ
        if (isChargingPower && powerSlider.value < powerSlider.maxValue)
        {
            powerSlider.value += powerChargeSpeed * Time.deltaTime;
        }
    }

    private void HitBall()
    {
        // ตีลูกบอลไปในทิศทางที่ถูกเล็ง พร้อมกับแรงที่ปรับจาก Slider
        ball.Hit(hitDirection * powerSlider.value);
        Debug.Log("Hit");
        Debug.Log(cueBall.rb.velocity.magnitude);
    }

    private void ResetPower()
    {
        // รีเซ็ตค่าแรงใน Slider กลับไปที่ 0 หลังจากการตีลูก
        powerSlider.value = powerSlider.minValue;
    }
}
