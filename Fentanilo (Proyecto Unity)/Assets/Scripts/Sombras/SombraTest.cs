using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class SombraTest : MonoBehaviour
{

    /*
    
    -lista de input.callback context (copiando los originales) + tiempo + tipo de evento (para saber a que funcion llamar)
    -para guardar el tiempo, initial time, ir restando (se asigna initial time en start)
    -con la lista, recorrerla (en update/corrutine) y llamar a los eventos segun el tipo con su callback
    -no usar corrutina por si hay menu de pausa
    -metodo register, comprobando que no se está en pausa ni nada raro
    
    -booleano recording, para el prefab que si esta grabando
    -el resto de sombras lo tienen a false y NO tienen asignados los unity events

    -si el objeto muere, guardar un comando especial
    -los eventos de daño no se guardan, solo el de muerte directamente


    -manager con las listas de comandos almacenadas (para todos los ghosts)
    -manager tiene las referencias a los ghosts y va dandoles su input


     */

    bool _recording = true;
    double _startTime = 0;

    SombraStorage _storage;



    void record(InputAction.CallbackContext callback)
    {
        SombraStorage.SombraAction sombraAction = new SombraStorage.SombraAction();

        sombraAction.callback = callback;
        sombraAction.time = Time.time - _startTime;

        _storage._record.Add(sombraAction);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void test (InputAction.CallbackContext callback){

        if (_recording)
        {
            record(callback);
        }

        print("tu vieja");

    }

}
