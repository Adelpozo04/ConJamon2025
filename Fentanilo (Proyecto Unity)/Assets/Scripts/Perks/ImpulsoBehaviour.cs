using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements.Experimental;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

[RequireComponent(typeof(PerkGrabable))]
public class ImpulsoBehaviour : PerkBehaviour
{
    [SerializeField] float attractTime;


    private Rigidbody2D playerRB;

    bool active = false;

    public override void ActivateEffect(GameObject player)
    {
        //player.GetComponent<PlayerMovement>().enabled = false;

        // desactivar gravedad y anular linearVelocity
        playerRB = player.GetComponent<Rigidbody2D>();
        //playerRB.linearVelocity = new Vector2(0, 0);
        playerRB.gravityScale = 0;

        StartCoroutine(AttractPlayer(playerRB.transform, transform.position, attractTime));
    }

    IEnumerator AttractPlayer(Transform playerTr, Vector3 endPos, float totalTime)
    {
        float time = 0;

        Vector3 startPos = playerTr.position;

        while (time < totalTime)
        {
            time += Time.deltaTime;

            Vector3 pos = Vector3.Lerp(startPos, endPos, Easing.InCirc(time / totalTime));
            
            playerTr.position = pos;

            yield return null;
        }

        active = true;
    }

    private void FixedUpdate()
    {
        if (active)
        {
            Vector3 direccion = transform.position - playerRB.transform.position;

            playerRB.AddForce(direccion.normalized, ForceMode2D.Impulse);
        }

    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    
    }
}
