using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private EnemyType _enemyType; // Tipo de enemigo (determina qué tipo de ataque usar)
    [SerializeField] private MovementState _currentState; // Estado actual del movimiento (esperando, siguiendo, etc.)
    [SerializeField] private LayerMask _playerLayer; // Capa para detectar al jugador (usado en OverlapSphere para verificar si el jugador está cerca)
    private Vector3 _startingPos; // Posición inicial del enemigo (cuando vuelve a esta posición)
    private Transform _playerTransform; // Referencia al transform del jugador
    [SerializeField] private float _searchRange; // Rango de búsqueda
    [SerializeField] private float _maxDistance; // Distancia máxima que se va a mover
    [SerializeField] private float _attackDistance; // Distancia para atacar
    [SerializeField] private float _speed, _speedReductor; // Velocidad de movimiento y factor para reducir la velocidad al atacar
    private float _maxSpeed; // Velocidad máxima (inicializada con la velocidad normal)
    private bool _lookingRight; // Variable para saber si el enemigo está mirando a la derecha (para invertir la dirección)
    //private Animator _anim;

    [SerializeField] private Transform[] _patrolPositions;
    private int _randomNumber;
    private float _distancePP = 0.25f; //Distancia en la que reconoce que llega al punto de patrullaje

    enum EnemyType
    {
        Meelee,
        Distance,
        Turret,
        Flying,
        Trap,
    }

    public enum MovementState
    {
        Patrolling,
        Following,
        Returning,
        Attacking,
    }

    private void Start()
    {
        _startingPos = transform.position;
        _maxSpeed = _speed;
        //_anim = GetComponent<Animator>();

        _randomNumber = Random.Range(0, _patrolPositions.Length);
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
        //empieza desde el startingpoint, una vez está en él, patrulla entre los puntos predefinidos
        FlipToTarget(_patrolPositions[_randomNumber].position);

        if (_enemyType == EnemyType.Meelee)
        {
            Vector3 _newPos = new Vector3(_patrolPositions[_randomNumber].position.x, transform.position.y, transform.position.z);
            transform.position = Vector2.MoveTowards(transform.position, _newPos, _speed * Time.deltaTime);
        }
        else if (_enemyType == EnemyType.Flying)
        {
            transform.position = Vector2.MoveTowards(transform.position, _patrolPositions[_randomNumber].position, _speed * Time.deltaTime);
        }

        if (Vector2.Distance(transform.position, _patrolPositions[_randomNumber].position) < _distancePP)
        {
            _randomNumber = Random.Range(0, _patrolPositions.Length);
        }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _searchRange, _playerLayer);
        Debug.Log(colliders.Length);
        if (colliders.Length > 0)
        {
            _playerTransform = colliders[0].transform;
            _currentState = MovementState.Following;
        }
        else
        {
            _playerTransform = null;
        }
    }

    public void FollowingState()
    {
        if (_playerTransform == null)
        {
            _currentState = MovementState.Patrolling;
            return;
        }

        if (_enemyType == EnemyType.Meelee)
        {
            Vector3 _posToFollow = new Vector3(_playerTransform.position.x, transform.position.y, 0);
            transform.position = Vector2.MoveTowards(transform.position, _posToFollow, _speed * Time.deltaTime);
        }

        //else if (_enemyType == EnemyType.Flying)
        //{
        //    if (transform.position.x > _playerTransform.position.x)
        //    {
        //        Vector3 _posToFollow = new Vector3(_playerTransform.position.x, transform.position.y, 0);
        //        transform.position = Vector2.MoveTowards(transform.position, _posToFollow, _speed * Time.deltaTime);
        //    }
        //    else if (transform.position.x < _playerTransform.position.x)
        //    {
        //        Vector3 _posToFollow = new Vector3(_playerTransform.position.x - 1.5f, transform.position.y + 1, 0);
        //        transform.position = Vector2.MoveTowards(transform.position, _posToFollow, _speed * Time.deltaTime);
        //    }
        //}

        //transform.position = Vector2.MoveTowards(transform.position, _playerTransform.position, _speed * Time.deltaTime);

        FlipToTarget(_playerTransform.position);

        if (/*Vector2.Distance(transform.position, _startingPos) > _searchRange ||*/
            Vector2.Distance(transform.position, _playerTransform.position) > _searchRange)
        {
            _currentState = MovementState.Patrolling;
            _playerTransform = null;
        }
        else if (Vector2.Distance(transform.position, _playerTransform.position) < _attackDistance)
        {
            _currentState = MovementState.Attacking;
        }
    }

    private void FlipToTarget(Vector3 _target)
    {
        if (_target.x > transform.position.x && !_lookingRight)
        {
            Flip();
        }
        else if (_target.x < transform.position.x && _lookingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        _lookingRight = !_lookingRight;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var playerMovement = collision.collider.gameObject.GetComponent<PlayerMovement>();

        if (playerMovement != null)
        {
            /*
            if (playerMovement._recording)
            {
                playerMovement._controller.stopRecording();
            }
            */

            Destroy(playerMovement.gameObject);

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _searchRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _maxDistance);
    }
}
