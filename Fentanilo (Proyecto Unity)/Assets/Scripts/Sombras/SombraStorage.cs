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

    //tipos de acciones que almacenamos
    public enum ActionType
    {
        MOVE,JUMP,SHOOT,AIM,STOP_RECORDING
    }

    public struct SombraAction
    {
        public Vector2 direction;
        public bool pressed;
        public ActionType type;
        public double time;

        public SombraAction(Vector2 dir, bool pres, ActionType typ, double tim)
        {
            direction = dir;
            pressed = pres;
            type = typ;
            time = tim;
        }
    }

    public class SombraState
    {
        public PlayerMovement playerMovement;

        public Vector2 movementDirection;
        public bool jumpPressed;
        public bool shootPressed;
        public bool nextIterationPressed;

        public bool alive;

        public SombraState(PlayerMovement pM)
        {
            playerMovement = pM;
            movementDirection = new Vector2(0,0);
            jumpPressed = false;
            shootPressed = false;
            nextIterationPressed = false;
            alive = true;
        }
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

    //limpia la lista de fantasmas
    public void clearRecords()
    {
        _records.Clear();
    }
    
}
