using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

public class EnemyCannon : MonoBehaviour
{
    [SerializeField] GameObject _player;
    [SerializeField] GameObject _shotPrefab;
    [SerializeField] Transform _spawnPoint;

    [SerializeField] int _burstCount; //Count of shots
    [SerializeField] float _shootingDelay; //Time between shots in burst
    [SerializeField] float _burstDelay; //Time between bursts
    public float _speed;
    private bool _lookingRight = true;
    private Vector2 _initPos;
 
    private void Start()
    {
        StartCoroutine(CanonSequence());
        _initPos.x = transform.position.x;
    }

    private void Update()
    {
        transform.position = new Vector2(_initPos.x, transform.position.y);

        if (transform.position.x < _player.transform.position.x && _lookingRight == false)
        {
            Flip();
        }
        else if (transform.position.x > _player.transform.position.x && _lookingRight == true)
        {
            Flip();
        }
    }

    private void Shoot()
    {
        GameObject instantiatedShot;
        instantiatedShot = GameObject.Instantiate(_shotPrefab, _spawnPoint.position, _spawnPoint.rotation);

        instantiatedShot.GetComponent<ProyectileBehaviour>().SetSpeed(_speed);

        //instantiatedShot.GetComponent<ProyectileBehaviour>().ChangeParent(transform);
    }

    IEnumerator CanonSequence()
    {
        while (true) //Repetir bucle infinitamente
        {
            //Repetir bucle hasta acabar ráfaga
            for (int _counter = 0; _counter < _burstCount; _counter++)
            {
                //Disparar
                Shoot();
                //Esperar entre disparos
                yield return new WaitForSeconds(_shootingDelay);
            }
            //Esperar entre ráfagas
            yield return new WaitForSeconds(_burstDelay);
        }
    }

    private void Flip()
    {
        _lookingRight = !_lookingRight;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
        _speed *= -1;
    }
}
