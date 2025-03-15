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
        MOVE,JUMP,SHOOT,AIM,STOP_RECORDING
    }

    //guarda el callback original, el momento en que se ejecutó y el tipo de accion que fue
    public struct SombraAction
    {
        public CustomCallbackContext callback;
        public double time;
        public ActionType type;
        public Vector3 position;
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

        if(target == null){
            print("target null at run action");
            return;
        }

        target.transform.position = sombraAction.position;


        //if else con todas las funciones
        if (sombraAction.type == ActionType.JUMP)
        {
            //print("replicando accion, time:" + sombraAction.time + "  callback started:" + sombraAction.callback.started);
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
        else if(sombraAction.type == ActionType.STOP_RECORDING){
            runCancelAllInputs(target);
            PerkBehaviour perk = target.GetComponent<PerkBehaviour>();
            if (perk != null)
            {
                target.GetComponent<PerkBehaviour>().ActivateEffect();
                Destroy(target.gameObject);
            }
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

            customCallbackContext.valueVector2.Normalize();
        }

        //mas copias de valores si fuera necesario


        return customCallbackContext; 
    }


    public static SombraAction getCancelInput(ActionType type)
    {
        SombraAction action = new SombraAction();


        action.time = 0;
        action.type = type;

        CustomCallbackContext callback = new CustomCallbackContext();

        //default, cambiar en cada caso si es necesario
        callback.started = false;
        callback.performed = false;

        callback.canceled = true;

        callback.valueVector2 = Vector2.zero;


        if (type == ActionType.MOVE)
        {
    
        }
        else if (type == ActionType.JUMP)
        {

        }
        else if (type == ActionType.SHOOT)
        {

        }
        else if (type == ActionType.AIM)
        {

        }


        action.callback = callback;

        return action;
    }


    public static void runCancelAllInputs(PlayerMovement target)
    {
        runAction(getCancelInput(ActionType.MOVE), target );
        runAction(getCancelInput(ActionType.JUMP), target);
        runAction(getCancelInput(ActionType.SHOOT), target);
        runAction(getCancelInput(ActionType.AIM), target);
    }


    //limpia la lista de fantasmas
    public void clearRecords()
    {
        _records.Clear();
    }
    
}
