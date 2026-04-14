using UnityEngine;

public class PlayAudioOnStart : MonoBehaviour
{
    [SerializeField] private string soundName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SoundManager.instance.PlayOneShot(soundName, 1);
    }
}
