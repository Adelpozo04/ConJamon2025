using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

public class EnemyCannon : MonoBehaviour
{
    Animator _anim;

    [SerializeField] GameObject _shotPrefab;
    [SerializeField] Transform _spawnPoint;
    [SerializeField] float _shootingDelay;
    [SerializeField] float _shotSpeed;
    [SerializeField] float _raycastForwardDistance;
    [SerializeField] float _raycastBackwardDistance;
    [SerializeField] float _flipDelay;
    [SerializeField] LayerMask _playerLayer;

    private Coroutine _flipCoroutine;
    private Vector2 _initPos;
    private bool _lookingRight = true;
    private bool _playerDetected = false;
    private float _lastShootTime = 0f;
    private Transform _detectedPlayerTransform;


    private AudioSource audioSource;
    private float startVolume;

    public void UpdateSFXVolume(float volume)
    {
        audioSource.volume = Mathf.Lerp(0, startVolume, volume);
    }
    private void Start()
    {
        _anim = GetComponent<Animator>();
        _initPos.x = transform.position.x;

        audioSource = GetComponent<AudioSource>();
        startVolume = audioSource.volume;
    }

    private void Update()
    {
        transform.position = new Vector2(_initPos.x, transform.position.y);
        DetectPlayer();
    }

    private void DetectPlayer()
    {
        Vector2 forwardDirection = _lookingRight ? Vector2.right : Vector2.left;
        Vector2 backwardDirection = -forwardDirection;

        RaycastHit2D hitForward = Physics2D.Raycast(transform.position, forwardDirection, _raycastForwardDistance, _playerLayer);
        Debug.DrawRay(transform.position, forwardDirection * _raycastForwardDistance, Color.green);

        RaycastHit2D hitBackward = Physics2D.Raycast(transform.position, backwardDirection, _raycastBackwardDistance, _playerLayer);
        Debug.DrawRay(transform.position, backwardDirection * _raycastBackwardDistance, Color.blue);

        if (hitForward.collider != null)
        {
            _playerDetected = true;
            _detectedPlayerTransform = hitForward.collider.transform;
            TryShoot();
        }
        else if (hitBackward.collider != null)
        {
            _playerDetected = true;
            _detectedPlayerTransform = hitBackward.collider.transform;
            StartFlipCoroutine();
        }
        else
        {
            _playerDetected = false;
            _anim.SetBool("_isShooting", false);
            _detectedPlayerTransform = null;
        }
    }

    private void TryShoot()
    {
        if (_playerDetected && _detectedPlayerTransform != null && Time.time >= _lastShootTime + _shootingDelay)
        {
            _anim.SetBool("_isShooting", true);
            _lastShootTime = Time.time;

            audioSource.clip = AudioManager.Instance.GetAudioClip(SoundSFX.SHOOT);
            audioSource.Play();
        }
    }

    private void Shoot()
    {
        GameObject instantiatedShot = Instantiate(_shotPrefab, _spawnPoint.position, Quaternion.identity);
        instantiatedShot.GetComponent<ProyectileBehaviour>().SetSpeed(_lookingRight ? _shotSpeed : -_shotSpeed);
    }

    private void StartFlipCoroutine()
    {
        if (_flipCoroutine == null)
        {
            _flipCoroutine = StartCoroutine(DelayedFlip());
        }
    }

    private IEnumerator DelayedFlip()
    {
        yield return new WaitForSeconds(_flipDelay);

        _lookingRight = !_lookingRight;
        transform.eulerAngles = new Vector3(0, _lookingRight ? 0 : 180, 0);
        _flipCoroutine = null;
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
