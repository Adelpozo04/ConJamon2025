using UnityEngine;

[RequireComponent(typeof(Animator))]
public class LivesComponent : MonoBehaviour
{
    [SerializeField] int lives = 1;

    private Animator animator;

    private void Start()
    {
        
        animator= GetComponent<Animator>();

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Bullet>() != null)
        {
            Hit();
            Destroy(other.gameObject);
        }
    }

    public void Hit()
    {
        lives--;
        if (lives <= 0)
        {
            EnemyDie();
        }
    }

    public void EnemyDie() {

        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().gravityScale = 0;

        EnemyMeleeMovement EMM = GetComponent<EnemyMeleeMovement>();

        if(EMM != null)
        {

            EMM.enabled= false;

        }

        EnemyCannon EC = GetComponent<EnemyCannon>();

        if(EC != null)
        {

            EC.enabled= false;

        }

        EnemyAirMovement EAM = GetComponent<EnemyAirMovement>();

        if(EAM != null)
        {

            EAM.enabled= false;

        }

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity= Vector3.zero;
        animator.SetTrigger("Die");
    }

    public void OnAnimationEnd()
    {
        Destroy(gameObject);
    }
}
