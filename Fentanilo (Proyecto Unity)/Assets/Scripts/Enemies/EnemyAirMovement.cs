using Unity.VisualScripting;
using UnityEngine;

public class EnemyAirMovement : MonoBehaviour
{
    SpriteRenderer _spriteRenderer;
    Animator _animator;

    [SerializeField] private Transform _firstPoint;  // Primer extremo del movimiento
    [SerializeField] private Transform _secondPoint;  // Segundo extremo del movimiento
    [SerializeField] private float _speed = 1f;  // Velocidad del movimiento
    [SerializeField] private float _inclination = 15f; // Grado de angulación del semiarco

    private float t = 0f;  // Parámetro de interpolación
    private bool _advancing = true; // Dirección del movimiento

    [SerializeField] private float _cooldown = 1f; // Tiempo de espera en cada punto
    private float _cooldownTimer = 0f; // Temporizador del cooldown
    private bool _onCooldown = false; // Indica si está en cooldown

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Si está en cooldown, esperar antes de moverse
        if (_onCooldown)
        {
            //////////_animator.SetBool("isMoving", false); // Activa animación de cooldown////////// ESTA ANIMACION ES PARA CUANDO ESTA PARADO
            _cooldownTimer -= Time.deltaTime;
            if (_cooldownTimer <= 0f) _onCooldown = false; // Reanudar el movimiento
            return;
        }

        //////////_animator.SetBool("isMoving", true); // Activa animación de movimiento////////// ESTA ANIMACION ES PARA CUANDO ESTA EN MOV

        // Interpolación entre el primer punto y el segundo
        Vector3 posicionLinea = Vector3.Lerp(_firstPoint.position, _secondPoint.position, t);

        // Cálculo de la altura del arco
        float altura = Mathf.Sin(t * Mathf.PI) * Mathf.Tan(_inclination * Mathf.Deg2Rad * -1) * Vector3.Distance(_firstPoint.position, _secondPoint.position);

        // Posicionamos al enemigo con la altura correspondiente
        transform.position = new Vector3(posicionLinea.x, posicionLinea.y + altura, posicionLinea.z);

        // Ajuste del parámetro t en función de la dirección
        if (_advancing)
        {
            t += _speed * Time.deltaTime;
            if (t >= 1f) ActivarCooldown(false); // Llega al segundo punto y activa cooldown
            _spriteRenderer.flipX = false;
        }
        else
        {
            t -= _speed * Time.deltaTime;
            if (t <= 0f) ActivarCooldown(true); // Llega al primer punto y activa cooldown
            _spriteRenderer.flipX = true;
        }
    }

    void ActivarCooldown(bool nuevaDireccion)
    {
        t = Mathf.Clamp01(t); // Asegurar que no pase de los límites
        _advancing = nuevaDireccion; // Cambiar dirección
        _onCooldown = true; // Activar cooldown
        _cooldownTimer = _cooldown; // Reiniciar temporizador
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var playerMovement = collision.collider.gameObject.GetComponent<PlayerMovement>();

        if (playerMovement != null)
        {
            if (playerMovement._recording)
            {
                playerMovement._controller.stopRecording();
            }

            playerMovement.OnDeath();
        }
    }
}
