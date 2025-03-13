using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class SombraStorage : MonoBehaviour
{

    public List<SombraAction> _record = new List<SombraAction>();


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
        TEST,MOVE,JUMP,SHOOT
    }

    //guarda el callback original, el momento en que se ejecutó y el tipo de accion que fue
    public struct SombraAction
    {
        public CustomCallbackContext callback;
        public double time;
        public ActionType type;
    }

    [SerializeField]
    SombraTest _sombraTarget;


    double _startTime = 0;
    bool _runningShadow = false;



    private void Start()
    {
    }   

    public void startShadow()
    {
        _startTime = Time.time;

        _runningShadow = true;
    }

    // Update is called once per frame
    void Update()
    {



        if (_runningShadow)
        {
            //print(_record.Count);
            if (_record.Count > 0)
            {
                double currTime = Time.time - _startTime;
                if (_record[0].time >= currTime)
                {
                    runAction(_record[0]);
                    _record.RemoveAt(0);
                }
            }
        }
    }

    void runAction(SombraAction sombraAction)
    {
        //if else con todas las funciones
        if (sombraAction.type == ActionType.TEST) {

            print("replicando accion, time:" + sombraAction.time + "  callback started:" + sombraAction.callback.started);
            _sombraTarget.test(sombraAction.callback);
        }
        //...
    }


    public void startShadow(InputAction.CallbackContext callback)
    {
        print("empezar a replicar");
        startShadow();
    }

    public static CustomCallbackContext convertCallbackContext(InputAction.CallbackContext callback)
    {

        CustomCallbackContext customCallbackContext = new CustomCallbackContext();
        customCallbackContext.started = callback.started;
        customCallbackContext.canceled = callback.canceled;
        customCallbackContext.performed = callback.performed;

        customCallbackContext.valueVector2 = callback.ReadValue<Vector2>();

        return customCallbackContext; 
    }

}
