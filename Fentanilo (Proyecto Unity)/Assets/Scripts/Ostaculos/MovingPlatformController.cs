using System;
using System.Linq;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class MovingPlatformController : Activable
{
    //La platadorma debe desplazarse de un punto a otro.
    [SerializeField] private Transform[] points;
    [SerializeField] private float speed;
    [SerializeField] private float cooldown;

    public bool _activado;

    public bool loop;

    //El punto del recorrido en el que está
    public int current;
    public bool forward = true;
    public override void Activar(bool state)
    {
        _activado = state;

        if (!loop)
        {
            // cambio pa atras o pa alante
            if (forward && !state && current != 0)
            {
                forward = false;
                current--;
            }
            else if (!forward && state && current < points.Count() - 1)
            {
                current++;
                forward = true;
            }
        }

    }
    
    private void Start()
    {
        current = 0;
    }

    private void Update()
    {
        if (loop)
        {
            if (_activado)
            {
                transform.position = Vector3.MoveTowards(transform.position, points[current].position, speed * Time.deltaTime);

                if ((points[current].position - transform.position).magnitude <= 0.1)
                {
                    if (forward)
                    {
                        if (current < points.Count() - 1)
                            current++;
                        else
                            forward = false;
                    }
                    else
                    {
                        if (current > 0)
                            current--;
                        else
                            forward = true;
                    }
                }
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, points[current].position, speed * Time.deltaTime);
            if ((points[current].position - transform.position).magnitude <= 0.1)
            {
                if (_activado && current < points.Count() - 1)
                {
                    current++;
                }
                else if (!_activado && current > 0)
                {
                    current--;
                }
            }
        }
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
}
