using UnityEngine;

public class CapFps : MonoBehaviour
{

    public int fps = 60;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = fps;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
