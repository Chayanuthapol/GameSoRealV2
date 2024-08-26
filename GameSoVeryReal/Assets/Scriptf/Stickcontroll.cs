using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stickcontroll : MonoBehaviour
{
    public Transform cue;
    public Ball ball;
    public float forceMultiplier = 10f;
    public Camera mainCamera;

    private Vector3 hitDirection;
    private TurnManager turnManager;

    private void Start()
    {
        turnManager = FindObjectOfType<TurnManager>();
    }

    private void Update()
    {
        if (turnManager.currentPlayer == 1 || turnManager.currentPlayer == 2)
        {
            AimCue();

            if (Input.GetMouseButtonDown(0))
            {
                HitBall();
            }
        }
    }

    private void AimCue()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            hitDirection = (ball.transform.position - hit.point).normalized;
            cue.position = ball.transform.position - hitDirection * 0.5f;
            cue.LookAt(ball.transform.position);
        }
    }

    private void HitBall()
    {
        ball.Hit(hitDirection * forceMultiplier);
        turnManager.EndTurn();  // End turn after hitting the ball
       
    }

    
}
