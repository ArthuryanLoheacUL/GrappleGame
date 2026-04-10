using UnityEngine;
using UnityEngine.UI;

public class DieFromSpeed : MonoBehaviour
{
    SpeedPlayerManager playerManager;
    Rigidbody2D rb;
    [HideInInspector] public float timer = 0f;
    public float durationBeforeDeath = 0f;

    [SerializeField] private Image spriteDie;
    [SerializeField] private GameObject prefabExplosion;
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
        } else
        {
            timer -= Time.deltaTime;
        }
        if (timer >= durationBeforeDeath)
        {
            DestroyBall();
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
        }
    }

    void DestroyBall()
    {
        Instantiate(prefabExplosion, transform.position, Quaternion.identity);
        finalCameraPos.ActiveFinalCam();
        Destroy(gameObject);
    }
}
