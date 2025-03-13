using UnityEngine;

public class RocaPerkEffect : PerkBehaviour
{
    [SerializeField] private GameObject rocaPrefab;
    public override void ActivateEffect()
    {;
        Instantiate(rocaPrefab);
    }
}
