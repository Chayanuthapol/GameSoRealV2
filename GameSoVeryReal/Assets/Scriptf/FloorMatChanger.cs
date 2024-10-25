using UnityEngine;
using System.Collections;

public class FloorMatChanger : MonoBehaviour
{
    [SerializeField] private Material originalMaterial;
    [SerializeField] private Material StickyMaterial;
    [SerializeField] private Material SliperyMaterial;
    [SerializeField] private float materialChangeDuration = 5f;
    
    private MeshRenderer meshRenderer;
    private Material currentMaterial;
    private Coroutine stickyMat;
    private Coroutine sliperyMat;
    
    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer.material != null)
        {
            originalMaterial = new Material(meshRenderer.material);
            currentMaterial = originalMaterial;
            meshRenderer.material = currentMaterial;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sticky"))
        {
            ChangeToStickyFloor();
            
        }
        if (other.CompareTag("Slip"))
        {
            ChangeToSliperyFloor();
        }
    }
    
    public void ChangeToStickyFloor()
    {
        // If there's already a material change in progress, stop it
        if (stickyMat != null)
        {
            StopCoroutine(stickyMat);
        }
        
        // Start new material change
        stickyMat = StartCoroutine(StickyMat());
    }
    
    private IEnumerator StickyMat()
    {
        // Change to power-up material
        Material tempMaterial = new Material(StickyMaterial);
        meshRenderer.material = tempMaterial;
        currentMaterial = tempMaterial;
        
        yield return new WaitForSeconds(materialChangeDuration);
        
        // Destroy the temporary material and revert to original
        meshRenderer.material = originalMaterial;
        currentMaterial = originalMaterial;
        Destroy(tempMaterial);
        
        stickyMat = null;
    }
    public void ChangeToSliperyFloor()
    {
        // If there's already a material change in progress, stop it
        if (sliperyMat != null)
        {
            StopCoroutine(sliperyMat);
        }
        
        // Start new material change
        sliperyMat = StartCoroutine(SliperyMat());
    }
    
    private IEnumerator SliperyMat()
    {
        Material tempMaterial = new Material(SliperyMaterial);
        meshRenderer.material = tempMaterial;
        currentMaterial = tempMaterial;
        
        yield return new WaitForSeconds(materialChangeDuration);
        
        // Destroy the temporary material and revert to original
        meshRenderer.material = originalMaterial;
        currentMaterial = originalMaterial;
        Destroy(tempMaterial);
        
        sliperyMat = null;
    }
}