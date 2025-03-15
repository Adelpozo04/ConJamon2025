using UnityEngine;

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

    private void Awake()
    {
        myTransform = transform;    
        rb = GetComponent<Rigidbody2D>();
        //Singleton, pero sin que se guarde entre escenas
        Instance = this;
    }
    private void Start()
    {
        if(target != null && goalTransform == null) myTransform.position = new Vector3(target.position.x, target.position.y, -1000);
    }

    private void FixedUpdate()
    {
        if (target != null || goalTransform != null)
        {
            Vector3 myPos = new Vector3(myTransform.position.x, myTransform.position.y);
            
            Vector3 targetPos;            
            if(goalTransform == null)targetPos = new Vector3(target.position.x, target.position.y, 0);
            else targetPos = new Vector3(goalTransform.position.x, goalTransform.position.y, 0);

            float distance = Vector3.Distance(targetPos, myPos);


            Vector3 targetVelocity = new Vector3();


            if (distance > followUmbral)
            {

                Vector3 dir = (targetPos - myPos).normalized;

                float desplazamiento = (dir * followVelocity).magnitude * Time.fixedDeltaTime;

                //print(desplazamiento);
                //print(distance);

                if (desplazamiento > distance)
                {
                    targetVelocity = dir * distance / Time.fixedDeltaTime;
                }
                else
                {
                    targetVelocity = dir * followVelocity;
                }
            }
            else
            {
                //targetVelocity = Vector3.zero;
            }

            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, targetVelocity, lerpTime);
            //myTransform.position = new Vector3( target.position.x,target.position.y, myTransform.position.z);
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
}
