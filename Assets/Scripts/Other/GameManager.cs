using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void Restart()
    {
        PathPlayerAnalyser.instance.StopRecording();
        PathPlayerAnalyser.instance.HidePath();
        string _currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(_currentSceneName);
    }

    private void Start()
    {
        PathPlayerAnalyser.instance.StartNewRecording();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            Restart();
        }
    }
}
