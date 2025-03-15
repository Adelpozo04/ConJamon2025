using UnityEngine;
using System.Collections;

public class PerkTwin : MonoBehaviour
{
    [SerializeField]
    public float floatStrength = 0.5f; // Amplitud del movimiento
    [SerializeField]
    public float floatSpeed = 2f;      // Velocidad de oscilación
    [SerializeField]
    private float startY;              // Posición inicial en Y

    void Start()
    {
        startY = transform.position.y;
        StartCoroutine(FloatEffect());
    }

    IEnumerator FloatEffect()
    {
        while (true) // Bucle infinito mientras el objeto esté activo
        {
            float newY = startY + Mathf.Sin(Time.time * floatSpeed) * floatStrength;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            yield return new WaitForSeconds(0.02f); // Espera un frame antes de continuar
        }
    }
}
