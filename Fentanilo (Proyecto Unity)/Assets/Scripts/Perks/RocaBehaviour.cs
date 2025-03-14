using UnityEngine;

public class RocaBehaviour : PerkBehaviour
{
    public GameObject rocaPrefab;
    public override void ActivateEffect()
    {;
        Instantiate(rocaPrefab);
        GameUI.Instance.RemovePerk();
    }
}
