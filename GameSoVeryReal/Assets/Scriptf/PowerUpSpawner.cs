using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerUpSpawner : MonoBehaviour
{
    [Header("Power-up Prefabs")]
    public GameObject[] powerUpPrefabs;
    
    [Header("Spawn Settings")]
    public float spawnInterval = 10f;     
    public int maxPowerUpsOnTable = 3;    
    public float tableLength = 2.7432f;   
    public float tableWidth = 1.3716f;    
    public float spawnHeightOffset = 0.1f;
    
    [Header("Safe Spawn Settings")]
    public float minDistanceFromBalls = 0.3f;
    public float minDistanceFromPockets = 0.3f;
    public float minDistanceFromWhiteBall = 0.3f;
    public int maxSpawnAttempts = 30;
    
    [Header("White Ball Movement Detection")]
    public float stopThreshold = 0.01f;  
     
    
    public Rigidbody whiteBallRb;
    private float timeSinceLastSpawn = 0f;
    private float stableTime = 0f;
    private List<GameObject> activePowerUps = new List<GameObject>();
    private Transform tableTransform;
    private bool ballWasMoving = false;
    private bool shouldSpawnAfterStop = false;
    
    void Start()
    {
        tableTransform = transform;
        
        GameObject whiteBall = GameObject.FindGameObjectWithTag("WhiteBall");
        if (whiteBall != null)
        {
            whiteBallRb = whiteBall.GetComponent<Rigidbody>();
        }
        
        SpawnInitialPowerUps();
    }
    
    private void SpawnInitialPowerUps()
    {
        
        for (int i = 0; i < maxPowerUpsOnTable; i++)
        {
            SpawnPowerUp();
        }
    }
    
    void Update()
    {
        if (whiteBallRb == null) return;
        
        activePowerUps.RemoveAll(item => item == null);
        
        float ballSpeed = whiteBallRb.velocity.magnitude;
        
        // Check if ball is moving
        if (ballSpeed >= stopThreshold)
        {
            ballWasMoving = true;
           
            shouldSpawnAfterStop = true;
        }

        if (ballSpeed < stopThreshold)
        {
            ballWasMoving = false;
            shouldSpawnAfterStop = false;

            if(activePowerUps.Count < maxPowerUpsOnTable)
            {
                // Clear existing power-ups
                ClearAllPowerUps();
                                
                // Spawn new power-ups
                for (int i = 0; i < maxPowerUpsOnTable; i++) 
                { 
                    SpawnPowerUp(); 
                }
            }
        }
    }
    
    void SpawnPowerUp()
    {
        if (activePowerUps.Count >= maxPowerUpsOnTable) 
            return;
        
        Vector3 spawnPosition = FindValidSpawnPosition();
        
        if (spawnPosition != Vector3.zero)
        {
            int randomIndex = Random.Range(0, powerUpPrefabs.Length);
            GameObject powerUpPrefab = powerUpPrefabs[randomIndex];
            GameObject powerUp = Instantiate(powerUpPrefab, spawnPosition, Quaternion.identity);
            powerUp.transform.parent = tableTransform;
            activePowerUps.Add(powerUp);
            powerUp.transform.rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
            
            PlaySpawnEffect(spawnPosition);
        }
    }
    
    private void PlaySpawnEffect(Vector3 position)
    {
        
    }
    
    Vector3 FindValidSpawnPosition()
    {
        for (int i = 0; i < maxSpawnAttempts; i++)
        {
            float randomX = Random.Range(-tableWidth/2, tableWidth/2);
            float randomZ = Random.Range(-tableLength/2, tableLength/2);
            Vector3 potentialPosition = tableTransform.position + 
                                      new Vector3(randomX, spawnHeightOffset, randomZ);
            
            if (IsValidSpawnPosition(potentialPosition))
            {
                return potentialPosition;
            }
        }
        
        Debug.LogWarning("Could not find valid spawn position after " + maxSpawnAttempts + " attempts");
        return Vector3.zero;
    }
    
    bool IsValidSpawnPosition(Vector3 position)
    {
        // Check distance from regular balls
        foreach (GameObject ball in GameObject.FindGameObjectsWithTag("Ball"))
        {
            if (Vector3.Distance(position, ball.transform.position) < minDistanceFromBalls)
            {
                return false;
            }
        }
        
        // Check distance from white ball
        foreach (GameObject whiteBall in GameObject.FindGameObjectsWithTag("WhiteBall"))
        {
            if (Vector3.Distance(position, whiteBall.transform.position) < minDistanceFromWhiteBall)
            {
                return false;
            }
        }
        
        // Check distance from pockets
        foreach (GameObject pocket in GameObject.FindGameObjectsWithTag("Pocket"))
        {
            if (Vector3.Distance(position, pocket.transform.position) < minDistanceFromPockets)
            {
                return false;
            }
        }
        
        // Check distance from other power-ups
        foreach (GameObject powerUp in activePowerUps)
        {
            if (powerUp != null && Vector3.Distance(position, powerUp.transform.position) < minDistanceFromBalls)
            {
                return false;
            }
        }
        
        return true;
    }
    
    public void ForceSpawnPowerUp(int powerUpIndex)
    {
        if (powerUpIndex >= 0 && powerUpIndex < powerUpPrefabs.Length && activePowerUps.Count < maxPowerUpsOnTable)
        {
            Vector3 spawnPosition = FindValidSpawnPosition();
            if (spawnPosition != Vector3.zero)
            {
                GameObject powerUp = Instantiate(powerUpPrefabs[powerUpIndex], spawnPosition, Quaternion.identity);
                powerUp.transform.parent = tableTransform;
                activePowerUps.Add(powerUp);
            }
        }
    }
    
    public void ClearAllPowerUps()
    {
        foreach (GameObject powerUp in activePowerUps)
        {
            if (powerUp != null)
            {
                Destroy(powerUp);
            }
        }
        activePowerUps.Clear();
    }
}