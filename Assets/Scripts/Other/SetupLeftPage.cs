using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetupLeftPage : MonoBehaviour
{
    [SerializeField] private TMP_Text bestText;
    [SerializeField] private TMP_Text triesText;
    [SerializeField] private TMP_Text nameText;

    void SetupTime(float _time)
    {
        if (_time == 0)
        {
            bestText.gameObject.SetActive(false);
            return;
        }

        int _minutes = Mathf.FloorToInt(_time / 60);
        _time -= _minutes * 60;
        int _seconds = Mathf.FloorToInt(_time);
        _time -= _seconds;
        int _milliseconds = Mathf.FloorToInt(_time * 100);

        bestText.text = _minutes.ToString().PadLeft(2, '0') + ":" + _seconds.ToString().PadLeft(2, '0') + ":" + _milliseconds.ToString().PadLeft(2, '0');
    }

    public void Setup(Level _level)
    {
        nameText.text = _level.name;
        float _time = PlayerPrefs.GetFloat("PB_" + _level.scene, 0);
        if (_time > 0)
        {
            SetupTime(_time);
        } else
        {
            bestText.text = "No Time";
        }
        triesText.text = PlayerPrefs.GetFloat("Tries_" + _level.scene, 0).ToString();
    }
}
