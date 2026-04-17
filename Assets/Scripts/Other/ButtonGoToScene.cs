using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonGoToScene : MonoBehaviour
{
    public void GoToScene(string _sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(_sceneName);
    }
}
