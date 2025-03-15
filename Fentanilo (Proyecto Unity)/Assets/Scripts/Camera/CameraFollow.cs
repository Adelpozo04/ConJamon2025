using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    Transform myTransform;
    Rigidbody2D rb;   

    public float followVelocity;
    public float followUmbral;
    public float lerpTime;

    private void Awake()
    {
        myTransform = transform;    
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if(target != null) myTransform.position = new Vector3(target.position.x, target.position.y, -1000);
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 targetPos = new Vector3(target.position.x, target.position.y, 0);
            Vector3 myPos = new Vector3(myTransform.position.x, myTransform.position.y);
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
}
