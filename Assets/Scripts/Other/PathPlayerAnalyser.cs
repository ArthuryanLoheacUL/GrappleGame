using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;
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
    private FinalCameraPos finalCameraPos;
    int idBest = -1;

    [Header("Ghost")]
    [SerializeField] private GameObject ghostPrefab;
    private GameObject playerGhost;
    private bool isActive = true;
    private Vector2 targetPos;
    private Vector2 prevPos;
    private float timerLerp = 0;


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
        finalCameraPos = GameObject.FindGameObjectWithTag("CineCam").GetComponent<FinalCameraPos>();
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
        if (idBest == -1)
            LoadBestSave();
        StartRecording();
        CreateGhost();
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
        idBest = -1;
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
        if (playerGhost)
        {
            UpdateGhost();
        }
    }

    void UpdateGhost()
    {
        playerGhost.SetActive(isActive);
        timerLerp += Time.deltaTime;
        if (positions[idBest] != null && current_positions.Count < positions[idBest].Count && current_positions.Count > 0)
        {
            if (targetPos != positions[idBest][current_positions.Count - 1])
            {
                timerLerp = 0;
                prevPos = playerGhost.transform.position;
            }
            targetPos = positions[idBest][current_positions.Count - 1];
        } else
        {
            playerGhost.SetActive(false);
        }
        playerGhost.transform.position = Vector2.Lerp(prevPos, targetPos, timerLerp / delayImages);
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
        finalCameraPos = GameObject.FindGameObjectWithTag("CineCam").GetComponent<FinalCameraPos>();

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

    void CreateGhost()
    {
        if (playerGhost == null)
            playerGhost = Instantiate(ghostPrefab);
        else
            playerGhost.SetActive(true);
    }

    void LoadBestSave()
    {
        List<Vector2> _pos = new List<Vector2>();
        int _c = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "_PB_Count", -1);
        CreateGhost();
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
