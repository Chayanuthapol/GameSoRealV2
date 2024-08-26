using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public Ball[] balls;  // All balls in the game
    public CueController cueController;  // Reference to the cue controller
    public int currentPlayer = 1;  // Track the current player (1 or 2)

    private bool turnInProgress = false;

    private void Update()
    {
        if (!turnInProgress && AreBallsStopped())
        {
            EndTurn();
        }
    }

    // Check if all balls have stopped moving
    private bool AreBallsStopped()
    {
        foreach (Ball ball in balls)
        {
            if (ball.isMoving)
            {
                return false;
            }
        }
        return true;
    }

    // End the current turn and switch players
    public void EndTurn()
    {
        turnInProgress = false;

        // Switch player
        currentPlayer = (currentPlayer == 1) ? 2 : 1;

        Debug.Log("Player " + currentPlayer + "'s turn");
        Debug.Log("End");
        StartTurn();
    }

    // Start the next player's turn
    public void StartTurn()
    {
        turnInProgress = true;

        // You can add additional logic to reset the game state for the next player's turn
    }
}
