using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeedMeter : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Image speedFillImg;
    [SerializeField] private Image speedBGImg;
    [SerializeField] private TMP_Text speedText;

    [SerializeField] private Image barSpeed;
    [SerializeField] private Image barMaxSpeed;
    private Color barOriginalColor;

    [SerializeField] private Color basicColor;

    private Rigidbody2D playerRb;
    private SpeedPlayerManager speedPlayerManager;

    private void Start()
    {
        playerRb = player.GetComponent<Rigidbody2D>();
        speedPlayerManager = player.GetComponent<SpeedPlayerManager>();
        barOriginalColor = barSpeed.color;
    }

    private void Update()
    {
        if (playerRb == null || speedPlayerManager == null)
        {
            SetSpeed(0);
            return;
        }
        SetSpeed(playerRb.linearVelocity.magnitude);
    }

    void SetSpeed(float _speed)
    {
        int _roundSpeed = Mathf.RoundToInt(_speed * 5);

        speedText.text = _roundSpeed.ToString();
        speedFillImg.fillAmount = (_speed / speedPlayerManager.maxSpeed) * speedBGImg.fillAmount;
        UpdateColor(_speed);
    }

    void UpdateColor(float _speed)
    {
        if (_speed < speedPlayerManager.speedThreshold)
        {
            SetColor(basicColor);
            barSpeed.color = barOriginalColor;
            barMaxSpeed.color = barOriginalColor;
        }
        else if (_speed > speedPlayerManager.maxSpeedColorThreshold)
        {
            SetColor(speedPlayerManager.maxSpeedColor);
            barSpeed.color = speedPlayerManager.maxSpeedColor;
            barMaxSpeed.color = speedPlayerManager.maxSpeedColor;
        } else
        {
            SetColor(speedPlayerManager.speedColor);
            barSpeed.color = speedPlayerManager.speedColor;
            barMaxSpeed.color = barOriginalColor;
        }
    }

    void SetColor(Color _color)
    {
        speedFillImg.color = _color;
        speedText.color = _color;
    }
}
