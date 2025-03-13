using System.Collections;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class ImpulsoEffect : MonoBehaviour
{
    [SerializeField] float attractTime;
    [SerializeField] float maxDistance;
    [SerializeField] float minDistance;

    private Rigidbody2D playerRB;
    private Transform playerTR;
    bool active = false;

    private Vector2 aimDirection;
    public void ActivateEffect()
    {
        // desactivar gravedad y anular linearVelocity
        playerRB.linearVelocity = new Vector2(0, 0);
        playerRB.gravityScale = 0;

        StartCoroutine(AttractPlayer(playerTR, transform.position, attractTime));
    }

    IEnumerator AttractPlayer(Transform playerTr, Vector3 endPos, float totalTime)
    {
        float time = 0;

        Vector3 startPos = playerTr.position;

        while (time < totalTime)
        {
            time += Time.deltaTime;

            if ((time / totalTime) < 1)
            {
                Vector3 pos = Vector3.Lerp(startPos, endPos, Easing.InCirc(time / totalTime));

                playerTr.position = pos;
            }

            yield return null;
        }

        playerTr.position = endPos;
        StartAim();
    }

    public void Aim(Vector2 directionInput)
    {
        aimDirection = directionInput;
    }

    private void StartAim()
    {
        playerRB.linearVelocity = new Vector2(0, 0);

        playerTR.GetComponentInChildren<ImpulsoEffectPlayer>().ActivateAiming(this);

        active = true;
    }

    private void FixedUpdate()
    {
        if (active && Vector3.Distance(transform.position, playerTR.position) > minDistance)
        {
            Debug.Log(Vector2.Distance(transform.position, playerTR.position));
            Vector3 direccion = transform.position - playerTR.position;

            //playerRB.AddForce(direccion.normalized, ForceMode2D.Impulse);
            playerRB.AddForce(aimDirection.normalized, ForceMode2D.Force);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!active && LayerMask.LayerToName(collision.gameObject.layer) == "Player")
        {
            playerRB = collision.gameObject.GetComponent<Rigidbody2D>();
            playerTR = collision.gameObject.transform;
            ActivateEffect();
            active = true;
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
