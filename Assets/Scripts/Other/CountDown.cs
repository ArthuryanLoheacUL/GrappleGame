using System.Collections;
using TMPro;
using UnityEngine;

public class CountDown : MonoBehaviour
{
    private TMP_Text textCountDown;
    float timer = 0;
    [SerializeField] private float durationFreeze = 1f;
    [SerializeField] private AnimationCurve scaleCurve;
    float durationNextNumber = 0f;
    int number = 3;
    float fontSizeOriginal = 0;


    private void Awake()
    {
        textCountDown = GetComponent<TMP_Text>();
        durationNextNumber = durationFreeze / 3;
        fontSizeOriginal = textCountDown.fontSize;
        StartCoroutine(ActiveInX(durationNextNumber / 5, true));
    }

    private void Start()
    {
        Time.timeScale = 0;
        textCountDown.enabled = true;
    }

    private void Update()
    {
        if (GameManager.instance.inPause)
            return;
        timer += Time.unscaledDeltaTime;

        if (timer >= durationNextNumber)
        {
            timer = 0;
            number -= 1;
            if (number <= 0)
            {
                Time.timeScale = 1;
                textCountDown.enabled = false;
                enabled = false;
            }
            StartCoroutine(ActiveInX(durationNextNumber / 5, number > 0));
            textCountDown.text = number.ToString();
        }
        textCountDown.fontSize = fontSizeOriginal * (1 + scaleCurve.Evaluate(timer / durationNextNumber));
    }

    IEnumerator ActiveInX(float _duration, bool _reactive)
    {
        textCountDown.enabled = false;
        yield return new WaitForSecondsRealtime(_duration);
        textCountDown.enabled = _reactive;
        if (!_reactive)
        {
            SoundManager.instance.PlayOneShot("FinalCount", 6);
        } else
        {
            SoundManager.instance.PlayOneShot("Count", 6);
        }
    }
}
