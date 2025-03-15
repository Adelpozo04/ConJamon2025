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
            lives--;
            if (lives <= 0)
            {
                EnemyDie();
            }            
        }
    }

    public void EnemyDie() {
        animator.SetTrigger("Die");
    }

    public void OnAnimationEnd()
    {
        Destroy(gameObject);
    }
}
