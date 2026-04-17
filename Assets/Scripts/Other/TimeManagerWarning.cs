using UnityEngine;

public class TimeManagerWarning : MonoBehaviour
{
    float timeScaleOrigin = 1f;

    void Start()
    {
        timeScaleOrigin = Time.timeScale;
        Time.timeScale = 0f;
    }

    public void Close()
    {
        Time.timeScale = timeScaleOrigin;
        Destroy(gameObject);
    }
}
