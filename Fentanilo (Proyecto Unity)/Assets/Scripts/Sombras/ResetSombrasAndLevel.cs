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

    public void OnReset()
    {
        SombraStorage.Instance.clearRecords();
        LevelManager.Instance.RestartLevel(SceneManager.GetActiveScene().name);
    }

}
