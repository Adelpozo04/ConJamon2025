using UnityEngine;
using UnityEngine.InputSystem;

public class ImpulsoEffectPlayer : MonoBehaviour
{
    public PlayerMovement playerMovement;

    public ImpulsoEffect impulsoEffect;
    public bool active;
    public void onAim(InputAction.CallbackContext context)
    {
        var customContext = SombraStorage.convertCallbackContext(context);
        if (playerMovement._recording)
        {
            playerMovement.record(customContext, SombraStorage.ActionType.AIM);
        }
        if (active)
            OnAim(customContext);
    }
    public void OnAim(SombraStorage.CustomCallbackContext context)
    {
        if (active && context.started)
        {
            TryAim(context.valueVector2);
        }
    }

    private void TryAim(Vector2 inputDirection)
    {
        if (inputDirection.sqrMagnitude > 0.1f)
        {
            impulsoEffect.Aim(inputDirection);
        }
    }

    public void ActivateAiming(ImpulsoEffect comp)
    {
        impulsoEffect = comp;
        active = true;
    }
    public void DeactivateAiming()
    {
        active = false;
    }

    void Start()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
