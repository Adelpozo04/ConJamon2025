using UnityEngine;

public class LeverHelper : MonoBehaviour
{
    [SerializeField] bool signal;
    [SerializeField] LeverController mLeverController;

    private void OnTriggerEnter2D(Collider2D other)
    {
        mLeverController.SetStateTo(signal);
    }
}
