using UnityEngine;

public class DoorController : Activable
{
    private bool _opened;
    
    public override void Activar(bool state)
    {
        if (state)
        {
            Open();
        }
        else
        {
            Close();
        }
    }
    
    void Open() {
        if(!_opened) {
            //Abrir la puerta
            //Efectuar animación de apertura
            _opened = true;
        }
    }
    
    void Close() {
        if (_opened) {
            //Cerrar la puerta
            //Efectuar animación de cierre
            _opened = false;
        }
    }
}