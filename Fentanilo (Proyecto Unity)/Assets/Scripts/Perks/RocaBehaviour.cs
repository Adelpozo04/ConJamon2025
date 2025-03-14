using UnityEngine;

public class RocaBehaviour : PerkBehaviour
{
    public GameObject rocaPrefab;
    public override void ActivateEffect()
    {;
        GameObject go = Instantiate(rocaPrefab);
        go.transform.position = transform.position;
        GameUI.Instance.RemovePerk();
    }
}
