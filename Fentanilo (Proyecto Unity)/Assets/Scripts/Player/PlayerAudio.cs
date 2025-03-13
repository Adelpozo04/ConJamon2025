using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private AudioClip jumpAudio;
    [SerializeField] private AudioClip deathAudio;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void PlayJump()
    {
        audioSource.clip = jumpAudio;
        audioSource.Play();
    }
    public void PlayDeath()
    {
        audioSource.clip = deathAudio;
        audioSource.Play();
    }
}
