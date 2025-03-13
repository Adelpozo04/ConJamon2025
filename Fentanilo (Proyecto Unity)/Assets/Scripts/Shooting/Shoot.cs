using UnityEngine;
using UnityEngine.InputSystem;


public class Shoot : MonoBehaviour
{
    [Header("Referencias")]

    [Tooltip("Prefab de la bala que se disparará")]
    [SerializeField] GameObject bulletPrefab;
    [Tooltip("Punto de salida de la bala")]
    [SerializeField] Transform firePoint;


    [Header("Propiedades del Disparo")]

    [Tooltip("Cadencia de disparo (segundos entre disparos)")]
    [SerializeField] float fireRate = 0.2f;
    [Tooltip("Retroceso aplicado al jugador al disparar")]
    [SerializeField] float recoilForce = 0f;
    [Tooltip("Número máximo de balas activas al mismo tiempo (-1 para ilimitado)")]
    [SerializeField] int maxActiveBullets = -1;


    [Header("Propiedades de la Bala")]

    [Tooltip("Velocidad de la bala")]
    [SerializeField] float bulletSpeed = 30f;
    [Tooltip("Tiempo de vida de la bala antes de autodestruirse")]
    [SerializeField] float bulletLifetime = 2f;
    [Tooltip("Capas que detectarán las colisiones de la bala")]
    [SerializeField] LayerMask bulletCollisionLayers;


    private float lastShotTime;
    private Rigidbody2D playerRb;
    private int activeBullets = 0;
    private Vector2 aimDirection = Vector2.right;

    private void Awake() {
        playerRb = GetComponent<Rigidbody2D>();
    }

    public void OnShoot(InputAction.CallbackContext context) {
        if (context.started) {
            TryShoot();
        }
    }

    public void OnAim(InputAction.CallbackContext context) {
        // Lee la dirección del input del joystick o teclas
        Vector2 inputDirection = context.ReadValue<Vector2>();

        // Solo se cambia la dirección si hay suficiente input
        if (inputDirection.sqrMagnitude > 0.1f) {
            // Compara las magnitudes absolutas para elegir la dirección dominante
            if (Mathf.Abs(inputDirection.x) >= Mathf.Abs(inputDirection.y)) {
                // Dominio horizontal
                aimDirection = (inputDirection.x > 0) ? Vector2.right : Vector2.left;
            } else {
                // Dominio vertical
                aimDirection = (inputDirection.y > 0) ? Vector2.up : Vector2.down;
            }

            // Actualiza la rotación de firePoint para reflejar la dirección cardinal
            if (aimDirection == Vector2.right) {
                firePoint.rotation = Quaternion.Euler(0, 0, 0);
            } else if (aimDirection == Vector2.left) {
                firePoint.rotation = Quaternion.Euler(0, 0, 180);
            } else if (aimDirection == Vector2.up) {
                firePoint.rotation = Quaternion.Euler(0, 0, 90);
            } else if (aimDirection == Vector2.down) {
                firePoint.rotation = Quaternion.Euler(0, 0, -90);
            }
        }
    }


    private void TryShoot() {
        if (Time.time - lastShotTime < fireRate)
            return;

        if (maxActiveBullets != -1 && activeBullets >= maxActiveBullets)
            return;

        ShootBullet();
        lastShotTime = Time.time;
    }

    private void ShootBullet() {
        if (bulletPrefab == null || firePoint == null)
            return;

        // Usamos la rotación del firePoint (ya ajustada en OnAim) para instanciar la bala
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

        if (bulletRb != null) {
            // La bala se dispara en la dirección cardinal (aimDirection)
            bulletRb.linearVelocity = aimDirection * bulletSpeed;
        }

        // Aplica retroceso al jugador si está configurado
        if (recoilForce > 0 && playerRb != null) {
            playerRb.AddForce(-aimDirection * recoilForce, ForceMode2D.Impulse);
        }

        activeBullets++;
        bullet.GetComponent<Bullet>()?.InitializeBullet(this, bulletLifetime, bulletCollisionLayers);
    }

    public void OnBulletDestroyed() {
        activeBullets--;
    }
}
