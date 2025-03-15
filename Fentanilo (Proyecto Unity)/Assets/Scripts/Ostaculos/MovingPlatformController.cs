using System;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class MovingPlatformController : Activable
{
    Vector2 platformVelocity;
    private Rigidbody2D rb;
    //La platadorma debe desplazarse de un punto a otro.
    [SerializeField] private Transform[] points;
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
        if (_activado && points.Length > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, points[_current].position, speed * Time.deltaTime);
            //Si ha llegado al current point
            if ((points[_current].position - transform.position).magnitude <= 1)
            {
                Next();
            }
        }
    }

    private void FixedUpdate()
    {
        platformVelocity = rb.linearVelocity;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerMovement>() != null)
        {
            other.transform.SetParent(transform);
        }
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<PlayerMovement>() != null)
        {
            other.transform.SetParent(null);
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
