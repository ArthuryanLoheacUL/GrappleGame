using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeedMeter : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Image speedFillImg;
    [SerializeField] private Image speedBGImg;
    [SerializeField] private TMP_Text speedText;

    [SerializeField] private Color basicColor;

    private Rigidbody2D playerRb;
    private SpeedPlayerManager speedPlayerManager;

    private void Start()
    {
        playerRb = player.GetComponent<Rigidbody2D>();
        speedPlayerManager = player.GetComponent<SpeedPlayerManager>();
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
        }
        else if (_speed > speedPlayerManager.maxSpeedColorThreshold)
        {
            SetColor(speedPlayerManager.maxSpeedColor);
        } else
        {
            SetColor(speedPlayerManager.speedColor);
        }
    }

    void SetColor(Color _color)
    {
        speedFillImg.color = _color;
        speedText.color = _color;
    }
}
