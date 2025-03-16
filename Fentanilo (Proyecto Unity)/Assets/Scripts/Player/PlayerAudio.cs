using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
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
        audioSource.clip = AudioManager.Instance.GetAudioClip(SoundSFX.PLAYER_RESTART);
        audioSource.Play();
    }
    public void PlayShoot()
    {
        audioSource.clip = AudioManager.Instance.GetAudioClip(SoundSFX.SHOOT);
        audioSource.Play();
    }
}
