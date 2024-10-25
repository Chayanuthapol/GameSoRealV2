using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class FreezePowerUpManager : MonoBehaviour
{
    private static FreezePowerUpManager instance;
    public static FreezePowerUpManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<FreezePowerUpManager>();
            }
            return instance;
        }
    }

    private List<Balls> frozenBalls = new List<Balls>();
    private int maxFrozenBalls = 3;

    public void ActivateFreezePowerUp()
    {
     
        var coloredBalls = FindObjectsOfType<Balls>()
            .Where(ball => !ball.isWhiteBall && !ball.IsFrozen())
            .ToList();

        // Randomly select 3 balls to freeze
        int ballsToFreeze = Mathf.Min(maxFrozenBalls, coloredBalls.Count);
        var selectedBalls = coloredBalls
            .OrderBy(x => Random.value)
            .Take(ballsToFreeze)
            .ToList();

        // Freeze the selected balls
        foreach (var ball in selectedBalls)
        {
            ball.FreezeBall();
            frozenBalls.Add(ball);
        }
    }

    public void UnfreezeAllBalls()
    {
        foreach (var ball in frozenBalls)
        {
            if (ball != null) // Check if ball still exists
            {
                ball.ForceUnfreeze();
            }
        }
        frozenBalls.Clear();
    }
}