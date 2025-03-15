using UnityEngine;

public abstract class Activador : MonoBehaviour
{
    /// <summary>
    /// Los objetos activables a ser activados por este activador. (xd)
    /// </summary>
    [SerializeField] private Activable[] activables;

    public bool alwaysSendTrueToActivators = false;

    /// <summary>
    /// Variable que guarda el último estado del Activador
    /// </summary>
    public bool isPressed = false;

    /// <summary>
    /// El método para enviar señal de activarse a todos los activables asociados a este activador.
    /// </summary>
    /// <param name="state">El estado de señal enviada. Ej. true -> abrir puerta, false -> cerrar puerta</param>
    /// 

    // para el sfx
    public AudioClip activateSFX;
    public AudioClip deactivateSFX;
    private AudioSource audioSource;
    protected void SendToActivables(bool state)
    {
        foreach (Activable act in activables)
        {
            if(act != null) act.Activar(state);
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// El método para cambiar automáticamente el estado del Activador a su opuesto.
    /// </summary>
    protected void Switch()
    {        
        isPressed = !isPressed;

        if (alwaysSendTrueToActivators)
        {
            SendToActivables(true);
        }
        else
            SendToActivables(!isPressed);
    }

    protected void PlayAudioSFX()
    {
        /*
        audioSource.clip = isPressed ? activateSFX : deactivateSFX;
        audioSource.Play();*/
    }
}
