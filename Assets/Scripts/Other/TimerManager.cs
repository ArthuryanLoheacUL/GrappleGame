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

    [Header("Diff Time")]
    [SerializeField] TMP_Text diffTimeText;
    [SerializeField] TMP_Text diffTimeTextMilliseconds;
    [SerializeField] Color higherTimeColor;
    [SerializeField] Color lowerTimeColor;
    [SerializeField] Color tieTimeColor;

    float time;
    float bestTime;
    bool isRunning = false;

    private void Start()
    {
        diffTimeText.enabled = false;
        diffTimeTextMilliseconds.enabled = false;
    }

    void SetText(float _time, TMP_Text _text, TMP_Text _textMill, string _preText = "")
    {
        int _minutes = Mathf.FloorToInt(_time / 60);
        _time -= _minutes * 60;
        int _seconds = Mathf.FloorToInt(_time);
        _time -= _seconds;
        int _milliseconds = Mathf.FloorToInt(_time * 100);

        _text.text = _preText + _minutes.ToString().PadLeft(2, '0') + ":" + _seconds.ToString().PadLeft(2, '0') + ":";
        _textMill.text = _milliseconds.ToString().PadLeft(2, '0');
    }

    public void SetBestTimeText(float _time)
    {
        bestTime = _time;
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

    public void ShowDiff()
    {
        if (bestTime <= 0)
            return;
        float _diffTime = time - bestTime;
        string _strSign = "";
        diffTimeText.enabled = true;
        diffTimeTextMilliseconds.enabled = true;
        if (_diffTime > -0.01 && _diffTime < 0.01)
        {
            diffTimeText.color = tieTimeColor;
            diffTimeTextMilliseconds.color = tieTimeColor;
        }
        else if (_diffTime < 0)
        {
            diffTimeText.color = lowerTimeColor;
            diffTimeTextMilliseconds.color = lowerTimeColor;
            _strSign = "-";
        }
        else
        {
            diffTimeText.color = higherTimeColor;
            diffTimeTextMilliseconds.color = higherTimeColor;
            _strSign = "+";
        }
        SetText(Mathf.Abs(_diffTime), diffTimeText, diffTimeTextMilliseconds, _strSign);
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
