using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [HideInInspector] public bool inGame = true;
    [SerializeField] private TimerManager timerManager;
    [SerializeField] private GameObject pauseWindow;
    float timerInGameOver = 0f;
    float bestTime = 0f;
    [HideInInspector] public bool inPause = false;
    float prevScale = 1f;

    bool isWin = false;
    [SerializeField] GameObject gameOverWindow;

    private void Awake()
    {
        instance = this;
    }

    public void Restart()
    {
        inPause = false;
        pauseWindow.SetActive(false);
        timerInGameOver = 0f;
        PathPlayerAnalyser.instance.StopRecording();
        PathPlayerAnalyser.instance.HidePath();
        string _currentSceneName = SceneManager.GetActiveScene().name;
        Time.timeScale = 1f;
        SceneManager.LoadScene(_currentSceneName);
    }

    private void Start()
    {
        bestTime = PlayerPrefs.GetFloat("PB_" + SceneManager.GetActiveScene().name, 0);
        PathPlayerAnalyser.instance.StartNewRecording();
        inGame = true;
        if (timerManager)
        {
            timerManager.SetBestTimeText(bestTime);
            timerManager.StartTimer();
        }
        gameOverWindow.SetActive(false);
        pauseWindow.SetActive(false);
        inPause = false;
    }

    void Update()
    {
        if (!inGame && timerInGameOver < 1f)
        {
            timerInGameOver += Time.deltaTime;
            if (timerInGameOver >= 1f)
            {
                gameOverWindow.SetActive(true);
            }
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            Restart();
        }

        if (inGame && Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    public void GameEnd(bool _isWin)
    {
        bool _isPB = false;

        inGame = false;
        isWin = _isWin;
        if (timerManager)
        {
            timerManager.StopTimer();
            if (_isWin)
                timerManager.ShowDiff();
        }
        if (_isWin)
        {
            if ((timerManager.GetTime() < bestTime && bestTime - timerManager.GetTime() > 0.01) || bestTime == 0f)
            {
                PlayerPrefs.SetFloat("PB_" + SceneManager.GetActiveScene().name, timerManager.GetTime());
                _isPB = true;
            }
        }
        PathPlayerAnalyser.instance.StopRecording(_isPB, !_isWin);
    }

    public void Pause()
    {
        if (inGame)
        {
            inPause = !inPause;
            pauseWindow.SetActive(inPause);
            if (inPause)
                prevScale = Time.timeScale;
            Time.timeScale = inPause ? 0f : prevScale;
        }
    }
}
