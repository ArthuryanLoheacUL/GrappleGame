using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PathPlayerAnalyser : MonoBehaviour
{
    public static PathPlayerAnalyser instance;

    [Header("Captures Params")]
    [SerializeField] private float delayImages = 0.5f;
    private Transform player;
    private bool recording = false;
    private float timerImages = 0;
    private List<Vector2> current_positions = new List<Vector2>();
    private List<List<Vector2>> positions = new List<List<Vector2>>();

    [Header("Show Params")]
    [SerializeField] private GameObject prefabLinePath;
    [SerializeField] private GameObject prefabLinePathCurrent;
    [SerializeField] private GameObject prefabLinePathBest;
    [SerializeField] private float delayShowPath = 0.05f;
    [SerializeField] private int showedImages = 5;
    [SerializeField] private FinalCameraPos finalCameraPos;
    int idBest = -1;

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

    private void Start()
    {
        LoadBestSave();
    }

    public void StartRecording()
    {
        recording = true;
        timerImages = 0;
    }
    public void StartNewRecording()
    {
        ClearImages();
        StartRecording();
    }

    public void StopRecording(bool _isBest = false)
    {
        if (!recording)
            return;
        TakeImage();
        recording = false;
        positions.Add(new List<Vector2>(current_positions));
        if (_isBest)
        {
            idBest = positions.Count - 1;
            PlayerSaveBest();
        }
    }

    public void ClearImages()
    {
        current_positions = new List<Vector2>();
    }

    public void ClearPaths()
    {
        positions.Clear();
        ClearImages();
    }

    private void Update()
    {
        if (recording && player)
        {
            timerImages += Time.deltaTime;
            if (timerImages > delayImages)
            {
                TakeImage();
            }
        } else
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        }
    }

    void TakeImage()
    {
        if (!player)
            return;

        timerImages = 0;
        current_positions.Add(player.position);
    }

    public void ShowPath()
    {
        if (recording)
            StopRecording();

        for (int _i = 0; _i < positions.Count; _i++)
        {
            GameObject _prefab = prefabLinePath;
            if (_i == positions.Count - 1)
                _prefab = prefabLinePathCurrent;
            else if (_i == idBest)
                _prefab = prefabLinePathBest;

            StartCoroutine(ProgressiveShow(positions[_i], _prefab));
        }
    }

    IEnumerator ProgressiveShow(List<Vector2> _positions, GameObject _prefab)
    {
        GameObject _linePath = Instantiate(_prefab, transform);
        LineRenderer _lineRenderer = _linePath.GetComponent<LineRenderer>();

        int _i = 0;
        foreach (Vector2 _segment in _positions)
        {
            _lineRenderer.positionCount = _i + 1;
            _lineRenderer.SetPosition(_i, _segment);
            _lineRenderer.widthMultiplier = finalCameraPos.finalOrthographicSize / 40;
            if (_i % showedImages == 0)
                yield return new WaitForSeconds(delayShowPath);
            _i++;
        }
    }

    public void HidePath()
    {
        StopAllCoroutines();
        int _childs = transform.childCount;
        for (int _i = 0; _i < _childs; _i++)
            Destroy(transform.GetChild(_i).gameObject);
    }

    void PlayerSaveBest()
    {
        List<Vector2> _pos = positions[idBest];
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_PB_Count", _pos.Count);
        for (int _i = 0; _i < _pos.Count; _i++)
        {
            PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + "_PB_" + _i.ToString() + "_x", _pos[_i].x);
            PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + "_PB_" + _i.ToString() + "_y", _pos[_i].y);
        }
    }

    void LoadBestSave()
    {
        List<Vector2> _pos = new List<Vector2>();
        int _c = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "_PB_Count", -1);
        if (_c == -1)
            return;
        for (int  _i = 0; _i < _c; _i++)
        {
            Vector2 _point;
            _point.x = PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name + "_PB_" + _i.ToString() + "_x", 0);
            _point.y = PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name + "_PB_" + _i.ToString() + "_y", 0);
            _pos.Add( _point );
        }
        positions.Add(_pos);
        idBest = positions.Count - 1;
    }
}
