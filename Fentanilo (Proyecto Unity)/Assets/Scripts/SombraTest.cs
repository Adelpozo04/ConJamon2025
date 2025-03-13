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



    [System.Serializable]
    public struct FentaAction
    {
       public UnityEvent _event;


    }

    [System.Serializable]
    public struct FentaCallbackContext
    {
        public bool _started;
        public bool _performed;
        public bool _canceled;
    }

    public FentaAction _action;

    public FentaCallbackContext _fentaContext;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void test (InputAction.CallbackContext context){

        print("tu vieja");

        InputAction.CallbackContext callbackContext = new InputAction.CallbackContext();


    }

}
