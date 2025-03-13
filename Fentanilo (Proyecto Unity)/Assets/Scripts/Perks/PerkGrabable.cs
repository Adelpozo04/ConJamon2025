using UnityEngine;


enum PerkType { Roca, Explosion, Impulso }
public class PerkGrabable : MonoBehaviour
{   
    [SerializeField] private PerkType perkType;
    private bool activated = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Player")
        {
            GameObject player = collision.gameObject;
            bool setted = false;

            switch (perkType)
            {
                case PerkType.Roca:
                    /*
                    if (player.GetComponent<RocaBehaviour>() == null){
                        player.AddComponent<RocaBehaviour>();
                        setted = true;
                    }
                    */
                    break;
                case PerkType.Explosion:
                    /*
                    if (player.GetComponent<ExplosionBehaviour>() == null){
                        player.AddComponent<ExplosionBehaviour>();
                        setted = true;
                    }
                    */
                    break;
                case PerkType.Impulso:
                    if (player.GetComponent<ImpulsoBehaviour>() == null){
                        player.AddComponent<ImpulsoBehaviour>();
                        setted = true;
                    }
                    break;
            }

            if (setted)
                gameObject.SetActive(false);
        }
    }
}

