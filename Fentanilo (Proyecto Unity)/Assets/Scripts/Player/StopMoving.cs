using UnityEngine;

public class StopMoving : MonoBehaviour
{
    public float velocityUmbral;
    public Rigidbody2D rb;
    
    void Start()
    {
        
    }

    void Update()
    {

        if(rb.linearVelocity.magnitude < velocityUmbral)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }
}
