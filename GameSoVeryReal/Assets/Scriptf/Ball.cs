using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("Freeze Power-up")]
    public Material frozenMaterial; 
    public ParticleSystem FreezeEffect;
    private List<Ball> frozenBalls = new List<Ball>();
    private Material originalMaterial;
    private bool isFrozen = false;
    private Rigidbody originalRigidbody;
    
    public Rigidbody rb;
    public bool isMoving;
    public Transform cue;                  // ตัวไม้คิว
    public Ball ball;                      // ลูกบอล
    public LineRenderer aimLineRenderer;
    public float ballStopThreshold = 0.2f;
    private BilliardsManager billiardsManager;
    public PhysicMaterial whiteball;
    public ParticleSystem explosionEffect;  
    public ParticleSystem SpeedEffect; 
    public ParticleSystem BounceEffect; 
    public ParticleSystem HeavyEffect;
    public ParticleSystem TornadoEffect;
    public float whiteballBounce = 1/2;
    public float countdownTime = 3f;
    public float countSpeed = 3f;
    public float countHeavy = 3f;  
    public float countBouncy = 3f;
    public float countTornado = 5f;
    public float countSlip = 3f;
    public float countSticky = 3f;
    private bool _isCountingDown = false;
    private bool isCountSpeed = false;
    private bool isCountHeavy = false;  
    private bool isCountBouncy = false;
    private bool isCountTornado = false;
    private bool isCountSlip = false;
    private bool isCountSticky = false;
    private Renderer _ballRenderer;          
    private Collider _ballCollider;
    private float _triggerForce = 0.5f;
    private float _explosionRadius = 5;
    private float _explosionForce = 100;
    public float dragValue = 0.5f;  // ปรับค่า Drag ตามความเหมาะสม
    public float angularDragValue = 0.5f;  // ปรับค่า Angular Drag
    public bool isCueBall = true;  // ตรวจสอบว่าเป็นลูกบอลสีขาว
    private bool isMouseReleased = false; 
    
    private Dictionary<string, ParticleSystem> activeEffects = new Dictionary<string, ParticleSystem>();

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        _ballRenderer = GetComponent<Renderer>();
        _ballCollider = GetComponent<Collider>();
        rb.drag = dragValue;
        rb.angularDrag = angularDragValue;
        originalMaterial = _ballRenderer.material;
        originalRigidbody = rb;

        // หา BilliardsManager เพื่อเรียกใช้เมื่อลูกบอลลงหลุม
        billiardsManager = FindObjectOfType<BilliardsManager>();
    }

    

    IEnumerator CountdownAndExplode()
    {
        _isCountingDown = true;
        var surroundingObjects = Physics.OverlapSphere(transform.position, _explosionRadius);

        // Countdown from 3 to 1
        for (int i = (int)countdownTime; i > 0; i--)
        {
            Debug.Log(i);  
            yield return new WaitForSeconds(1f);  
        }

        foreach (var obj in surroundingObjects)
        {
            var rb = obj.GetComponent<Rigidbody>();
            if (rb == null) continue;
            rb.AddExplosionForce(_explosionForce, transform.position, _explosionRadius);
        }
        if (explosionEffect != null)
        {
            ParticleSystem explosion = Instantiate(explosionEffect);
            explosion.transform.position = transform.position;
            explosion.Play();

            // Wait for particle system to finish
            yield return new WaitForSeconds(explosion.main.duration);
            Destroy(explosion.gameObject);
        }
       
    }

    IEnumerator IShowSpeed()
    {
        isCountSpeed = true;
        float originalSpeed = rb.velocity.magnitude;
        rb.velocity *= 1.5f;
        if (SpeedEffect != null)
        {
            ParticleSystem speedEffect = Instantiate(SpeedEffect);
            speedEffect.transform.position = ball.transform.position;
            speedEffect.transform.rotation = ball.transform.rotation;
            activeEffects["Speed"] = speedEffect;
            speedEffect.Play();

            // Duration of speed effect
            for (int i = (int)countSpeed; i > 0; i--)
            {
                yield return new WaitForSeconds(1f);
            }

            rb.velocity = originalSpeed * rb.velocity.normalized;

            // Cleanup
            if (speedEffect != null)
            {
                Destroy(speedEffect.gameObject);
                activeEffects.Remove("Speed");
            }
        }
        isCountSpeed = false;
    }
    

    IEnumerator HeavyBall()
    {
        isCountHeavy = true;
        float originalMass = rb.mass;
        rb.mass *= 10000;
        
        if (HeavyEffect != null)
        {
            ParticleSystem heavyEffect = Instantiate(HeavyEffect);
            heavyEffect.transform.position = transform.position;
            heavyEffect.transform.rotation = transform.rotation;
            activeEffects["Heavy"] = heavyEffect;
            heavyEffect.Play();

            for (int i = (int)countHeavy; i > 0; i--)
            {
                yield return new WaitForSeconds(1f);
            }

            rb.mass = originalMass;

            // Cleanup
            if (heavyEffect != null)
            {
                Destroy(heavyEffect.gameObject);
                activeEffects.Remove("Heavy");
            }
        }

        isCountHeavy = false;
      
    }
    IEnumerator FireTornado()
    {
        isCountTornado = true;
        if (TornadoEffect != null)
        {
            ParticleSystem tornadoEffect = Instantiate(TornadoEffect);
            tornadoEffect.transform.position = transform.position;
            
            activeEffects["Tornado"] = tornadoEffect;
            tornadoEffect.Play();

            for (int i = (int)countTornado; i > 0; i--)
            {
                yield return new WaitForSeconds(1f);
            }

            // Cleanup
            if (tornadoEffect != null)
            {
                Destroy(tornadoEffect.gameObject);
                activeEffects.Remove("Tornado");
            }
        }

        isCountTornado = false;
        
    }
    IEnumerator FreezeRandomBalls()
    {
        Ball[] allBalls = FindObjectsOfType<Ball>();
        List<Ball> coloredBalls = new List<Ball>();
        foreach (Ball b in allBalls)
        {
            if (!b.isCueBall && !b.isFrozen)
            {
                coloredBalls.Add(b);
            }
        }
        
        int ballsToFreeze = Mathf.Min(3, coloredBalls.Count);
        for (int i = 0; i < ballsToFreeze; i++)
        {
            int randomIndex = Random.Range(0, coloredBalls.Count);
            Ball selectedBall = coloredBalls[randomIndex];
            coloredBalls.RemoveAt(randomIndex);
            frozenBalls.Add(selectedBall);
            StartCoroutine(FreezeBall(selectedBall));
        }
        yield return new WaitForSeconds(5f);
        foreach (Ball frozenBall in frozenBalls)
        {
            if (frozenBall != null)
            {
                UnfreezeBall(frozenBall);
            }
        }
        
        frozenBalls.Clear();
    }
    IEnumerator FreezeBall(Ball ball)
    {
        Vector3 originalVelocity = ball.rb.velocity;
        Vector3 originalAngularVelocity = ball.rb.angularVelocity;
        ball.isFrozen = true;
        ball._ballRenderer.material = frozenMaterial;
        ParticleSystem freezeVFX = Instantiate(FreezeEffect, ball.transform.position, Quaternion.identity);
        freezeVFX.transform.parent = ball.transform;
        freezeVFX.Play();
        ball.rb.velocity = Vector3.zero;
        ball.rb.angularVelocity = Vector3.zero;
        ball.rb.constraints = RigidbodyConstraints.FreezeAll;
        yield return new WaitForSeconds(5f);
        if (ball != null)
        {
            UnfreezeBall(ball);
        }

        if (freezeVFX != null)
        {
            Destroy(freezeVFX.gameObject);
        }
    }
    private void UnfreezeBall(Ball ball)
    {
        ball._ballRenderer.material = ball.originalMaterial;
        ball.rb.constraints = RigidbodyConstraints.None;
        ball.isFrozen = false;
    }

    

    // IEnumerator BouncyBall()
    // {
    //     isCountBouncy = true;
    //     Collider ballCollider = GetComponent<Collider>();
    //     whiteball = ballCollider.sharedMaterial;
    //     whiteball.bounciness = 1/2;
    //     Instantiate(BounceEffect, ball.transform.position, ball.transform.rotation);
    //     BounceEffect.Play();
    //     for (int i = (int)countBouncy; i > 0; i--)
    //     {
    //         yield return new WaitForSeconds(1f);
    //     } 
    //     whiteball.bounciness = whiteballBounce;
    //     yield return new WaitForSeconds(BounceEffect.main.duration);
    //     isCountBouncy = false;
    // }

    private void Update()
    {
        if (!isFrozen)
        {
            float currentVelocity = rb.velocity.magnitude;
            Debug.Log("Current velocity: " + currentVelocity);
            
            if (rb.velocity.magnitude > ballStopThreshold)
            {
                isMoving = true;
            }
            else
            {
                isMoving = false;
            }

            if (isMoving)
            {
                HideCue();
            }
            else if (!isMoving)
            {
                ShowCue();
            }
        }
        
        // ตรวจสอบการปล่อยเมาส์ขวา
        if (Input.GetMouseButtonUp(0))
        {
            StartCoroutine(DelayMouseRelease()); // เริ่ม Coroutine สำหรับดีเลย์ 1 วินาที
        }
            
        // ตรวจสอบความเร็วของลูกบอลหลังจากปล่อยเมาส์ขวา
        if (isMouseReleased && rb.velocity.magnitude <= ballStopThreshold)
        {
            billiardsManager.EndTurn();
            isMouseReleased = false; // รีเซ็ตสถานะเมาส์เพื่อไม่ให้ฟังก์ชันถูกเรียกซ้ำ
        }
        //Debug.Log(rb.velocity.magnitude);
        Debug.Log(isMouseReleased);
        
    }
    
    // Coroutine ที่จะดีเลย์ 1 วินาทีก่อนตั้งค่า isMouseReleased = true;
    IEnumerator DelayMouseRelease()
    {
        yield return new WaitForSeconds(1f); // รอ 1 วินาที
        isMouseReleased = true;
    }
    
    
    // Apply force to hit the ball
    public void Hit(Vector3 force)
    {
        
        rb.AddForce(force, ForceMode.Impulse);
    }

    public bool IsBallMoving()
    {
        float velocityMagnitude = rb.velocity.magnitude;
        
        return rb.velocity.magnitude > ballStopThreshold;
    }
    
    private void HideCue()
    {
        cue.gameObject.SetActive(false); // ซ่อนไม้คิว
        aimLineRenderer.enabled = false;
    }

    private void ShowCue()
    {
        cue.gameObject.SetActive(true); // แสดงไม้คิว
        aimLineRenderer.enabled = true; // เปิดใช้งานเส้นทิศทางอีกครั้ง
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pocket"))
        {
            // ตรวจสอบว่าลูกบอลสีขาวตกลงหลุมหรือไม่
            if (isCueBall)
            {
                // หยุดการเคลื่อนที่ของลูกบอลสีขาว
                StopBall();

                // แจ้งไปที่ BilliardsManager ให้ทำการ spawn ลูกบอลสีขาวใหม่
                Vector3 pocketPosition = other.transform.position;
                billiardsManager.BallPocketed(gameObject, pocketPosition);
            }
            else
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
        if (other.CompareTag("Bomb") && !_isCountingDown)
        {
            other.gameObject.SetActive(false);
            StartCoroutine(CountdownAndExplode());
        }
        if (other.CompareTag("Speed"))
        {
            other.gameObject.SetActive(false);
            StartCoroutine(IShowSpeed());
        }
        if (other.CompareTag("Heavy"))
        {
            other.gameObject.SetActive(false);
            StartCoroutine(HeavyBall());
        }
        if (other.CompareTag("Freeze") && isCueBall)
        {
            other.gameObject.SetActive(false);
            StartCoroutine(FreezeRandomBalls());
        }

        if (other.CompareTag("Tornado"))
        {
            other.gameObject.SetActive(false);
            StartCoroutine(FireTornado());
        }
        // if (other.CompareTag("Bouncy"))
        // {
        //     other.gameObject.SetActive(false);
        //     StartCoroutine(BouncyBall());
        // }
    }

    private void StopBall()
    {
        // หยุดการเคลื่อนที่ของลูกบอลสีขาวโดยการตั้งค่า velocity และ angularVelocity เป็น 0
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
