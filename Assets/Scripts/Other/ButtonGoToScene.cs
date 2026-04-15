using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonGoToScene : MonoBehaviour
{
    public void GoToScene(string _sceneName)
    {
        SceneManager.LoadScene(_sceneName);
    }
}
