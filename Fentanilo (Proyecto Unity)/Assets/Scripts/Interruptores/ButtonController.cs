using UnityEngine;
using UnityEngine.Serialization;

public class ButtonController : Activador
{
    [SerializeField] private Sprite unpressedSprite;
    [SerializeField] private Sprite pressedSprite;
    private SpriteRenderer renderer;


    //Para probar la funcionalidad del botón sin que esté hecho. ESTO ES TEMPORAL
    [FormerlySerializedAs("clicktestPress")][SerializeField] private bool clicktestPress;
    int numPisando = 0;

    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<PlayerMovement>() != null)
        {
            if(numPisando == 0)
            {
                SendToActivables(true);
                renderer.sprite = pressedSprite;

                PlayAudioSFX();
            }            
            numPisando++;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerMovement>() != null)
        {
            if (numPisando == 1)
            {
                SendToActivables(false);
                renderer.sprite = unpressedSprite;


                PlayAudioSFX();
            }
            numPisando--;           
        }
    }
}
