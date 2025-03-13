using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class DoorController : Activable
{
    private SpriteRenderer renderer;
    private Collider2D collider;
    private bool _opened;

    private void Start()
    {
        collider = GetComponent<Collider2D>();
        renderer = GetComponent<SpriteRenderer>();
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
            collider.enabled = false;
            renderer.color = Color.green;
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