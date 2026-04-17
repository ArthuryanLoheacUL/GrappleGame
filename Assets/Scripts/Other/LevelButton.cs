using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    private string title;
    private string nameScene;
    private float time;

    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text timeText;

    public void Setup(string _tilte, string _nameScene, float _time)
    {
        title = _tilte;
        nameScene = _nameScene;
        time = _time;
        SetupTexts();
    }

    void SetupTexts()
    {
        titleText.text = title;
        float _time = time;
        if (_time == 0)
        {
            timeText.gameObject.SetActive(false);
            return;
        }

        int _minutes = Mathf.FloorToInt(_time / 60);
        _time -= _minutes * 60;
        int _seconds = Mathf.FloorToInt(_time);
        _time -= _seconds;
        int _milliseconds = Mathf.FloorToInt(_time * 100);

        timeText.text = _minutes.ToString().PadLeft(2, '0') + ":" + _seconds.ToString().PadLeft(2, '0') + ":" + _milliseconds.ToString().PadLeft(2, '0');
    }

    public void GoToScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nameScene);
    }
}
