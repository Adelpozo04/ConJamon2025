using System.Collections;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class ImpulsoEffect : MonoBehaviour
{
    [Header("Ajustes de atracción inicial al interactuar")]
    [SerializeField] float attractTime;
    [Header("Ajustes de atracción al estar enganchado")]
    [SerializeField] float maxDistance;
    [SerializeField] float minDistance;
    [SerializeField] float moveForce;
    [SerializeField] float attractForce;
    [Header("Ajustes de vibración")]
    [SerializeField] private float vibrationIntensity = 0.1f;
    [SerializeField] private float vibrationSpeed = 50f;
    [Header("Ajustes de lanzamiento")]
    [SerializeField] private float shootingSpeed;

    private Rigidbody2D playerRB;
    private Transform playerTR;
    bool active = false;

    private Vector2 aimDirection;

    bool canShoot = false;
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
        playerTR.GetComponentInChildren<ImpulsoEffectPlayer>().ActivateAiming(this);
        playerRB.linearVelocity = new Vector2(0, 0);

        active = true;
        canShoot = true;
    }

    private void FixedUpdate()
    {
        if (active)
        {
            Vector3 direction = playerTR.position - transform.position;
            float distance = direction.magnitude;

            if (distance < maxDistance)
            {
                // Calcular el factor de reducción de velocidad según la distancia
                float speedFactor = Mathf.InverseLerp(maxDistance, 0.2f, distance); // 1 lejos, 0 cerca
                float currentMoveForce = moveForce * speedFactor;
                float currentAttractForce = attractForce * speedFactor;

                // Aplicar movimiento con la fuerza ajustada
                playerRB.AddForce(aimDirection.normalized * currentMoveForce, ForceMode2D.Force);

                // Aplicar fuerza de atracción si está fuera del rango mínimo
                if (distance > minDistance)
                    playerRB.AddForce(-direction.normalized * currentAttractForce, ForceMode2D.Force);

                // vibracion
                float offsetX = Mathf.Sin(Time.time * vibrationSpeed) * vibrationIntensity;
                float offsetY = Mathf.Cos(Time.time * vibrationSpeed) * vibrationIntensity;

                playerTR.position += new Vector3(offsetX, offsetY, 0);
            }
            else if (canShoot)
            {
                playerRB.linearVelocity = new Vector2(0,0);

                playerTR.GetComponentInChildren<ImpulsoEffectPlayer>().DeactivateAiming();
                playerRB.gravityScale = 1;

                playerRB.AddForce(aimDirection.normalized * shootingSpeed, ForceMode2D.Impulse);

                canShoot = false;
                active = false;
                //Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!active && LayerMask.LayerToName(collision.gameObject.layer) == "Player")
        {
            playerRB = collision.gameObject.GetComponentInParent<Rigidbody2D>();
            playerTR = playerRB.gameObject.transform;
            ActivateEffect();
            active = true;
        }
    }
}
