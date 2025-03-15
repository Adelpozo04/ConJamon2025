using System.Collections.Generic;
using UnityEngine;

public class TrampaComponent : MonoBehaviour
{
    //tiempo que tarda en empezar a caer desde que detecta al jugador
    public float _delayTime;


    //tiempo que tarda en caer desde que empieza a caer hasta que choca con el suelo
    public float _atackTime;

    //tiempo que tarda en subir y en resetearse
    public float _recoveryTime;


    //layers con las que va a caer, para caer se lanza un rayo con estas layers para obtener el punto destino,
    //y se calcula la velocidad teniendo en cuenta los tiempos
    //normalmente estaran ground y plataforma (si plataforma tiene una layer propia)
    public List<LayerMask> stopFall;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    //movemos el objeto usando velocity y fixedDelta time
    private void FixedUpdate()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //si hemos visto a un player
        if(collision.gameObject.GetComponent<PlayerMovement>()!= null)
        {
            //lanzar empezar a caer


        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //por si en la subida ha vuelto a poners el player aqui
        //si hemos visto a un player
        if (collision.gameObject.GetComponent<PlayerMovement>()!= null)
        {
            //lanzar empezar a caer


        }
    }


}
