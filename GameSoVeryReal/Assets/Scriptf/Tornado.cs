using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado : MonoBehaviour
{
        public Transform tornadoCenter;
        public float pullforce;
        public float RefreshRate;
        private void OnTriggerEnter(Collider other)
        {
                if (other.tag == "Ball")
                {
                        StartCoroutine(pullBalls(other,true));
                }
        }

        private void OnTriggerExit(Collider other)
        {
                if (other.tag == "Ball")
                {
                        StartCoroutine(pullBalls(other,false));
                }
        }

        IEnumerator pullBalls(Collider ball, bool shouldPull)
        {
                if(shouldPull)
                {
                        Vector3 ForceDir = tornadoCenter.position - ball.transform.position;
                        ball.GetComponent<Rigidbody>().AddForce(ForceDir.normalized * pullforce * Time.deltaTime);
                        yield return RefreshRate;
                        StartCoroutine(pullBalls(ball, shouldPull));
                }
               
        }
}
