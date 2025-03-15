using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ColorIndicator : MonoBehaviour
{

    [SerializeField] SpriteRenderer colorIndicator;
    [SerializeField] List<Color> listaColores = new List<Color>();

    private int nSombras;

    void Start()
    {
        if(GetComponent<PlayerInput>().enabled)
            SetColor(SombraStorage.Instance._records.Count);
            
    }

    public void SetColor(int n) {
        print(name + n);
        if(colorIndicator != null && n < listaColores.Count)
            colorIndicator.color = listaColores[n];
    }
}
