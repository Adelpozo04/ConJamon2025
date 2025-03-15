using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public float SongVolume = 1;
    public float SFXVolume = 1;

    [SerializeField] private AudioClip song1;
    [SerializeField] private AudioClip song2;

    private AudioSource audioSource;

    public static AudioManager Instance = null;

    public float getSongVolume() { return SongVolume; }
    public float getSFXVolume() { return SFXVolume; }
    
    public void setSFXVolume(float sfxVolume)
    {
        SFXVolume = sfxVolume;
    }
    public void setSongVolume(float songVolume)
    {
        SongVolume = songVolume;
        audioSource.volume = SongVolume;
    }
    public void StopSong()
    {
        audioSource.Pause();
    }
    public void PlaySong(int numSong)
    {
        if (numSong == 1)
            audioSource.clip = song1;
        else if (numSong == 2)
            audioSource.clip = song2;

        audioSource.PlayScheduled(AudioSettings.dspTime);
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //para conservar entre escenas
            gameObject.transform.parent = null;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
    }

}
