using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class PathPlayerAnalyser : MonoBehaviour
{
    public static PathPlayerAnalyser instance;

    private Transform player;
    [SerializeField] private float delayShowPath = 0.05f;
    [SerializeField] private int showedImages = 5;
    private bool recording = false;
    [SerializeField] private float delayImages = 0.5f;
    float timerImages = 0;

    List<Vector2> current_positions = new List<Vector2>();
    List<List<Vector2>> positions = new List<List<Vector2>>();

    [SerializeField] private GameObject prefabLinePath;

    [SerializeField] private Gradient gradientCurrent;
    [SerializeField] private Gradient gradientPreviousRun;


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

    public void StopRecording()
    {
        recording = false;
        positions.Add(new List<Vector2>(current_positions));
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
                timerImages = 0;
                current_positions.Add(player.position);
            }
        } else
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        }
    }

    public void ShowPath()
    {
        StopRecording();

        for (int _i = 0; _i < positions.Count; _i++)
        {
            Gradient _gradient = gradientPreviousRun;
            if (_i == positions.Count - 1)
                _gradient = gradientCurrent; 

            StartCoroutine(ProgressiveShow(positions[_i], _gradient));
        }
    }

    IEnumerator ProgressiveShow(List<Vector2> _positions, Gradient _gradient)
    {
        GameObject _linePath = Instantiate(prefabLinePath, transform);
        LineRenderer _lineRenderer = _linePath.GetComponent<LineRenderer>();
        _lineRenderer.colorGradient = _gradient;

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
