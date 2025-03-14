using UnityEngine;


public enum PerkType { Roca, Explosion, Impulso }
public class PerkGrabable : MonoBehaviour
{   
    [SerializeField] private PerkType perkType;

    [Header("Prefabs")]
    [SerializeField] GameObject rocaPrefab;
    [SerializeField] GameObject explosionEffectPrefab;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Player")
        {
            
            GameObject player = collision.gameObject;
            bool setted = false;
            if (player.GetComponent<RocaBehaviour>() == null
                && player.GetComponent<ExplosionBehaviour>() == null
                && player.GetComponent<ImpulsoBehaviour>() == null)
            {
                switch (perkType)
                {
                    case PerkType.Roca:
                        if (player.GetComponent<RocaBehaviour>() == null)
                        {
                            RocaBehaviour roca = player.AddComponent<RocaBehaviour>();
                            roca.rocaPrefab = rocaPrefab;
                            setted = true;
                        }
                        break;
                    case PerkType.Explosion:
                        if (player.GetComponent<ExplosionBehaviour>() == null)
                        {
                            ExplosionBehaviour exp = player.AddComponent<ExplosionBehaviour>();
                            exp.explosionEffectPrefab = explosionEffectPrefab;
                            setted = true;
                        }
                        break;
                    case PerkType.Impulso:
                        if (player.GetComponent<ImpulsoBehaviour>() == null)
                        {
                            player.AddComponent<ImpulsoBehaviour>();
                            setted = true;
                        }
                        break;
                }
            }

            if (setted)
            {
                gameObject.SetActive(false);

                // si no es sombra
                if (collision.gameObject.name == "Player")
                    GameUI.Instance.AddPerk(perkType);
            }
        }
    }
}

