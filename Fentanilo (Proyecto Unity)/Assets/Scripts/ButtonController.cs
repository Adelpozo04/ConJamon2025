using UnityEngine;
using UnityEngine.Serialization;

public class ButtonController : Activador
{
    //Para probar la funcionalidad del botón sin que esté hecho. ESTO ES TEMPORAL
    [FormerlySerializedAs("clicktestPress")] [SerializeField] private bool clicktestPress;
    [FormerlySerializedAs("clicktestRelease")] [SerializeField] private bool clicktestRelease;

    private bool isPressed;
    // Update is called once per frame
    void Update()
    {
        //EL CLICKTEST ES TEMPORAL
        if (clicktestPress)
        {
            SendToActivables(true);
            clicktestPress = false;
        }
        if (clicktestRelease)
        {
            SendToActivables(false);
            clicktestRelease = false;
        }
    }
}
