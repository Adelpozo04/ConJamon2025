using UnityEngine;
using UnityEngine.InputSystem;

public class ImpulsoEffectPlayer : MonoBehaviour
{
    public PlayerMovement playerMovement;

    public ImpulsoEffect impulsoEffect;
    public bool active;
    private bool shooting = false;
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
        TryAim(context.valueVector2);
    }

    private void TryAim(Vector2 inputDirection)
    {
        impulsoEffect.Aim(inputDirection);
    }

    public void ActivateAiming(ImpulsoEffect comp)
    {
        impulsoEffect = comp;
        playerMovement.enabled = false;
        active = true;
    }
    public void DeactivateAiming()
    {
        //playerMovement.enabled = true;
        shooting = true;
        active = false;
    }

    void Start()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (shooting && LayerMask.LayerToName(collision.gameObject.layer) == "Ground")
        {
            playerMovement.enabled = true;
            shooting = false;
        }
    }
}
