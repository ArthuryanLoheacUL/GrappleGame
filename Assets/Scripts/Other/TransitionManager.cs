using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    enum StepAnimation
    {
        None,
        In,
        Out,
    };


    public static TransitionManager instance;

    [SerializeField] float animationTime = 1.0f;
    [SerializeField] Image img;
    float timer;
    string nextScene;

    int ignoredFrames = 0;

    StepAnimation stepAnimation;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    void SetAlpha(float _a)
    {
        Color _c = img.color;
        _c.a = _a;
        img.color = _c;
    }

    private void Start()
    {
        img.raycastTarget = false;
        SetAlpha(0f);
    }

    public void LoadTransition(string _scene)
    {
        nextScene = _scene;
        if (stepAnimation != StepAnimation.None)
            return;
        stepAnimation = StepAnimation.In;
        timer = 0;
        img.raycastTarget = true;
        Time.timeScale = 0f;
    }

    private void Update()
    {
        if (stepAnimation == StepAnimation.None) return;
        if (ignoredFrames > 0)
        {
            ignoredFrames--;
            timer = 0;
        }
        else
        {
            timer += Time.unscaledDeltaTime;
        }
        if (timer > animationTime / 2)
        {
            if (stepAnimation == StepAnimation.Out)
            {
                img.raycastTarget = false;
                stepAnimation = StepAnimation.None;
                timer = 0;
                Time.timeScale = 1f;
                SetAlpha(0);
            } else
            {
                stepAnimation = StepAnimation.Out;
                timer = 0;
                SetAlpha(1);
                ignoredFrames = 2;
                SceneManager.LoadScene(nextScene);
                nextScene = "";
            }
        } else
        {
            if (stepAnimation == StepAnimation.Out)
            {
                SetAlpha(1 - timer / (animationTime / 2));
            }
            else
            {
                SetAlpha(timer / (animationTime / 2));
            }
        }
    }

    public bool IsInTransition()
    {
        return stepAnimation != StepAnimation.None;
    }
}
