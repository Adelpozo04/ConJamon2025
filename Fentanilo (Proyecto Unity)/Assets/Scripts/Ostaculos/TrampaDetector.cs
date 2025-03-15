using UnityEngine;

public class TrampaDetector : MonoBehaviour
{

    public Animator animator;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(Animator.StringToHash("Idle") == animator.GetCurrentAnimatorStateInfo(0).fullPathHash)
        {
            //si se esta ejecutando el idle
            animator.Play("Attack");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Animator.StringToHash("Idle") == animator.GetCurrentAnimatorStateInfo(0).fullPathHash)
        {
            //si se esta ejecutando el idle
            animator.Play("Attack");
        }
    }
}
