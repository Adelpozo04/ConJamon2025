using UnityEngine;

public class ExplosionBehaviour : PerkBehaviour
{
    [Header("Explosion Settings")]
    [Tooltip("Radio de la explosión")]
    [SerializeField] float explosionRadius = 5f;

    [Tooltip("Prefab del efecto visual de la explosión (opcional)")]
    public GameObject explosionEffectPrefab;

    public override void ActivateEffect() {
        // Instanciamos el efecto visual de la explosión, si se ha asignado
        if (explosionEffectPrefab != null) {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            GameUI.Instance.RemovePerk();
        }

        // Obtenemos todos los colliders dentro del radio de la explosión en las capas definidas
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D col in colliders) {
            // Si el collider tiene el componente LivesComponent, llamamos a Die()
            LivesComponent enemy = col.GetComponent<LivesComponent>();
            if (enemy != null) {
                enemy.EnemyDie();
            } else {
                // Si tiene el componente ExplosionDestructible, llamamos a ActivateDestruction()
                ExplosionDestructible destructible = col.GetComponent<ExplosionDestructible>();
                if (destructible != null) {
                    destructible.ActivateDestruction();
                }
            }
        }
    }

    // Dibujar la esfera de explosión en el editor para visualizar el área de efecto
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}

