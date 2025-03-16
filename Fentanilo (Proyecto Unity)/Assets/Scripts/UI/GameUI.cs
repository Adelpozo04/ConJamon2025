using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public enum TipoPerk { }
public class GameUI : MonoBehaviour
{
    [SerializeField] private Image perkImageHolder;
    [Header("Setting shadows UI")]
    [SerializeField] private Color currentShadowColor;
    [SerializeField] private Color inactiveShadowColor;
    [SerializeField] private Color activeShadowColor;
    [SerializeField] private GameObject shadowHolder;
    [SerializeField] private GameObject shadowUIPrefab;
    [Header("Sprites UI")]
    [SerializeField] private Sprite perkNone;
    [SerializeField] private Sprite perkRoca;
    [SerializeField] private Sprite perkExplosion;
    [SerializeField] private Sprite perkImpulso;

    private List<Image> shadowsUI;

    private int numShadowsTotal;
    private int numShadowsActive;

    public static GameUI Instance = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }


    public void AddPerk(PerkType perkType)
    {
        switch (perkType)
        {
            case PerkType.Roca:
                perkImageHolder.sprite = perkRoca;
                break;
            case PerkType.Explosion:
                perkImageHolder.sprite = perkExplosion;
                break;
            case PerkType.Impulso:
                perkImageHolder.sprite = perkImpulso;
                break;
        }
    }

    public void RemovePerk()
    {
        perkImageHolder.sprite = perkNone;
    }

    public void StartUI(int numShadowsTotal, int numShadowsActive)
    {
        shadowsUI = new List<Image>();

        for (int i = 0; i < numShadowsTotal; i++)
        {
            GameObject shadowUI = Instantiate(shadowUIPrefab, shadowHolder.transform);

            Image shadowUIImage = shadowUI.GetComponent<Image>();
            if (i < numShadowsActive)
                shadowUIImage.color = activeShadowColor;
            else if (i > numShadowsActive)
                shadowUIImage.color = inactiveShadowColor;
            else
                shadowUIImage.color = currentShadowColor;

        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
