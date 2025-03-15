using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    PlayerInput input;
    bool paused;
    [SerializeField] GameObject Opciones;
    [SerializeField] GameObject Volumen;
    [SerializeField] GameObject Controles;

    [SerializeField] Slider Music;
    [SerializeField] Slider SFX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        paused = false;
        input = GetComponent<PlayerInput>();
        HideAll();
    }

    // Update is called once per frame
    void Update()
    {
        if (input.actions["Pause"].IsPressed() && !paused)
        {
            paused = true;
            Time.timeScale = 0;
            ShowOptions();
        }
    }

    void HideAll()
    {
        Opciones.SetActive(false);
        Volumen.SetActive(false);
        Controles.SetActive(false);
    }

    public void ShowOptions()
    {
        HideAll();
        Opciones.SetActive(true);
    }

    public void ShowVolumen()
    {
        HideAll();
        Volumen.SetActive(true);
    }

    public void ShowControles()
    {
        HideAll();
        Controles.SetActive(true);
    }

    public void Exit()
    {
        HideAll();
        Time.timeScale = 1;
        paused = false;
    }

    public void changeVolMusic()
    {
        AudioManager.Instance.SongVolume = Music.value;        
    }
    public void changeVolSFX()
    {
        AudioManager.Instance.SFXVolume = SFX.value;        
    }
}
