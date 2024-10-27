using TMPro;
using UnityEngine;

public class PowerUpTextUI : MonoBehaviour
{
    public TextMeshProUGUI powerUpText;
    public TextMeshProUGUI countdownText; // New text component for countdown
    private Color originalPowerUpColor;

    void Start()
    {
        originalPowerUpColor = powerUpText.color;
        powerUpText.text = "";
        countdownText.text = "";
    }

    public void SetText(string message)
    {
        powerUpText.text = message;
        powerUpText.color = originalPowerUpColor;
    }

    public void SetCountdownText(string message)
    {
        countdownText.text = message;
    }

    public void ClearText()
    {
        powerUpText.text = "";
        countdownText.text = "";
    }
}