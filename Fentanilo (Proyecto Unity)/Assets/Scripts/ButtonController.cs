using UnityEngine;

public class ButtonController : Activador
{
    //Para probar la funcionalidad del botón sin que esté hecho. ESTO ES TEMPORAL
    [SerializeField] private bool clicktest;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //EL CLICKTEST ES TEMPORAL
        if (clicktest)
        {
            SendToActivables(true);
        }
    }
}
