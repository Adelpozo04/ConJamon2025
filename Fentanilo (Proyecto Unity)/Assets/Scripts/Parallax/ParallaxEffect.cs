using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float parallaxSpeed;

    private Vector3 lastCameraPosition;

    public float textureUnitSizeX;

    private void Start()
    {
        lastCameraPosition = cameraTransform.position;
    }

    private void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        transform.position += new Vector3(deltaMovement.x * parallaxSpeed, deltaMovement.y * 1, 0);
        lastCameraPosition = cameraTransform.position;

        // Reposicionar el fondo si se sale de la pantalla
        if (Mathf.Abs(cameraTransform.position.x - transform.position.x) >= textureUnitSizeX)
        {
            float offsetPositionX = (cameraTransform.position.x - transform.position.x) % textureUnitSizeX;
            transform.position = new Vector3(cameraTransform.position.x + offsetPositionX, transform.position.y, transform.position.z);
        }
    }
}