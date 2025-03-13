using UnityEngine;
using UnityEngine.Serialization;

public class LeverController : Activador
{
    //Para probar la funcionalidad del botón sin que esté hecho. ESTO ES TEMPORAL
    [FormerlySerializedAs("clicktestPress")][SerializeField] private bool clicktestPress;
    
    // Update is called once per frame
    void Update()
    {
        //EL CLICKTEST ES TEMPORAL
        if (clicktestPress)
        {
            Switch();
            clicktestPress = false;
        }
    }
}
