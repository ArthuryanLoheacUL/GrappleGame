using System.Collections.Generic;
using UnityEngine;
using System.Collections;

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
            idBest = positions.Count - 1;
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
}
