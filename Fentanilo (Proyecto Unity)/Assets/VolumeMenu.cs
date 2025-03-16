using UnityEngine;
using UnityEngine.UI;

public class VolumeMenu : MonoBehaviour
{
    public Slider sfxSlider;
    public Slider musicSlider;

    public void ChangeAudio()
    {
        BroadcastMessage("UpdateSFXVolume", sfxSlider.value);
        BroadcastMessage("UpdateSongVolume", musicSlider.value);
    }
}
