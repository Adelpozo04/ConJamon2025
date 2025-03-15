using UnityEngine;

public class PerkVisualizer : MonoBehaviour
{
    [SerializeField] private SpriteRenderer perkRenderer;
    [SerializeField] private SpriteRenderer gameIndicator;

    [Header("Sprites UI")]
    [SerializeField] private Sprite perkRoca;
    [SerializeField] private Sprite perkExplosion;
    [SerializeField] private Sprite perkImpulso;


    public void AddPerk(PerkType perkType) {
        gameIndicator.enabled = true;
        switch (perkType) {
            case PerkType.Roca:
                perkRenderer.sprite = perkRoca;
                break;
            case PerkType.Explosion:
                perkRenderer.sprite = perkExplosion;
                break;
            case PerkType.Impulso:
                perkRenderer.sprite = perkImpulso;
                break;
        }
    }

    public void RemovePerk() {
        gameIndicator.enabled = false;
        perkRenderer.sprite = null;
    }
}
