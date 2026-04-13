using UnityEngine;
using System.Collections.Generic;
using System.IO;

[System.Serializable]
public struct Sound
{
    public string name;
    public AudioClip clip;
    [Range(0, 2f)] public float volume;
    [Range(0, 2f)] public float pitch;
    public Sound(AudioClip _clip, float _volume, float _pitch, string _name)
    {
        clip = _clip;
        volume = _volume;
        pitch = _pitch;
        name = _name;
    }
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private Sound[] sounds;

    List<AudioSource> audioSources = new List<AudioSource>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }else
        {
            Destroy(this);
            return;
        }
        AddAudioSource();
    }

    void AddAudioSource()
    {
        audioSources.Add(gameObject.AddComponent<AudioSource>());
        audioSources[audioSources.Count - 1].enabled = true;
    }

    bool GetSound(string _name, out Sound _soundOut)
    {
        foreach (var _sound in sounds)
        {
            if (_sound.name == name)
            {
                _soundOut = _sound;
                return true;
            }
        }
        _soundOut = new Sound();
        return false;
    }

    public void PlayOneShot(string _name, int _canal)
    {
        if (_canal >= audioSources.Count)
        {
            for (int _i = audioSources.Count - 1; _i < _canal; _i++)
                AddAudioSource();
        }
        AudioSource _audioSource = audioSources[_canal];
        Sound _s;
        if (!GetSound(_name, out _s))
           return;
        _audioSource.pitch = _s.pitch;
        _audioSource.PlayOneShot(_s.clip, _s.volume);
    }
}
