using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class SombraStorage : MonoBehaviour
{

    public List<SombraAction> _record;

    public enum ActionType
    {
        TEST,MOVE,JUMP,SHOOT
    }
    public struct SombraAction
    {
        public InputAction.CallbackContext callback;
        public double time;
        public ActionType type;
    }

    [SerializeField]
    SombraTest _sombraTarget;


    double _startTime = 0;
    bool _runningShadow = false;

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

            _sombraTarget.test(sombraAction.callback);
        }
    }


}
