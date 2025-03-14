using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements.Experimental;
using UnityEngine.InputSystem;
using Unity.VisualScripting;


public class ImpulsoBehaviour : PerkBehaviour
{
    public override void ActivateEffect()
    {
        gameObject.AddComponent<ImpulsoEffect>();
        GameUI.Instance.RemovePerk();
    }
}
