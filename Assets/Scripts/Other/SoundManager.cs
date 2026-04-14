using UnityEngine;
using System.Collections.Generic;

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
        string _nameTrim = _name.TrimEnd(new char[] { '\r', ' ' });
        foreach (var _sound in sounds)
        {
            string _soundNameTrim = _sound.name.TrimEnd(new char[] { '\r', ' ' });

            if (_nameTrim == _soundNameTrim)
            {
                _soundOut = _sound;
                return true;
            }
        }
        _soundOut = new Sound();
        return false;
    }

    public float GetDurationSound(string _name)
    {
        Sound _s;
        if (GetSound(_name, out _s))
            return _s.clip.length;
        return 0f;
    }
    public float GetVolumeSound(string _name)
    {
        Sound _s;
        if (GetSound(_name, out _s))
            return _s.volume;
        return 0f;
    }


    public AudioSource GetCanal(int _canal)
    {
        if (_canal >= audioSources.Count)
        {
            for (int _i = audioSources.Count - 1; _i < _canal; _i++)
                AddAudioSource();
        }
        return audioSources[_canal];
    }

    public void PlayOneShot(string _name, int _canal, float _rangePitch = 0f)
    {
        AudioSource _audioSource = GetCanal(_canal);
        Sound _s;
        if (!GetSound(_name, out _s))
            return;
        _audioSource.pitch = _s.pitch + Random.Range(-_rangePitch, _rangePitch);
        _audioSource.PlayOneShot(_s.clip, _s.volume);
    }
    public void PlayCanal(string _name, int _canal, bool _loop = false)
    {
        AudioSource _audioSource = GetCanal(_canal);
        Sound _s;
        if (!GetSound(_name, out _s))
            return;
        _audioSource.pitch = _s.pitch;
        _audioSource.clip = _s.clip;
        _audioSource.volume = _s.volume;
        _audioSource.loop = _loop;
        _audioSource.Play();
    }

    public void RestartAudioCanal(int _canal)
    {
        AudioSource _audioSource = GetCanal(_canal);
        _audioSource.Stop();
        _audioSource.clip = _audioSource.clip;
        _audioSource.Play();
    }

    public void SetVolumeCanal(int _canal, float _volume)
    {
        AudioSource _audioSource = GetCanal(_canal);
        _audioSource.volume = _volume;
    }

    public void SetPitchCanal(int _canal, float _pitch)
    {
        AudioSource _audioSource = GetCanal(_canal);
        _audioSource.pitch = _pitch;
    }

    public void StopAudioCanal(int _canal)
    {
        AudioSource _audioSource = GetCanal(_canal);
        _audioSource.Stop();
    }
}
