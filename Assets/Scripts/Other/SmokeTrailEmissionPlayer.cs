using UnityEngine;

public class SmokeTrailEmissionPlayer : MonoBehaviour
{
    private ParticleSystem particles;
    [SerializeField] private SpeedPlayerManager speedPlayerManager;
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private AnimationCurve emissionCurve;
    [SerializeField] float maxEmission = 2f;

    bool isActive = false;

    private void Awake()
    {
        particles = GetComponent<ParticleSystem>();
        Desactivate();
    }

    void Activate()
    {
        isActive = true;
        particles.Play();
    }

    void Desactivate()
    {
        isActive = false;
        particles.Stop();
    }

    private void Update()
    {
        if (particles != null && speedPlayerManager != null)
        {
            if (rb.linearVelocity.magnitude < speedPlayerManager.speedThreshold)
            {
                if (!isActive)
                {
                    Activate();
                }
                ParticleSystem.EmissionModule _emission = particles.emission;
                float _t = rb.linearVelocity.magnitude / speedPlayerManager.speedThreshold;

                _emission.rateOverTime = emissionCurve.Evaluate(1 - _t) * maxEmission;
            }

            if (rb.linearVelocity.magnitude > speedPlayerManager.speedThreshold && isActive)
            {
                Desactivate();
            }
        }
    }
}
