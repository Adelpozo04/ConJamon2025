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
    -metodo register, comprobando que no se est� en pausa ni nada raro
    
    -booleano recording, para el prefab que si esta grabando
    -el resto de sombras lo tienen a false y NO tienen asignados los unity events

    -si el objeto muere, guardar un comando especial
    -los eventos de da�o no se guardan, solo el de muerte directamente


    -manager con las listas de comandos almacenadas (para todos los ghosts)
    -manager tiene las referencias a los ghosts y va dandoles su input


     */
    

    double _startTime = 0;

    [SerializeField]
    bool _recording = true;

    [SerializeField]
    SombraStorage _storage;



    void record(SombraStorage.CustomCallbackContext callback)
    {
        SombraStorage.SombraAction sombraAction = new SombraStorage.SombraAction();

        sombraAction.callback = callback;
        sombraAction.time = Time.time - _startTime;

        _storage._currentRecord.Add(sombraAction);
        print(_storage._currentRecord.Count);
        print(_storage.gameObject.name);

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

        test(SombraStorage.convertCallbackContext(callback));
    }

    public void test(SombraStorage.CustomCallbackContext callback)
    {

        if (callback.started)
        {
            if (_recording)
            {
                record(callback);
            }

            print("tu vieja from object:" + gameObject.name);

            transform.position+= new Vector3(0, 2, 0);
        }

    }

}
