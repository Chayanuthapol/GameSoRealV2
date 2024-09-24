using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    public int score;

    public Player()
    {
        score = 0;
    }

    public void AddScore(int points)
    {
        score += points;
    }
}

