using UnityEngine;

public class MusicManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.PlayScheduled(AudioSettings.dspTime);

    }

}
