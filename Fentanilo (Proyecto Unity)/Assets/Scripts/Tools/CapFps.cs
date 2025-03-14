using UnityEngine;

public class CapFps : MonoBehaviour
{
    public int fps = 60;
    void Start()
    {
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = fps;
    }
}