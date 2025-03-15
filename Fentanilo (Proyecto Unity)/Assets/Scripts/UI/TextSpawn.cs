using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextSpawn : MonoBehaviour
{
    public string text;
    public TMP_Text textTMPRO;

    IEnumerator appearText(float totalTime)
    {
        int index = 1;
        float time = 0;
        float timeToType = 0;
        while (time < totalTime)
        {
            if (time >= timeToType)
            {
                textTMPRO.text = text.Substring(0, index++);

                timeToType = time + (totalTime / text.Length); 
            }
            
            time += Time.deltaTime;

            yield return null;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(appearText(5));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
