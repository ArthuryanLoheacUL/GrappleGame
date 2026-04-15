using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonGoToMenu : MonoBehaviour
{
    public void GoToMenu(string _sceneName)
    {
        PathPlayerAnalyser.instance.HidePath();
        PathPlayerAnalyser.instance.ClearPaths();
        SceneManager.LoadScene(_sceneName);
    }
}
