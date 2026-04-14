using UnityEngine;

public class SmokeTrailEmissionPlayer : MonoBehaviour
{
    private ParticleSystem particles;
    [SerializeField] private DieFromSpeed dieFromSpeed;
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private AnimationCurve emissionCurve;
    [SerializeField] float maxEmission = 2f;

    [SerializeField] private AnimationCurve soundCurve;
    [SerializeField] private float maxVolumeSound = 0.2f;
    [SerializeField] private AnimationCurve pitchCurve;

    bool isActive = false;
    const int CANAL = 2;

    private void Awake()
    {
        particles = GetComponent<ParticleSystem>();
        Desactivate();
    }

    void Activate()
    {
        isActive = true;
        particles.Play();
        SoundManager.instance.PlayCanal("Charging", CANAL);
    }

    void Desactivate()
    {
        isActive = false;
        particles.Stop();
        if (SoundManager.instance)
            SoundManager.instance.StopAudioCanal(CANAL);
    }

    private void Update()
    {
        if (particles != null && dieFromSpeed != null)
        {
            if (dieFromSpeed.timer > 0 && dieFromSpeed.isDying)
            {
                if (!isActive)
                {
                    Activate();
                }
                ParticleSystem.EmissionModule _emission = particles.emission;
                float _t = dieFromSpeed.timer / dieFromSpeed.durationBeforeDeath;

                _emission.rateOverTime = emissionCurve.Evaluate(1 - _t) * maxEmission;
                SoundManager.instance.SetVolumeCanal(CANAL, soundCurve.Evaluate(_t) * maxVolumeSound);
                SoundManager.instance.SetPitchCanal(CANAL, pitchCurve.Evaluate(_t));
            }

            if (dieFromSpeed.timer < 0 || !dieFromSpeed.isDying)
            {
                Desactivate();
            }
        }
    }

    private void OnDisable()
    {
        Desactivate();
    }
}
