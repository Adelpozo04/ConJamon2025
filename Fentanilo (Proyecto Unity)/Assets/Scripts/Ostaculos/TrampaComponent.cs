using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrampaComponent : MonoBehaviour
{
    //tiempo que tarda en empezar a caer desde que detecta al jugador
    public float _delayTime;

    //tiempo que tarda en caer desde que empieza a caer hasta que choca con el suelo
    public float _atackTime;

    //tiempo que tarda en subir y en resetearse
    public float _recoveryTime;

    //booleanos para controlar el estado de la trampa
    bool _inAction = false;
    bool _atacking = false;
    bool _recovering =  false;

    float timeCounter;


    public Rigidbody2D rb;

    //punto q se usa para calcular la distancia de desplazamiento
    public Transform movePoint;
    public Transform moveTransform;

    Vector2 targetVel = Vector2.zero;

    Vector3 startPos;


    //layers con las que va a caer, para caer se lanza un rayo con estas layers para obtener el punto destino,
    //y se calcula la velocidad teniendo en cuenta los tiempos
    //normalmente estaran ground y plataforma (si plataforma tiene una layer propia)
    public List<LayerMask> stopFall;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = moveTransform.position;  
    }

    //movemos el objeto usando velocity y fixedDelta time
    private void FixedUpdate()
    {
        if (_inAction)
        {
            timeCounter += Time.fixedDeltaTime;

            //print(timeCounter);
            //si todavia no hemos empezado a bajar
            if (!_atacking)
            {
                //si nos toca empezar a bajar
                if(timeCounter >= _delayTime)
                {
                    _atacking =true;
                    timeCounter = 0;


                    //lanzar rayo para ver el punto de colision
                    RaycastHit2D hit;

                    float distance = 0;

                    for (int i = 0; i < stopFall.Count; i++) { 
                    
                        hit = Physics2D.Raycast(movePoint.position, Vector2.down, Mathf.Infinity, stopFall[i]);

                        //si hemos chocado con algo
                        if(hit.collider != null)
                        {
                            distance = Vector2.Distance(hit.point, (Vector2)movePoint.position);

                            print(distance);

                            break;
                        }
                    }


                    //calcular la velocidad a partir de la distancia
                    float velocity = distance / _atackTime;

                    print(velocity);
                    //setear la velocidad de caida
                    targetVel = new Vector2(0, -velocity);
                    rb.linearVelocity = targetVel;
                }
            }
            else if(!_recovering)// si estamos bajando y todavia no hemos empezado a subir
            {
                //si nos toca empezar a subir
                if(timeCounter >= _atackTime)
                {
                    _recovering=true;
                    timeCounter = 0;


                    float distance = Vector2.Distance(startPos, (Vector2)moveTransform.position);

                    //calcular la velocidad a partir de la distancia
                    float velocity = distance / _recoveryTime;

                    //setear la velocidad de caida
                    targetVel = new Vector2(0, velocity);
                    rb.linearVelocity = targetVel;
                }
            }
            else
            {
                //si hemos terminado de recuperarnos 
                if(timeCounter >= _recoveryTime)
                {
                    timeCounter = 0;

                    rb.linearVelocity = Vector2.zero;   

                    _inAction = false;
                    _atacking = false;
                    _recovering = false;

                    moveTransform.position = startPos;
                }

            }
        }




    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //si hemos visto a un player
        if (collision.gameObject.GetComponent<PlayerMovement>()!= null)
        {
            //lanzar empezar a caer
            if (!_inAction)
            {
                _inAction = true;
                timeCounter = 0;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //por si en la subida ha vuelto a poners el player aqui
        //si hemos visto a un player
        if (collision.gameObject.GetComponent<PlayerMovement>()!= null)
        {
            //lanzar empezar a caer
            if (!_inAction)
            {
                _inAction = true;
                timeCounter = 0;
            }
        }
    }

}
