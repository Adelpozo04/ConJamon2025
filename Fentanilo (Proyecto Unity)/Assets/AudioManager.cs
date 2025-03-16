using UnityEditor;
using UnityEngine;

public enum SoundSFX { MECANISMO_ACTIVAR, MECANISMO_DESACTIVAR, JUMP, PLAYER_DEATH, PLAYER_RESTART, PLAYER_SHOOT, MELEE_WALK, MELEE_HURT }

public class AudioManager : MonoBehaviour
{   
    
    [Header("Ajustes de audio")]
    public float SongVolume = 1;
    public float SFXVolume = 1;

    [Header("Canciones")]
    [SerializeField] private AudioClip song1;
    [SerializeField] private AudioClip song2;
    [Header("SFX")]
    [SerializeField] private AudioClip mecanismoActivar; 
    [SerializeField] private AudioClip mecanismoDesactivar;
    [SerializeField] private AudioClip saltar;
    [SerializeField] private AudioClip playerMuerte;
    [SerializeField] private AudioClip playerRestart;
    [SerializeField] private AudioClip playerDisparar;
    [SerializeField] private AudioClip meleeAndar;
    [SerializeField] private AudioClip meleeHurt;

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

        audioSource.Stop();
        audioSource.PlayScheduled(AudioSettings.dspTime);
    }
    public void PlayRandomSong()
    {
        int random = Random.Range(1, 2);

        PlaySong(random);
    }

    public AudioClip GetAudioClip(SoundSFX sfx)
    {
        switch (sfx)
        {
            case SoundSFX.MECANISMO_ACTIVAR:
                return mecanismoActivar;
            case SoundSFX.MECANISMO_DESACTIVAR:
                return mecanismoDesactivar;
            case SoundSFX.JUMP:
                return saltar;
            case SoundSFX.PLAYER_DEATH:
                return playerMuerte;
            case SoundSFX.PLAYER_RESTART:
                return playerRestart;
            case SoundSFX.PLAYER_SHOOT:
                return playerDisparar;
            case SoundSFX.MELEE_WALK:
                return meleeAndar;
            case SoundSFX.MELEE_HURT:
                return meleeHurt;
            default:
                return null;
        }
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
