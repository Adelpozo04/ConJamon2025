using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

public class EnemyCannon : MonoBehaviour
{
    [SerializeField] GameObject _player;
    [SerializeField] GameObject _shotPrefab;
    [SerializeField] Transform _spawnPoint;

    [SerializeField] float _shootingDelay; // Tiempo entre disparos
    [SerializeField] float _shotSpeed; // Velocidad del disparo
    [SerializeField] float _raycastDistance; // Distancia del raycast
    [SerializeField] float _flipDelay; // Tiempo de espera antes de girar
    [SerializeField] LayerMask _playerLayer; // Capa del player para el raycast

    private Coroutine _flipCoroutine;
    private Vector2 _initPos;
    private bool _lookingRight = true;
    private bool _playerDetected = false;
    private float _lastShootTime = 0f; // Última vez que disparó

    private void Start()
    {
        _initPos.x = transform.position.x;
    }

    private void Update()
    {
        transform.position = new Vector2(_initPos.x, transform.position.y);
        DetectPlayer();

        // Rotar el cañón en función de la posición del jugador
        if (transform.position.x < _player.transform.position.x && !_lookingRight)
        {
            StartFlipCoroutine();
        }
        else if (transform.position.x > _player.transform.position.x && _lookingRight)
        {
            StartFlipCoroutine();
        }
    }

    private void DetectPlayer()
    {
        Vector2 direction = _lookingRight ? Vector2.right : Vector2.left;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, _raycastDistance, _playerLayer);

        Debug.DrawRay(transform.position, direction * _raycastDistance, Color.red);

        if (hit.collider != null && hit.collider.gameObject == _player)
        {
            _playerDetected = true;

            // Disparar solo si ha pasado el tiempo de espera entre disparos
            if (Time.time >= _lastShootTime + _shootingDelay)
            {
                Shoot();
                _lastShootTime = Time.time; // Actualiza el tiempo del último disparo
            }
        }
        else
        {
            _playerDetected = false;
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
