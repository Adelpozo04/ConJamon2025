using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements.Experimental;
using UnityEngine.InputSystem;
using Unity.VisualScripting;


public class ImpulsoBehaviour : PerkBehaviour
{
    public GameObject ImpulsoPrefab;
    public override void ActivateEffect()
    {
        GameObject go = Instantiate(ImpulsoPrefab);
        go.transform.position = transform.position;

        GameUI.Instance.RemovePerk();

        Destroy(gameObject);
    }
}
