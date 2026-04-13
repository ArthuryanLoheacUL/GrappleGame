using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeedMeter : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Image speedFillImg;
    [SerializeField] private Image speedBGImg;
    [SerializeField] private TMP_Text speedText;

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
            return;
        float _speed = playerRb.linearVelocity.magnitude;
        int _roundSpeed = Mathf.RoundToInt(_speed * 5);

        speedText.text = _roundSpeed.ToString();
        speedFillImg.fillAmount = (_speed / speedPlayerManager.maxSpeed) * speedBGImg.fillAmount;
    }
}
