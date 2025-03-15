using UnityEngine;

public class EnemyShotComponent : MonoBehaviour
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
                animator.SetTrigger("Die");
            }            
        }
    }

    public void OnAnimationEnd()
    {
        Destroy(gameObject);
    }
}
