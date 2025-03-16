using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ProyectileBehaviour : MonoBehaviour
{
    [SerializeField] private float _selfDestroyTime;
    private float _speed;

    private void Start()
    {
        Destroy(gameObject, _selfDestroyTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        Destroy(this.gameObject);
    }

    private void Update()
    {
        Vector3 _position = transform.localPosition;
        _position = new Vector3(_position.x + _speed * Time.deltaTime, _position.y, _position.z);
        transform.position = _position;
    }

    public void SetSpeed(float _newSpeed)
    {
        _speed = _newSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var playerMovement = collision.gameObject.GetComponent<PlayerMovement>();

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
