using UnityEngine;
using static Unity.VisualScripting.Member;

public class MusicManager : MonoBehaviour
{
    [System.Serializable]
    public struct Music
    {
        public string name;
        public AudioClip audioclip;
        [Range(0, 2f)] public float volume;
    }


    public static MusicManager instance;
    public string playMusic;
    [HideInInspector] public string currentMusic;
    [SerializeField] private Music[] musics;

    [Range(0, 2f)] public float volume;

    AudioSource source;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            if (MusicManager.instance.currentMusic != playMusic && playMusic != "")
                MusicManager.instance.PlayMusic(playMusic);
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        source = gameObject.AddComponent<AudioSource>();
        source.loop = true;
        if (currentMusic != playMusic && playMusic != "")
            MusicManager.instance.PlayMusic(playMusic);
    }

    public void PlayMusic(string _nameMusic)
    {
        foreach (var _music in musics)
        {
            if (_music.name == _nameMusic)
            {
                source.Stop();
                source.clip = _music.audioclip;
                source.volume = _music.volume * volume;
                source.Play();
                currentMusic = _music.name;
            }
        }
    }
}
