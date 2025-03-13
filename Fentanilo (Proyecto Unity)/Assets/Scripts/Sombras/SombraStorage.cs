using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class SombraStorage : MonoBehaviour
{

    //singleton para manejar una sola instancia
    public static SombraStorage Instance = null;



    //grabacion actual
    public List<SombraAction> _currentRecord = new List<SombraAction>();


    //lista de todas las grabaciones
    public List<List<SombraAction>> _records = new List<List<SombraAction>>();


    [SerializeField]
    int _maxRecords;


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
    PlayerMovement _sombraTarget;


    double _startTime = 0;
    bool _runningShadow = false;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;    
            //para conservar entre escenas
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += onSceneLoaded;
        }   
        else
        {
            Destroy(this);
        }
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
            if (_currentRecord.Count > 0)
            {
                double currTime = Time.time - _startTime;
                if (currTime  >= _currentRecord[0].time)
                {
                    runAction(_currentRecord[0],_sombraTarget);
                    _currentRecord.RemoveAt(0);
                }
            }
        }
    }

    void runAction(SombraAction sombraAction,PlayerMovement target)
    {
        //if else con todas las funciones
        if (sombraAction.type == ActionType.TEST) {

            print("replicando accion, time:" + sombraAction.time + "  callback started:" + sombraAction.callback.started);
            //_sombraTarget.test(sombraAction.callback);
        }

        if (sombraAction.type == ActionType.JUMP)
        {
            target.OnJump(sombraAction.callback);
        }
        else if (sombraAction.type == ActionType.MOVE)
        {
            target.OnMove(sombraAction.callback);
        }


        //...
    }


    public void startShadow(InputAction.CallbackContext callback)
    {
        print("starrrrt");
        startShadow();
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


    public void stopRecording(InputAction.CallbackContext callback)
    {
        if (callback.started) {
            _records.Add(_currentRecord);
            _currentRecord.Clear();
            reloadScene();
        }
    }

    public void clearRecords()
    {
        _records.Clear();
        _currentRecord.Clear();
    }

    void reloadScene()
    {
        SceneManager.UnloadSceneAsync("SombrasScene");
        SceneManager.LoadScene("SombrasScene");
    }

    void onSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //crear y ejecutar todas las sombras

    }


}
