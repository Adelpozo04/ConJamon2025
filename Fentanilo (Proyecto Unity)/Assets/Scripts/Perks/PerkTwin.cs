using UnityEngine;

public class PerkTwin : MonoBehaviour
{

    [SerializeField]
    private float upOffset = 0;

    [SerializeField]
    private float downOffset = 0;

    [SerializeField]
    private float speed = 30;

    [SerializeField]
    private Vector3 direction = Vector2.up;

    private Vector3 originalPos;

    [SerializeField]
    private float cooldowm = 0.2f;

    private float elapsedTime = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalPos = transform.position;

        elapsedTime = cooldowm;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(elapsedTime >= cooldowm)
        {
            if (transform.position.y >= originalPos.y + upOffset && direction == Vector3.up)
            {
                direction = Vector3.down;
            }
            else if (transform.position.y <= originalPos.y - upOffset && direction == Vector3.down)
            {
                direction = Vector3.up;
            }

            elapsedTime = 0;
        }
        else
        {
            elapsedTime += Time.deltaTime;
        }
        

        transform.position += direction * speed * Time.deltaTime;

    }
}
