using UnityEngine;
using UnityEngine.InputSystem;

#region Game Feel Settings

[System.Serializable]
public class FallMultiplierSettings
{
    [Tooltip("Activar/desactivar el multiplicador de caída")]
    public bool enabled = true;
    [Tooltip("Multiplicador de gravedad cuando el jugador está cayendo")]
    public float fallMultiplier = 2f;
}

[System.Serializable]
public class FallControlSettings
{
    [Tooltip("Activar/desactivar el limitador de velocidad de caída")]
    public bool enabled = true;
    [Tooltip("Limitador de velocidad cuando el jugador está cayendo")]
    public float maxFallSpeed = 50f;
}

[System.Serializable]
public class JumpBufferSettings
{
    [Tooltip("Activar/desactivar el buffer de salto")]
    public bool enabled = true;
    [Tooltip("Tiempo (en segundos) durante el cual se guarda la intención de salto")]
    public float jumpBufferTime = 0.1f;
}

[System.Serializable]
public class JumpCutSettings
{
    [Tooltip("Activar/desactivar el salto variable (jump cut)")]
    public bool enabled = true;
    [Tooltip("Multiplicador para reducir la velocidad vertical al soltar el salto antes del pico")]
    public float jumpCutMultiplier = 0.5f;
}

[System.Serializable]
public class CoyoteJumpSettings
{
    [Tooltip("Activar/desactivar el coyote jump")]
    public bool enabled = true;
    [Tooltip("Tiempo (en segundos) durante el cual se permite saltar después de salir del suelo")]
    public float coyoteTime = 0.1f;
}

[System.Serializable]
public class SmoothMovementSettings
{
    [Tooltip("Activar/desactivar la aceleración y desaceleración suaves")]
    public bool enabled = true;
    [Tooltip("Tiempo en segundos para alcanzar la velocidad máxima")]
    public float accelerationTime = 0.1f;
    [Tooltip("Tiempo en segundos para detenerse cuando no hay input")]
    public float decelerationTime = 0.1f;
    [Tooltip("Si está activado, el jugador cambiará de dirección instantáneamente sin inercia")]
    public bool instantTurn = false;
}

#endregion

