using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

[RequireComponent(typeof(PlayerInput))]
public class ResetSombrasAndLevel : MonoBehaviour
{
    PlayerInput input;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        input = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        if (input.actions["ResetLevel"].IsPressed()) {
            SombraStorage.Instance.clearRecords();
            SceneManager.UnloadSceneAsync(gameObject.scene);
            SceneManager.LoadScene(gameObject.scene.name);
        }
    }
}
