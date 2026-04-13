using TMPro;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    [Header("Time")]
    [SerializeField] TMP_Text timeText;
    [SerializeField] TMP_Text timeTextMilliseconds;
    [Header("Best Time")]
    [SerializeField] TMP_Text bestTimeText;
    [SerializeField] TMP_Text bestTimeTextMilliseconds;


    float time;
    bool isRunning = false;

    void SetText(float _time, TMP_Text _text, TMP_Text _textMill)
    {
        int _minutes = Mathf.FloorToInt(_time / 60);
        _time -= _minutes * 60;
        int _seconds = Mathf.FloorToInt(_time);
        _time -= _seconds;
        int _milliseconds = Mathf.FloorToInt(_time * 100);

        _text.text = _minutes.ToString().PadLeft(2, '0') + ":" + _seconds.ToString().PadLeft(2, '0') + ":";
        _textMill.text = _milliseconds.ToString().PadLeft(2, '0');
    }

    public void SetBestTimeText(float _time)
    {
        if (_time <= 0)
        {
            bestTimeText.enabled = false;
            bestTimeTextMilliseconds.enabled = false;
        } else
        {
            bestTimeText.enabled = true;
            bestTimeTextMilliseconds.enabled = true;
            SetText(_time, bestTimeText, bestTimeTextMilliseconds);
        }
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
            SetText(time, timeText, timeTextMilliseconds);
        }
    }

    public float GetTime()
    {
        return time;
    }
}
