using UnityEngine;

public abstract class Activable : MonoBehaviour
{
    /// <summary>
    /// Método a ser llamado por el objeto activador.
    /// Debe ser implementado por la clase hija para la funcionalidad deseada.
    /// </summary>
    /// <param name="state">El estado de señal recibido Ej. true -> abrir puerta, false -> cerrar puerta</param>
    public abstract void Activar(bool state);
}
