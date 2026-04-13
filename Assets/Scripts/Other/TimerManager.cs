using TMPro;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    [SerializeField] TMP_Text timeText;
    [SerializeField] TMP_Text timeTextMilliseconds;

    float time;
    bool isRunning = false;

    void SetText(float _time)
    {
        int _minutes = Mathf.FloorToInt(_time / 60);
        _time -= _minutes * 60;
        int _seconds = Mathf.FloorToInt(_time);
        _time -= _seconds;
        int _milliseconds = Mathf.FloorToInt(_time * 100);

        timeText.text = _minutes.ToString().PadLeft(2, '0') + ":" + _seconds.ToString().PadLeft(2, '0') + ":";
        timeTextMilliseconds.text = _milliseconds.ToString().PadLeft(2, '0');
    }

    public void StartTimer()
    {
        isRunning = true;
        time = 0f;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    private void Update()
    {
        if (isRunning)
        {
            time += Time.deltaTime;
            SetText(time);
        }
    }
}
