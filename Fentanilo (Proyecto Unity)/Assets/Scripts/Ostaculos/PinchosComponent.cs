using UnityEngine;

public class PinchosComponent : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var playerMovement = collision.collider.gameObject.GetComponent<PlayerMovement>();

        if (playerMovement != null)
        {
            if (playerMovement._recording)
            {
                playerMovement._controller.stopRecording();
            }


            Destroy(playerMovement.gameObject); 

        }
    }
}
