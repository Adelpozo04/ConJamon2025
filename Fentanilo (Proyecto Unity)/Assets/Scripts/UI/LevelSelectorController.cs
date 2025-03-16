using UnityEngine;

public class LevelSelectorController : MonoBehaviour
{
    public void LoadLevel(int i)
    {
        LevelManager.Instance.LoadFromMenu(i);
    }
}