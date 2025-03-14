using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;

public class LeverController : Activador
{
    //Para probar la funcionalidad del botón sin que esté hecho. ESTO ES TEMPORAL
    [FormerlySerializedAs("clicktestPress")][SerializeField] private bool clicktestPress;

    [SerializeField] SpriteRenderer spriteOn;


    void Start() 
    {
        ChangeAspect();
    }
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
    private void ChangeAspect()
    {
        if (isPressed)
        {
            spriteOn.enabled = true;
            GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            spriteOn.enabled = false;
            GetComponent<SpriteRenderer>().enabled = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerMovement>() != null)
        {            
            Switch();
            ChangeAspect();
            PlayAudioSFX();
        }
    }
}
