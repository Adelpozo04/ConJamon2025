using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



/*
Este componente se encarga de guardar la lista  de grabaciones de toda la escena,
es singleton y pervive entre escenas
 */
public class SombraStorage : MonoBehaviour
{
    //singleton para manejar una sola instancia
    public static SombraStorage Instance = null;

    //lista de todas las grabaciones
    public List<List<SombraAction>> _records = new List<List<SombraAction>>();

    //wrapper de callbackContext
    public struct CustomCallbackContext
    {
        public bool started;
        public bool canceled;
        public bool performed;

        public Vector2 valueVector2;
    }

    //tipos de acciones que almacenamos
    public enum ActionType
    {
        MOVE,JUMP,SHOOT,AIM
    }

    //guarda el callback original, el momento en que se ejecutó y el tipo de accion que fue
    public struct SombraAction
    {
        public CustomCallbackContext callback;
        public double time;
        public ActionType type;
    }


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            //para conservar entre escenas
            gameObject.transform.parent = null;
            DontDestroyOnLoad(gameObject);
        }   
        else
        {
            Destroy(this);
        }
    }


    //controller
    public static void runAction(SombraAction sombraAction,PlayerMovement target)
    {
        //if else con todas las funciones
        if (sombraAction.type == ActionType.JUMP)
        {
            print("replicando accion, time:" + sombraAction.time + "  callback started:" + sombraAction.callback.started);
            target.OnJump(sombraAction.callback);
        }
        else if (sombraAction.type == ActionType.MOVE)
        {
            target.OnMove(sombraAction.callback);
        }
        else if (sombraAction.type == ActionType.SHOOT)
        {
            target.gameObject.GetComponentInChildren<Shoot>().OnShoot(sombraAction.callback);
        }
        else if (sombraAction.type == ActionType.AIM)
        {
            target.gameObject.GetComponentInChildren<Shoot>().OnAim(sombraAction.callback);
        }

        //...
    }

    //convierte un CallbackContext de unity a uno custom
    public static CustomCallbackContext convertCallbackContext(InputAction.CallbackContext callback)
    {

        CustomCallbackContext customCallbackContext = new CustomCallbackContext();

        //copiar todos los valores que nos interesan
        customCallbackContext.started = callback.started;
        customCallbackContext.canceled = callback.canceled;
        customCallbackContext.performed = callback.performed;

        //si el callback es de tipo vector2
        if (callback.valueType == typeof(Vector2)) { 
            customCallbackContext.valueVector2 = callback.ReadValue<Vector2>();
        }

        //mas copias de valores si fuera necesario


        return customCallbackContext; 
    }


    //limpia la lista de fantasmas
    public void clearRecords()
    {
        _records.Clear();
    }
    
}
