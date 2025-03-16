using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ColorIndicator : MonoBehaviour
{

    [SerializeField] SpriteRenderer colorIndicator;
    [SerializeField] List<Color> listaColores = new List<Color>();

    bool a = true;

    private int nSombras;

    void Start()
    {

            
    }

    private void Update() {
        if (GetComponent<PlayerInput>().enabled)
            SetColor(GetComponent<PlayerMovement>()._controller._currentRecordIndex);
    }
    public void SetColor(int n) {
        if(colorIndicator != null && n < listaColores.Count)
            colorIndicator.color = listaColores[n];
    }
}
