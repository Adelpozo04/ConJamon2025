using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


/*
Este componente se encarga de guardar la lista  de grabaciones de toda la escena,
es singleton y pervive entre escenas
 */
public class SombraStorage : MonoBehaviour
{
    //IMPORTANTEEEEE: SI LAS SOMBRAS FALLAN PROBAR A CAMBIAR  ESTO
    public static  float positionCompareUmbral = 0.5f;



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


    public struct PlatformState
    {
        public bool isInContact;

        public bool active;
        public int current;
        public Vector3 position;
    }

    public struct DoorState
    {
        public bool isInContact;
        public bool flag;
        public Vector3 position;
    }

    public struct RocaState
    {
        public bool isInContact;
        public Vector3 position;
    }


    //guarda el callback original, el momento en que se ejecutó y el tipo de accion que fue
    public struct SombraAction
    {
        public CustomCallbackContext callback;
        public double time;
        public ActionType type;
        public Vector3 position;

        public PlatformState platformState;
        public DoorState doorState;
        public RocaState rocaState;

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

        if (target._copyPosition)
        {
            if (comparePlatformInfo(sombraAction, target) && compareDoorInfo(sombraAction,target) &&
                compareRockInfo(sombraAction, target))
            {
                target.transform.position = sombraAction.position;
            }
            else
            {
                target._copyPosition = false;
            }
        }



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


    public static SombraAction getCancelInput(ActionType type, PlayerMovement target)
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

        action.position = target.transform.position;

        action.callback = callback;

        return action;
    }


    public static void runCancelAllInputs(PlayerMovement target)
    {
        runAction(getCancelInput(ActionType.MOVE, target), target);
        runAction(getCancelInput(ActionType.JUMP, target), target);
        runAction(getCancelInput(ActionType.SHOOT, target), target);
        runAction(getCancelInput(ActionType.AIM, target), target);
    }


    //true equals, false algo distinto
    public static bool comparePlatformInfo(SombraAction sombraAction, PlayerMovement target)
    {
        bool bothContact = sombraAction.platformState.isInContact == (target._contactPlatform != null);
        bool noContact = !sombraAction.platformState.isInContact && (target._contactPlatform == null);

        if (noContact) return true;
        if (!bothContact) return false;  


        bool bothActive = sombraAction.platformState.active == target._contactPlatform._activado;
        bool bothCurrent =  sombraAction.platformState.current == target._contactPlatform.current;
        bool bothPos = Vector3.Distance(sombraAction.platformState.position, target._contactPlatform.transform.position) < positionCompareUmbral;


        return bothContact && bothActive && bothCurrent && bothPos;
    }


    public static bool compareDoorInfo(SombraAction sombraAction, PlayerMovement target)
    {
        bool bothContact = sombraAction.doorState.isInContact == (target._contactDoor != null);
        bool noContact = !sombraAction.doorState.isInContact && (target._contactDoor == null);

        if (noContact) return true;
        if (!bothContact) return false;

        bool bothFlag = sombraAction.doorState.flag == target._contactDoor.flag;
        bool bothPos = Vector3.Distance(sombraAction.doorState.position, target._contactDoor.transform.position) < positionCompareUmbral;


        return bothContact && bothFlag  && bothPos;
    }


    public static bool compareRockInfo(SombraAction sombraAction, PlayerMovement target)
    {
        bool bothContact = sombraAction.rocaState.isInContact == (target._contactRock != null);
        bool noContact = !sombraAction.rocaState.isInContact && (target._contactRock == null);

        if (noContact) return true;
        if (!bothContact) return false;

        bool bothPos = Vector3.Distance(sombraAction.rocaState.position, target._contactRock.transform.position) < positionCompareUmbral;


        return bothContact && bothPos;
    }



    //limpia la lista de fantasmas
    public void clearRecords()
    {
        _records.Clear();
    }
    
}
