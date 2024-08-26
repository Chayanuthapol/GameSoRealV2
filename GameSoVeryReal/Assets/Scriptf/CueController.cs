using System;
using UnityEngine;

public class CueController : MonoBehaviour
{
    [Range(1f, 5f)]
    [SerializeField] private float _strokeSensitivity = 1f;

    private Rigidbody _rb;
    private SphereCollider _tipCollider;
    [SerializeField] private GameObject _stick;

    private float _mouseStroke;

    private bool _active = false;
   

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _tipCollider = GetComponent<SphereCollider>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ToggleActive();
        }
        
   
        if (!_active)
        {
            return;
        }

        _mouseStroke = Input.GetAxis("Mouse Y");
    }

    private void FixedUpdate()
    {
        if (!_active)
        {
            return;
        }

        Vector3 movement = transform.forward * _mouseStroke * _strokeSensitivity;
        _rb.MovePosition(transform.position + movement);
    }

    private void ToggleActive()
    {
        _active = !_active;
        _tipCollider.enabled = !_active;
        _stick.SetActive(_active);
    }
   
}