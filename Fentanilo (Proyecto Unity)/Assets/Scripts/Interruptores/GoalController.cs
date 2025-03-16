using System;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Script para el objeto al que hay que llegar para pasar al siguiente nivel
/// </summary>
public class GoalController : MonoBehaviour
{

    private AudioSource audioSource;
    private float startVolume;
    public void UpdateSFXVolume(float volume)
    {
        audioSource.volume = Mathf.Lerp(0, startVolume, volume);
    }
    void Start()
    {
        if(CameraFollow.Instance != null)
        {
            CameraFollow.Instance.setGoalTransform(GetComponent<Transform>());
        }

        audioSource = GetComponent<AudioSource>();
    }




    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            //Sacar un mensaje de que has ganado
            //Fade out y cambiar de escena
            other.gameObject.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, 0);
            other.gameObject.GetComponent<PlayerMovement>().enabled = false;
            SombraStorage.Instance.clearRecords();
            LevelManager.Instance.Won();

            audioSource.clip = AudioManager.Instance.GetAudioClip(SoundSFX.PASAR_NIVEL);
            audioSource.Play();
        }
        else
        {
            Destroy(other.gameObject);
        }
    }
}
