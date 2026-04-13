using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [HideInInspector] public bool inGame = true;
    [SerializeField] private TimerManager timerManager;
    float timerInGameOver = 0f;

    private void Awake()
    {
        instance = this;
    }

    public void Restart()
    {
        timerInGameOver = 0f;
        PathPlayerAnalyser.instance.StopRecording();
        PathPlayerAnalyser.instance.HidePath();
        string _currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(_currentSceneName);
    }

    private void Start()
    {
        PathPlayerAnalyser.instance.StartNewRecording();
        inGame = true;
        if (timerManager)
            timerManager.StartTimer();
    }

    void Update()
    {
        if (!inGame)
        {
            timerInGameOver += Time.deltaTime;
        }

        if ((Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1)) && !inGame && timerInGameOver > 1f
            || Input.GetKeyUp(KeyCode.R)) 
        {
            Restart();
        }
    }

    public void GameEnd(bool _isWin)
    {
        inGame = false;
        if (timerManager)
            timerManager.StopTimer();
    }
}
