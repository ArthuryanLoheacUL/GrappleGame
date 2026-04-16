using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

[System.Serializable]
public struct Level
{
    public string name;
    public string scene;
    public Sprite sprite;
}

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Level[] levels;
    [SerializeField] private SetupLeftPage leftPage;
    [SerializeField] private GameObject prefabLevel;
    [SerializeField] private List<GameObject> levelPage = new List<GameObject>();
    Vector2 targetPos = Vector2.zero;
    Vector2 originalPos = Vector2.zero;

    Vector2 originalPosNext = Vector2.zero;
    [SerializeField] private float deltaMove;
    [SerializeField] private float animationTime;
    [SerializeField] private AnimationCurve animationCurve;
    int id = 0;

    bool inTransition = false;
    float timer = 0;

    void Start()
    {
        id = 0;
        SetupLevel(id);
    }

    void Update()
    {
        if (inTransition)
        {
            timer += Time.deltaTime;
            if (timer >= animationTime)
            {
                timer = 0;
                inTransition = false;
                GameObject _ref = levelPage[0];
                levelPage.RemoveAt(0);
                Destroy( _ref );
            }
            else
            {
                levelPage[0].GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(originalPos, targetPos, animationCurve.Evaluate(timer / animationTime));
                levelPage[1].GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(originalPosNext, originalPos, animationCurve.Evaluate(timer / animationTime));
                levelPage[1].GetComponent<SetupLevelPage>().Load(animationCurve.Evaluate(timer / animationTime));
            }

        }
    }

    void SetupLevel(int _id)
    {
        if (id < 0 || id >= levels.Length)
            return;

        Level _level = levels[id];
        leftPage.Setup(_level);
    }

    void Reload()
    {
        SetupLevel(id);
    }

    public void NextPage()
    {
        if (inTransition)
            return;
        id++;
        if (id >= levels.Length)
        {
            id = 0;
        }
        inTransition = true;
        timer = 0;
        SoundManager.instance.PlayOneShot("Swoosh", 10);
        CreateNext(deltaMove);
        Reload();
    }

    void CreateNext(float _delta)
    {
        targetPos = levelPage[0].GetComponent<RectTransform>().anchoredPosition + new Vector2(_delta, 0);
        originalPos = levelPage[0].GetComponent<RectTransform>().anchoredPosition;

        levelPage.Add(Instantiate(prefabLevel, levelPage[0].GetComponent<RectTransform>().parent));
        levelPage[1].transform.SetSiblingIndex(0);
        levelPage[1].GetComponent<RectTransform>().anchoredPosition = levelPage[0].GetComponent<RectTransform>().anchoredPosition + new Vector2(-_delta, 0);
        originalPosNext = levelPage[1].GetComponent<RectTransform>().anchoredPosition;
        levelPage[1].GetComponent<SetupLevelPage>().SetupPage(levels[id]);
    }

    public void PrevPage()
    {
        if (inTransition)
            return;
        id--;
        if (id < 0)
        {
            id = levels.Length - 1;
        }
        inTransition = true;
        timer = 0;
        SoundManager.instance.PlayOneShot("Swoosh", 10);
        CreateNext(-deltaMove);
        Reload();
    }

    public void StartCurrentLevel()
    {
        SceneManager.LoadScene(levels[id].scene);
    }
}
