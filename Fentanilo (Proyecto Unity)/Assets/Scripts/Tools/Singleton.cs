using UnityEngine;

public class Singleton : MonoBehaviour
{
    public static Singleton Instance { get; private set; }

    private void Awake() {
        // Si no existe una instancia, la establecemos y marcamos el objeto para que no se destruya al cambiar de escena
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else if (Instance != this) {
            // Si ya existe una instancia, destruimos este objeto duplicado
            Destroy(gameObject);
        }
    }
}