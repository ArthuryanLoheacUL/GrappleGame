using UnityEngine;

public class GunSound : MonoBehaviour
{
    [SerializeField] private AudioClip audioOnShoot;
    [Range(0.0f, 2f)] [SerializeField] private float volumeShoot;
    [Range(0.0f, 2f)] [SerializeField] private float pitchShoot;
    [SerializeField] private AudioClip audioOnGrab;
    [Range(0.0f, 2f)][SerializeField] private float volumeGrab;
    [Range(0.0f, 2f)] [SerializeField] private float pitchGrab;

    AudioSource sourceOnShoot;
    AudioSource sourceOnGrab;

    private void Awake()
    {
        sourceOnShoot = gameObject.AddComponent<AudioSource>();
        sourceOnGrab = gameObject.AddComponent<AudioSource>();
    }

    public void PlayOnShoot()
    {
        if (sourceOnShoot == null || audioOnShoot == null)
            return;

        sourceOnShoot.pitch = Random.Range(pitchShoot - 0.1f, pitchShoot + 0.1f);
        sourceOnShoot.PlayOneShot(audioOnShoot, volumeShoot);
    }

    public void PlayOnGrab()
    {
        if (sourceOnGrab == null || audioOnGrab == null)
            return;

        sourceOnGrab.pitch = Random.Range(pitchGrab - 0.1f, pitchGrab + 0.1f);
        sourceOnGrab.PlayOneShot(audioOnGrab, volumeGrab);
    }
}
