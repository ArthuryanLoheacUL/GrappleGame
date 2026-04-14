using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct Level
{
    public string name;
    public string scene;
}

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Level[] levels;
    [SerializeField] private GameObject prefabLevel;

    void Start()
    {
        foreach (var _level in levels)
        {
            GameObject _levelObj = Instantiate(prefabLevel, transform);
            _levelObj.GetComponent<LevelButton>().Setup(_level.name, _level.scene, PlayerPrefs.GetFloat("PB_" + _level.scene, 0));
        }
    }
}
