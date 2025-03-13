using UnityEngine;
using UnityEngine.WSA;

public abstract class Activador : MonoBehaviour
{
    /// <summary>
    /// Los objetos activables a ser activados por este activador. (xd)
    /// </summary>
    [SerializeField] private Activable[] activables;

    /// <summary>
    /// Variable que guarda el último estado del Activador
    /// </summary>
    private bool isPressed = false;

    /// <summary>
    /// El método para enviar señal de activarse a todos los activables asociados a este activador.
    /// </summary>
    /// <param name="state">El estado de señal enviada. Ej. true -> abrir puerta, false -> cerrar puerta</param>
    protected void SendToActivables(bool state)
    {
        isPressed = state;
        foreach (Activable act in activables)
        {
            act.Activar(state);
        }
    }

    /// <summary>
    /// El método para cambiar automáticamente el estado del Activador a su opuesto.
    /// </summary>
    protected void Switch()
    {        
        SendToActivables(!isPressed);
    }
}