/**
 * PlayerMovement implementa:
 * 
 *      - Movimiento horizontal.
 *      - Salto vertical si el jugador está en el suelo (detección con raycast y layers físicas)
 *      - Multiplicador de caída
 *      - Limitador de velocidad en caida
 *      - Buffer de salto
 *      - Salto con fuerza variable
 *      - Coyote Jump
 *      - Movimiento suavizado
 *      
 * La activación, desactivación y parametrización de estos 6 comportamientos se realiza de forma cómoda desde el Inspector.
 * 
 * Se recomienda subir considerablemente la gravedad en la simulación física para mejorar el game feeling del salto.
 */

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    public SombrasController _controller;

    public GameObject colliderOnDead;


    [Header("Movimiento")]
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;

    [Header("Game Feel Settings")]
    [SerializeField] FallMultiplierSettings fallMultiplierSettings;
    [SerializeField] FallControlSettings fallControlSettings;
    [SerializeField] JumpBufferSettings jumpBufferSettings;
    [SerializeField] JumpCutSettings jumpCutSettings;
    [SerializeField] CoyoteJumpSettings coyoteJumpSettings;
    [SerializeField] SmoothMovementSettings smoothMovementSettings;

    [Header("Ground Detection")]
    [Tooltip("Capas que serán detectadas como suelo")]
    [SerializeField] LayerMask groundLayers;
    [Tooltip("Distancia del Raycast para detectar el suelo")]
    [SerializeField] float groundCheckDistance = 0.1f;
    [Tooltip("Posición desde donde se lanza el Raycast (relativa al jugador)")]
    [SerializeField] Vector2 groundCheckOffset = Vector2.zero;

    private Rigidbody2D rb;
    private bool onGround;
    private bool shouldJump;
    private float coyoteTimeCounter;
    private float jumpBufferCounter;
    private float currentSpeed;
    private float velocitySmoothing;

    private PlayerAudio playerAudio;

    private bool active = true;
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        playerAudio = GetComponentInChildren<PlayerAudio>();
    }

    public void DisableMovement()
    {
        active = false;
    }

    public void EnableMovement()
    {
        active = true;
    }

    /// <summary>
    /// Lanza un Raycast hacia abajo para detectar si estamos en el suelo.
    /// </summary>
    /// <returns>True si el Raycast detecta el suelo, False en caso contrario.</returns>
    private bool CheckGround()
    {
        Vector2 rayOrigin = (Vector2)transform.position + groundCheckOffset;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, groundCheckDistance, groundLayers);
        Debug.DrawRay(rayOrigin, Vector2.down * groundCheckDistance, hit ? Color.green : Color.red); // Debug visual

        return hit.collider != null;
    }
    public void MovementUpdate(Vector2 moveInput, bool pressedJump) {

        // Actualiza el contador de coyote jump si está activo.
        if (onGround)
        {
            coyoteTimeCounter = coyoteJumpSettings.enabled ? coyoteJumpSettings.coyoteTime : 0;
        }
        else
        {
            coyoteTimeCounter -= Time.fixedDeltaTime;
        }

        // Decrementa el contador del buffer de salto si está activo.
        if (jumpBufferCounter > 0)
        {
            jumpBufferCounter -= Time.fixedDeltaTime;
        }


        // Deteccion de suelo basada en raycast
        onGround = CheckGround();

        // el jump buffer AHORA NO VA pero no implementar
        if (jumpBufferSettings.enabled && jumpBufferCounter <= 0 &&
            (onGround || (coyoteJumpSettings.enabled && coyoteTimeCounter > 0)))
        {
            shouldJump = true;
        }

        // Calcula la velocidad horizontal según la entrada.

        float horizontalVelocity = moveInput.x * moveSpeed;
        // Obtiene la velocidad vertical actual.
        float verticalVelocity = rb.linearVelocity.y;

        if (shouldJump && pressedJump)
        {
            verticalVelocity = jumpForce;
            shouldJump = false;

            // Reseteamos Coyote Jump y Jump Buffer al saltar
            coyoteTimeCounter = 0;
            jumpBufferCounter = 0;

            playerAudio.PlayJump();
        }

        // Si el jugador está cayendo y el multiplicador de caída está activo, se aplica mayor gravedad.
        if (fallMultiplierSettings.enabled && verticalVelocity < 0)
        {
            verticalVelocity += Physics2D.gravity.y * (fallMultiplierSettings.fallMultiplier - 1) * Time.fixedDeltaTime;
        }

        // Aplicamos el limitador de velocidad en caida
        if (fallControlSettings.enabled && verticalVelocity < -fallControlSettings.maxFallSpeed)
        {
            verticalVelocity = -fallControlSettings.maxFallSpeed;
        }

        // Suavizamos el movimiento si el smooth está habilitado
        if (smoothMovementSettings.enabled)
        {
            if (moveInput.x != 0)
            {
                // Aplicamos aceleración si hay input.
                currentSpeed = Mathf.SmoothDamp(
                    currentSpeed,
                    horizontalVelocity,
                    ref velocitySmoothing,
                    smoothMovementSettings.accelerationTime
                );
            }
            else
            {
                // Aplicamos desaceleración si no hay input.
                currentSpeed = Mathf.SmoothDamp(
                    currentSpeed,
                    0,
                    ref velocitySmoothing,
                    smoothMovementSettings.decelerationTime
                );
            }

            // Permite cambiar de dirección instantáneamente si está activado.
            if (smoothMovementSettings.instantTurn && moveInput.x != 0)
            {
                currentSpeed = horizontalVelocity;
            }
        }
        else
        {
            currentSpeed = horizontalVelocity * Time.fixedDeltaTime;
        }


        rb.linearVelocity = new Vector2(currentSpeed, verticalVelocity);
    }

    #region Sombras


    

    private void OnDestroy()
    {

    }

    #endregion





}
