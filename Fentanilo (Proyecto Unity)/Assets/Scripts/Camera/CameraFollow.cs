using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using UnityEngine.XR;

public class CameraFollow : MonoBehaviour
{
    //Singleton
    public static CameraFollow Instance { get; private set; }

    public Transform target;

    Transform myTransform;
    Rigidbody2D rb;

    Transform goalTransform;

    public float followVelocity;
    public float followUmbral;
    public float lerpTime;

    //public Vector2 _posActivacionMov;

    PlayerInput _input;
    private InputAction _move;
    private InputAction _shoot;
    private InputAction _jump;
    private InputAction _nextIteration;
    private InputAction _resetLevel;

    bool done = false;

    private void Awake()
    {
        myTransform = transform;
        rb = GetComponent<Rigidbody2D>();
        _input = target.GetComponent<PlayerInput>();

        _move = _input.actions["Move"];
        _shoot = _input.actions["Shoot"];
        _jump = _input.actions["Jump"];
        _nextIteration = _input.actions["NextIteration"];
        _resetLevel = _input.actions["ResetLevel"];

        //Singleton, pero sin que se guarde entre escenas
        Instance = this;
    }
    private void Start()
    {
        if (target != null && goalTransform == null) myTransform.position = new Vector3(target.position.x, target.position.y, -1000);
        //BloqueoInputs();
    }

    private void FixedUpdate()
    {
        if (target != null || goalTransform != null)
        {
            TPinitPos();

            Vector3 myPos = new Vector3(myTransform.position.x, myTransform.position.y);
            Vector3 targetPos = goalTransform == null ? target.position : goalTransform.position;

            float distance = Vector3.Distance(targetPos, myPos);
            Vector3 targetVelocity = Vector3.zero;

            if (distance > followUmbral)
            {
                Vector3 dir = (targetPos - myPos).normalized;
                float desplazamiento = (dir * followVelocity).magnitude * Time.fixedDeltaTime;

                targetVelocity = desplazamiento > distance ? dir * distance / Time.fixedDeltaTime : dir * followVelocity;
            }

            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, targetVelocity, lerpTime);

            
        }
    }

    public void setGoalTransform(Transform g)
    {
        if (LevelManager.Instance.checkGoalTransform())
        {
            goalTransform = g;
            myTransform.position = new Vector3(goalTransform.position.x, goalTransform.position.y, -1000);
        }
    }

    public void destroyGoalTransform()
    {
        goalTransform = null;
    }

    public void TPinitPos()
    {
        if (done != true)
        {
            Vector2 move = _input.actions["Move"].ReadValue<Vector2>();
            if (move.x != 0)
            {
                transform.position = new Vector3(target.transform.position.x, target.transform.position.y, -1000);
                goalTransform = null;
                done = true;
            }

            if (_shoot.IsPressed() || _jump.IsPressed() || _nextIteration.IsPressed() || _resetLevel.IsPressed())
            {
                transform.position = new Vector3(target.transform.position.x, target.transform.position.y, -1000);
                goalTransform = null;
                done = true;
            }
        }
    }
}
