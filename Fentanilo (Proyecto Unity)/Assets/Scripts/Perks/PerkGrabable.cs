using UnityEngine;

public class PerkGrabable : MonoBehaviour
{   
    private PerkBehaviour perkBehaviour;
    private GameObject player;
    private bool activated = false;
    void Start()
    {
        perkBehaviour = GetComponent<PerkBehaviour>();    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!activated && LayerMask.LayerToName(collision.gameObject.layer) == "Player")
        {
            player = collision.gameObject;
            perkBehaviour.ActivateEffect(player);
            activated = true;
        }
    }
}

