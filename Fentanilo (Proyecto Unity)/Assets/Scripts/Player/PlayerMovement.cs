using UnityEngine;
using UnityEngine.InputSystem;

/**
 * Player Movement implementa:
 *      Movimiento horizontal
 *      Salto vertical si el jugador está onGround (tocando un Tag "Ground")
 *      Multiplicador de gravedad en caida de salto
 *      Coyote Jump
 *      
 *      Es recomendable combinar los atributos jumpForce y fallMultiplayer con una gravedad bastante elevada.
 *      Esto mejora el gamefeeling del salto, ganando dinamismo.
 */

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;

    [Header("Mejora del Salto")]
    [Tooltip("Multiplicador de gravedad cuando el jugador esta cayendo")]
    [SerializeField] float fallMultiplier;
    [Tooltip("Tiempo (en segundos) durante el cual se guarda la intención de salto")]
    [SerializeField] float jumpBufferTime;

    [Header("Coyote Jump")]
    [Tooltip("Tiempo (en segundos) durante el cual se permite saltar después de salir del suelo")]
    [SerializeField] float coyoteTime;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool onGround;
    private float coyoteTimeCounter;
    private float jumpBufferCounter;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    // Metodo para manejar el movimiento (llamado por el Input System)
    public void OnMove(InputAction.CallbackContext context) {
        OnMove(SombraStorage.convertCallbackContext(context));
    }

    // Metodo para manejar el salto (llamado por el Input System)
    public void OnJump(InputAction.CallbackContext context) {
        OnJump(SombraStorage.convertCallbackContext(context));
    }

    // Metodo para manejar el movimiento 
    public void OnMove(SombraStorage.CustomCallbackContext context)
    {
        moveInput = context.valueVector2;
    }

    // Metodo para manejar el salto 
    public void OnJump(SombraStorage.CustomCallbackContext context)
    {
        if (context.started) {
            jumpBufferCounter = jumpBufferTime;
        }
    }

    // Ejecuta el salto asignando la velocidad vertical y resetea el coyote time
    private void Jump() {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        // Reseteamos contadores para evitar saltos múltiples
        coyoteTimeCounter = 0;
        jumpBufferCounter = 0;
    }

    private void Update() {

        // Si hay una intención de salto guardada y se cumple que el jugador puede saltar,
        // se ejecuta el salto.
        if (jumpBufferCounter > 0 && (onGround || coyoteTimeCounter > 0)) {
            Jump();
        }

        // Mientras el jugador esté en el suelo, reiniciamos el contador de coyoteTime
        if (onGround) {
            coyoteTimeCounter = coyoteTime;
        } else {
            // Se va decrementando conforme el jugador permanece en el aire
            coyoteTimeCounter -= Time.deltaTime;
        }

        // Se decrementa el contador del buffer de salto si está activo
        if (jumpBufferCounter > 0) {
            jumpBufferCounter -= Time.deltaTime;
        }
    }

    private void FixedUpdate() {
        // Actualiza la velocidad horizontal seg�n la entrada
        float horizontalVelocity = moveInput.x * moveSpeed;
        // Obtiene la velocidad vertical actual
        float verticalVelocity = rb.linearVelocity.y;

        // Si el jugador esta cayendo (velocidad vertical negativa), se aplica mayor gravedad
        if (verticalVelocity < 0) {
            verticalVelocity += Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }

        // Se actualiza la velocidad del rigidbody con la velocidad horizontal calculada y la vertical modificada
        rb.linearVelocity = new Vector2(horizontalVelocity, verticalVelocity);
    }

    // Determina cu�ndo el jugador esta en contacto con el suelo
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Ground")) {
            onGround = true;
        }
    }

    // Detecta cuando el jugador deja de estar en contacto con el suelo
    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Ground")) {
            onGround = false;
        }
    }
}
