using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class DoorController : Activable
{
    private SpriteRenderer _rend;
    private Collider2D _coll;
    private bool _opened;

    private void Start()
    {
        _coll = GetComponent<Collider2D>();
        _rend = GetComponent<SpriteRenderer>();
    }

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
            _coll.enabled = false;
            _rend.color = Color.green;
            Debug.Log("door opened");
            //Efectuar animación de apertura
            _opened = true;
        }
    }
    
    void Close() {
        if (_opened) {
            //Cerrar la puerta
            
            _coll.enabled = true;
            _rend.color = Color.red;
            Debug.Log("door closed");
            //Efectuar animación de cierre
            
            _opened = false;
        }
    }
}