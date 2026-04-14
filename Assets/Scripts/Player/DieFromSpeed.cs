using UnityEngine;
using UnityEngine.UI;

public class DieFromSpeed : MonoBehaviour
{
    SpeedPlayerManager playerManager;
    Rigidbody2D rb;
    [HideInInspector] public float timer = 0f;
    public float durationBeforeDeath = 0f;
    public bool isDying = false;

    [SerializeField] private Image spriteDie;
    [SerializeField] private FinalCameraPos finalCameraPos;

    private void Awake()
    {
        playerManager = GetComponent<SpeedPlayerManager>();
        rb = GetComponent<Rigidbody2D>();
        spriteDie.enabled = false;
    }

    private void Update()
    {
        float _magnitude = rb.linearVelocity.magnitude;
        if (_magnitude < playerManager.speedThreshold)
        {
            timer += Time.deltaTime;
            isDying = true;
        } else
        {
            timer -= Time.deltaTime;
            isDying = false;
        }
        if (timer >= durationBeforeDeath)
        {
            isDying = false;
            GetComponent<ActivePlayer>().Die();
        }
        if (timer > 0f)
        {
            spriteDie.enabled = true;
            spriteDie.fillAmount = timer / durationBeforeDeath;
            CineShakeManager.instance.ShakeOneFrame((timer / durationBeforeDeath) * 3);
        } else
        {
            spriteDie.enabled = false;
            timer = 0f;
            isDying = false;
        }
    }

}
