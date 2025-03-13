using System;
using UnityEngine;

public class MovingPlatformController : Activable
{
    private Rigidbody2D rb;
    //La platadorma debe desplazarse de un punto a otro.
    [SerializeField] private Vector3[] points;
    [SerializeField] private float speed;
    
    private bool _activado;
    //El punto del recorrido en el que está
    private int _current;
    //Está haciendo el recorrido al revés.
    private bool _inverse;
    public override void Activar(bool state)
    {
        _activado = state;
    }
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _current = 0;
    }

    private void Update()
    {
        if (_activado)
        {
            rb.MovePosition(Vector3.MoveTowards(transform.position, points[_current], speed * Time.deltaTime));
            //Si ha llegado al current point
            if ((points[_current] - transform.position).magnitude <= 1)
            {
                Next();
            }
        }
    }

    //Comienza el movimiento al siguiente punto.
    private void Next()
    {
        if (_current == 0 || _current == points.Length - 1)
            _inverse = !_inverse;
        if (_inverse)
        {
            _current++;
        }
        else
        {
            _current--;
        }
    }
}
