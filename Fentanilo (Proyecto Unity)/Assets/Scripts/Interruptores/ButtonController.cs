using UnityEngine;
using UnityEngine.Serialization;

public class ButtonController : Activador
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

    private void OnTriggerEnter2D(Collider2D other)
    {

        Debug.Log("BotonTocado");

        if(other.gameObject.GetComponent<PlayerMovement>() != null)
        {
            SendToActivables(true);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerMovement>() != null)
        {
            SendToActivables(false);
        }
    }
}
