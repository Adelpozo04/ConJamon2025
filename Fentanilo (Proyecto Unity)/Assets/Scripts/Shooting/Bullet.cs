using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Shoot shootScript;
    private LayerMask collisionLayers;

    private void Start() {
        
    }

    public void InitializeBullet(Shoot shooter, float lifetime, LayerMask layers) {
        shootScript = shooter;
        collisionLayers = layers;
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        // Comprobamos que la layer del objeto contra el que chocamos sea una de las layers con las que debemos colisionar.
        /*
        if (((1 << collision.gameObject.layer) & collisionLayers) != 0) {
            Destroy(gameObject);
        }*/        
        if(collision.gameObject.layer != 7) Destroy(gameObject);        
    }

    private void OnDestroy() {
        if (shootScript != null) {
            shootScript.OnBulletDestroyed();
        }
    }
}
