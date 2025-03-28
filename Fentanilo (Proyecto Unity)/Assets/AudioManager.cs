using UnityEditor;
using UnityEngine;

public enum SoundSFX { MECANISMO_ACTIVAR, MECANISMO_DESACTIVAR, JUMP, PLAYER_DEATH, PLAYER_RESTART, SHOOT, MELEE_WALK, ENEMY_HURT, PASAR_NIVEL }

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
    [SerializeField] private AudioClip enemyHurt;
    [SerializeField] private AudioClip pasarNivel;

    [SerializeField] AudioSource audioSource;

    public static AudioManager Instance = null;

    public float getSongVolume() { return SongVolume; }
    public float getSFXVolume() { return SFXVolume; }
    
    public void updateSFXVolume(float sfxVolume)
    {
        SFXVolume = sfxVolume;
    }
    public void updateSongVolume(float songVolume)
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

        audioSource.volume = SongVolume;

        audioSource.Stop();
        audioSource.PlayScheduled(AudioSettings.dspTime);
    }
    public void PlayRandomSong()
    {
        int random = Random.Range(1, 3);

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
            case SoundSFX.SHOOT:
                return playerDisparar;
            case SoundSFX.MELEE_WALK:
                return meleeAndar;
            case SoundSFX.ENEMY_HURT:
                return enemyHurt;
            case SoundSFX.PASAR_NIVEL:
                return pasarNivel;
            default:
                return null;
        }
    }

    private void CallOnRestartScene()
    {
        // ALUCINAS CANTIDUBI BRO PERO SON LAS 15:00 Y QUEDA 5 HORAS
        GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (var obj in allObjects)
        {
            obj.BroadcastMessage("UpdateSFXVolume", SFXVolume, SendMessageOptions.DontRequireReceiver);
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

        audioSource = GetComponent<AudioSource>();
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource.loop = true;
        CallOnRestartScene();
    }

}
