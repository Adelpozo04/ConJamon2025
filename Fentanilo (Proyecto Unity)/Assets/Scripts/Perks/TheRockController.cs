using System.Threading;
using UnityEngine;

public class TheRockController : MonoBehaviour
{
    /// <summary>
    /// El tiempo para la destrucción de la roca
    /// </summary>
    [SerializeField] private float lifeTime = 3;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("SelfDestroy", lifeTime);
    }

    void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
