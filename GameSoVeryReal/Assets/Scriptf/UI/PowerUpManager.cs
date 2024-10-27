using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public PowerUpTextUI powerUpTextUI;
    private Dictionary<string, Coroutine> activePowerUps = new Dictionary<string, Coroutine>();
    private string powerUpFormat = "{0}: {1:F1}s";
    private string countdownFormat = "{0:F1}s";
    
    void Start()
    {
        powerUpTextUI.ClearText();
    }
    public void ShowPowerUp(string powerUpType, float duration)
    {
        if (activePowerUps.ContainsKey(powerUpType))
        {
            StopCoroutine(activePowerUps[powerUpType]);
            activePowerUps.Remove(powerUpType);
        }

        Coroutine powerUpCoroutine = StartCoroutine(PowerUpCountdown(powerUpType, duration));
        activePowerUps.Add(powerUpType, powerUpCoroutine);
    }

    private IEnumerator PowerUpCountdown(string powerUpType, float duration)
    {
        float remainingTime = duration;

        while (remainingTime > 0)
        {
            UpdatePowerUpCountdownText(powerUpType, remainingTime);
            remainingTime -= Time.deltaTime;
            yield return null;
        }

        activePowerUps.Remove(powerUpType);
        UpdatePowerUpText();

        if (activePowerUps.Count == 0)
        {
            powerUpTextUI.ClearText();
        }
    }

    private void UpdatePowerUpText()
    {
        if (activePowerUps.Count == 0)
        {
            return;
        }

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        System.Text.StringBuilder countdownSb = new System.Text.StringBuilder();
        bool isFirst = true;

        foreach (string powerUpType in activePowerUps.Keys)
        {
            if (!isFirst)
            {
                sb.AppendLine();
            }

            float remainingTime = GetRemainingTime(powerUpType);
            sb.AppendFormat(powerUpFormat, powerUpType, remainingTime);
            countdownSb.AppendFormat(countdownFormat, remainingTime);
            isFirst = false;
        }

        powerUpTextUI.SetText(sb.ToString());
    }
    private void UpdatePowerUpCountdownText(string powerUpType, float remainingTime)
    {
        TextMeshProUGUI powerUpCountdownText = GameObject.Find("PowerUpTimeRemaining").GetComponent<TextMeshProUGUI>();
        
        string countdownText = $"{powerUpType}: {remainingTime:F1}s";
        powerUpCountdownText.text = countdownText;
    }

    private float GetRemainingTime(string powerUpType)
    {
        Ball ball = FindObjectOfType<Ball>();
        switch (powerUpType.ToLower())
        {
            case "bomb": return ball.countdownTime;
            case "speed": return ball.countSpeed;
            case "heavy": return ball.countHeavy;
            case "freeze": return ball.countFreeze;
            case "tornado": return ball.countTornado;
            case "slip": return ball.countSlip;
            case "sticky": return ball.countSticky;
            default: return 0f;
        }
    }
    
}