using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CueBall : MonoBehaviour
{
   [SerializeField] private GameObject cuehited;
   private Rigidbody _rb;
   private bool _hasCued;

   private void Awake()
   {
      _rb = GetComponent<Rigidbody>();
   }

   private void Cue()
   {
      _hasCued = true;
   }
   private void HitBall()
   {
      _rb.AddForce (new Vector3 (0f, 0f, 20f), ForceMode.Impulse);
   }
   private void Update()
   {
     
      if (_hasCued && _rb.velocity.magnitude !=0)
      {
         Invoke ("HitBall", 1f);  
      }
   }
}
