using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

public class EnemyCannon : MonoBehaviour
{
    Animator _anim;

    [SerializeField] GameObject _shotPrefab;
    [SerializeField] Transform _spawnPoint;

    [SerializeField] float _shootingDelay; // Tiempo entre disparos
    [SerializeField] float _shotSpeed; // Velocidad del disparo
    [SerializeField] float _raycastForwardDistance; // Distancia del raycast delantero
    [SerializeField] float _raycastBackwardDistance; // Distancia del raycast delantero
    [SerializeField] float _flipDelay; // Tiempo de espera antes de girar
    [SerializeField] LayerMask _playerLayer; // Capa del player para el raycast

    private Coroutine _flipCoroutine;
    private Vector2 _initPos;
    private bool _lookingRight = true;
    private bool _playerDetected = false;
    private float _lastShootTime = 0f; // Última vez que disparó
    private Transform _detectedPlayerTransform; // Guarda la posición del jugador detectado

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _initPos.x = transform.position.x;
    }

    private void Update()
    {
        transform.position = new Vector2(_initPos.x, transform.position.y);
        DetectPlayer();

        // Si hay un jugador detectado, ajustamos la rotación
        if (_playerDetected && _detectedPlayerTransform != null)
        {
            if (transform.position.x < _detectedPlayerTransform.position.x && !_lookingRight)
            {
                StartFlipCoroutine();
            }
            else if (transform.position.x > _detectedPlayerTransform.position.x && _lookingRight)
            {
                StartFlipCoroutine();
            }
        }
    }

    private void DetectPlayer()
    {
        Vector2 forwardDirection = _lookingRight ? Vector2.right : Vector2.left;
        Vector2 backwardDirection = -forwardDirection; // Dirección opuesta

        // Raycast hacia adelante
        RaycastHit2D hitForward = Physics2D.Raycast(transform.position, forwardDirection, _raycastForwardDistance, _playerLayer);
        Debug.DrawRay(transform.position, forwardDirection * _raycastForwardDistance, Color.green);

        // Raycast hacia atrás
        RaycastHit2D hitBackward = Physics2D.Raycast(transform.position, backwardDirection, _raycastBackwardDistance, _playerLayer);
        Debug.DrawRay(transform.position, backwardDirection * _raycastBackwardDistance, Color.blue);

        // Si detectamos al jugador en cualquiera de los dos raycasts
        if (hitForward.collider != null)
        {
            _playerDetected = true;
            _detectedPlayerTransform = hitForward.collider.transform;
        }
        else if (hitBackward.collider != null)
        {
            _playerDetected = true;
            _detectedPlayerTransform = hitBackward.collider.transform;
            StartFlipCoroutine(); // Si el jugador está detrás, giramos de inmediato
        }
        else
        {
            _playerDetected = false;
            _anim.SetBool("_isShooting", false);
            _detectedPlayerTransform = null;
        }

        // Disparar solo si el jugador es detectado y ha pasado el tiempo de espera
        if (_playerDetected && Time.time >= _lastShootTime + _shootingDelay)
        {
            _anim.SetBool("_isShooting", true);
            _lastShootTime = Time.time;
        }
    }

    private void Shoot()
    {
        GameObject instantiatedShot = Instantiate(_shotPrefab, _spawnPoint.position, _spawnPoint.rotation);
        instantiatedShot.GetComponent<ProyectileBehaviour>().SetSpeed(_shotSpeed);
    }

    private void StartFlipCoroutine()
    {
        if (_flipCoroutine == null) // Evita iniciar múltiples veces la misma corrutina
        {
            _flipCoroutine = StartCoroutine(DelayedFlip());
        }
    }

    private IEnumerator DelayedFlip()
    {
        yield return new WaitForSeconds(_flipDelay);

        _lookingRight = !_lookingRight;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
        _shotSpeed *= -1;

        _flipCoroutine = null; // Restablece la variable para permitir nuevas rotaciones
    }

}
