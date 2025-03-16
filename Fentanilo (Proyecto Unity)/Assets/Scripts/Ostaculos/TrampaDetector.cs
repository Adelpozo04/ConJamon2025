using UnityEngine;

public class TrampaDetector : MonoBehaviour
{

    public Animator animator;
    [SerializeField] float attackCooldown;

    private float cont;

    private void Start() {
        cont = attackCooldown;
    }

    private void Update() {
        if(cont > 0) {
            cont -= Time.deltaTime;
        } else {
            animator.SetTrigger("Attack");
            cont = attackCooldown;
        }
    }


    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if(collision.gameObject.GetComponent<PlayerMovement>() != null)
    //    {
    //        //si se esta ejecutando el idle
    //        animator.SetBool("Attack",true);
    //    }
    //}

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.gameObject.GetComponent<PlayerMovement>() != null)
    //    {
    //        //si se esta ejecutando el idle
    //        animator.SetBool("Attack", true);
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.gameObject.GetComponent<PlayerMovement>() != null)
    //    {
    //        //si se esta ejecutando el idle
    //        animator.SetBool("Attack", false);
    //    }
    //}
}
