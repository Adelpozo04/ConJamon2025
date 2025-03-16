using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    private AudioSource audioSource;

    private bool playedRestart = false;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playedRestart = false;
    }
    public void PlayJump()
    {
        audioSource.clip = AudioManager.Instance.GetAudioClip(SoundSFX.JUMP);
        audioSource.Play();
    }
    public void PlayDeath()
    {
        audioSource.clip = AudioManager.Instance.GetAudioClip(SoundSFX.PLAYER_DEATH);
        audioSource.Play();
    }
    public void PlayRestart()
    {
        if (!playedRestart)
        {
            audioSource.clip = AudioManager.Instance.GetAudioClip(SoundSFX.PLAYER_RESTART);
            audioSource.Play();
            playedRestart = true;
        }
    }
    public void PlayShoot()
    {
        audioSource.clip = AudioManager.Instance.GetAudioClip(SoundSFX.SHOOT);
        audioSource.Play();
    }
}
