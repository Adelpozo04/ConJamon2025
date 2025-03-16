using UnityEngine;

public class EnemyMeleeMovement : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    [SerializeField] private MovementState _currentState; // Estado actual del movimiento
    [SerializeField] private LayerMask _playerLayer; // Capa para detectar al jugador
    
    [SerializeField] private float _followingRange;  // Distancia del Raycast
    [SerializeField] private float _speed; // Velocidad de movimiento
    
    private Transform _playerTransform; // Referencia al transform del jugador
    private bool _lookingRight = true; // Variable para saber si el enemigo está mirando a la derecha

    [SerializeField] private Transform[] _patrolPositions;
    private int _actualPatrolPoint = 0; // Punto de patrullaje actual, empieza en 0 q es la pos del primero en el array
    private float _distancePP = 0.25f; // Distancia en la que reconoce que llega al punto de patrullaje

    AudioSource audioSource1;
    AudioSource audioSource2;
    private float startVolume1;
    private float startVolume2;
    public enum MovementState
    {
        Patrolling,
        Following,
    }

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();

        //_actualPatrolPoint = Random.Range(0, _patrolPositions.Length);
        _actualPatrolPoint = 0;

        audioSource1 = GetComponents<AudioSource>()[0];
        audioSource2 = GetComponents<AudioSource>()[1];

        audioSource1.clip = AudioManager.Instance.GetAudioClip(SoundSFX.ENEMY_HURT);
        audioSource2.clip = AudioManager.Instance.GetAudioClip(SoundSFX.MELEE_WALK);
        startVolume1 = audioSource1.volume;
        startVolume2 = audioSource2.volume;
    }

    public void UpdateSFXVolume(float volume)
    {
        audioSource1.volume = Mathf.Lerp(0, startVolume1, volume);
        audioSource2.volume = Mathf.Lerp(0, startVolume2, volume);
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
        FlipToTarget(_patrolPositions[_actualPatrolPoint].position);

        Vector3 _newPos = new Vector3(_patrolPositions[_actualPatrolPoint].position.x, transform.position.y, transform.position.z);
        transform.position = Vector2.MoveTowards(transform.position, _newPos, _speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, _patrolPositions[_actualPatrolPoint].position) < _distancePP ||
            transform.position.x == _patrolPositions[_actualPatrolPoint].position.x)
        {
            if (_actualPatrolPoint < _patrolPositions.Length - 1) { _actualPatrolPoint++; } else { _actualPatrolPoint = 0; }
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

            playerMovement.OnDeath();
        }
    }
}
