using UnityEngine;
using UnityEngine.InputSystem;

public class ImpulsoEffectPlayer : MonoBehaviour
{
    public PlayerMovement playerMovement;

    public ImpulsoEffect impulsoEffect;
    public bool aiming;
    private bool shooting = false;

    private void TryAim(Vector2 inputDirection)
    {
        impulsoEffect.Aim(inputDirection);
    }

    public void ActivateAiming(ImpulsoEffect comp)
    {
        impulsoEffect = comp;
        playerMovement.DisableMovement();
        aiming = true;
    }
    public void DeactivateAiming()
    {
        shooting = true;
        aiming = false;
    }

    private void FixedUpdate()
    {
        if (aiming)
            TryAim(playerMovement.moveInput);
    }

    void Start()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (shooting && LayerMask.LayerToName(collision.gameObject.layer) == "Ground")
        {
            playerMovement.EnableMovement();
            shooting = false;
        }
    }
}
