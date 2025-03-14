using UnityEngine;

public class EnemyShotComponent : MonoBehaviour
{
    [SerializeField] int lives = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Bullet>() != null)
        {
            lives--;
            if (lives <= 0)
            {
                Destroy(gameObject);
            }            
        }
    }
}
