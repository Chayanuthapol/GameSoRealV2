using UnityEngine;
public class Balls : MonoBehaviour
{
    private BilliardsManager billiardsManager;
    public Rigidbody rb;
    public float dragValue = 0.5f;
    public float angularDragValue = 0.5f;
    
    // Freeze properties
    private bool isFrozen = false;
    private float freezeDuration = 5f;
    private float freezeTimer = 0f;
    private Vector3 frozenPosition;
    private Material originalMaterial;
    public Material frozenMaterial;
    private MeshRenderer meshRenderer;
    [HideInInspector]
    public bool isWhiteBall;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.drag = dragValue;
        rb.angularDrag = angularDragValue;
        billiardsManager = FindObjectOfType<BilliardsManager>();
        meshRenderer = GetComponent<MeshRenderer>();
        originalMaterial = meshRenderer.material;
        isWhiteBall = gameObject.CompareTag("WhiteBall");
    }

    private void FixedUpdate()
    {
        if (isFrozen)
        {
            // Keep frozen ball completely still
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            transform.position = frozenPosition;
            
            freezeTimer -= Time.fixedDeltaTime;
            if (freezeTimer <= 0)
            {
                UnfreezeBall();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isFrozen)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            transform.position = frozenPosition;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pocket"))
        {
            if (!isFrozen && billiardsManager.IsBallAllowed(gameObject))
            {
                Vector3 pocketPosition = other.transform.position;
                billiardsManager.BallPocketed(gameObject, pocketPosition);
            }
        } 
        if (other.CompareTag("Freeze"))
        {
            Destroy(other.gameObject);
        }
    }

    public void FreezeBall()
    {
        if (!isFrozen && !isWhiteBall)
        {
            isFrozen = true;
            freezeTimer = freezeDuration;
            
            frozenPosition = transform.position;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            
            if (frozenMaterial != null)
            {
                meshRenderer.material = frozenMaterial;
            }

            PlayFreezeEffect();
        }
    }

    public void ForceUnfreeze()
    {
        UnfreezeBall();
    }

    private void UnfreezeBall()
    {
        if (isFrozen)
        {
            isFrozen = false;
            rb.constraints = RigidbodyConstraints.None;
            meshRenderer.material = originalMaterial;
            PlayUnfreezeEffect();
        }
    }

    private void PlayFreezeEffect()
    {
      
    }

    private void PlayUnfreezeEffect()
    {
       
    }

    public bool IsFrozen()
    {
        return isFrozen;
    }
}