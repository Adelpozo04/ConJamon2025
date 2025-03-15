using UnityEngine;

public class EnemyMeleeMovement : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    [SerializeField] private MovementState _currentState; // Estado actual del movimiento
    [SerializeField] private LayerMask _playerLayer; // Capa para detectar al jugador
    
    [SerializeField] private float _followingRange;  // Distancia del Raycast
    [SerializeField] private float _speed; // Velocidad de movimiento
    

    private Transform _playerTransform; // Referencia al transform del jugador
    private bool _lookingRight = true; // Variable para saber si el enemigo está mirando a la derecha

    [SerializeField] private Transform[] _patrolPositions;
    private int _randomNumber;
    private float _distancePP = 0.25f; //Distancia en la que reconoce que llega al punto de patrullaje

    private Animator _animator;

    public enum MovementState
    {
        Patrolling,
        Following,
    }

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _randomNumber = Random.Range(0, _patrolPositions.Length);
        _animator= GetComponent<Animator>();
    }

    private void Update()
    {
        switch (_currentState)
        {
            case MovementState.Patrolling:
                PatrollingState();
                break;
            case MovementState.Following:
                FollowingState();
                break;
        }
    }

    private void PatrollingState()
    {
        FlipToTarget(_patrolPositions[_randomNumber].position);

        Vector3 _newPos = new Vector3(_patrolPositions[_randomNumber].position.x, transform.position.y, transform.position.z);
        transform.position = Vector2.MoveTowards(transform.position, _newPos, _speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, _patrolPositions[_randomNumber].position) < _distancePP ||
            transform.position.x == _patrolPositions[_randomNumber].position.x)
        {
            _randomNumber = Random.Range(0, _patrolPositions.Length);
        }

        // Determinar la dirección del raycast: hacia la derecha o izquierda dependiendo de si el enemigo está mirando a la derecha
        Vector2 direction = _lookingRight ? Vector2.right : Vector2.left;

        // Lanzar un Raycast para detectar al jugador
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, _followingRange, _playerLayer);

        // Dibujar el Raycast en la Scene para depuración
        Debug.DrawRay(transform.position, direction * _followingRange, Color.red);

        // Si el raycast golpea al jugador, cambia al estado de "Following"
        if (hit.collider != null)
        {
            _playerTransform = hit.collider.transform;
            _currentState = MovementState.Following;
        }
        else
        {
            _playerTransform = null;
        }

        _animator.SetBool("followPlayer", false);
    }

    public void FollowingState()
    {
        if (_playerTransform == null)
        {
            _currentState = MovementState.Patrolling;
            return;
        }

        // Moverse hacia la posición del jugador
        Vector3 _posToFollow = new Vector3(_playerTransform.position.x, transform.position.y, 0);
        transform.position = Vector2.MoveTowards(transform.position, _posToFollow, _speed * Time.deltaTime);

        Vector2 direction = _lookingRight ? Vector2.right : Vector2.left;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, _followingRange, _playerLayer);
        Debug.DrawRay(transform.position, direction * _followingRange, Color.blue);

        if (hit.collider != null)
        {
            _playerTransform = hit.collider.transform;
        }
        else
        {
            _playerTransform = null;
        }

        _animator.SetBool("followPlayer", true);
    }

    private void FlipToTarget(Vector3 _target)
    {
        if (_target.x > transform.position.x && !_lookingRight)
        {
            _lookingRight = true;
            _spriteRenderer.flipX = false; // Mirar a la derecha
        }
        else if (_target.x < transform.position.x && _lookingRight)
        {
            _lookingRight = false;
            _spriteRenderer.flipX = true; // Mirar a la izquierda
        }
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

            Destroy(playerMovement.gameObject);
        }
    }
}
