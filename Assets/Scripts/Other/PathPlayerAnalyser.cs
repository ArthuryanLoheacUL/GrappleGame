using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PathPlayerAnalyser : MonoBehaviour
{
    [SerializeField] private Transform player;
    private bool recording = false;
    [SerializeField] private float delayImages = 0.5f;
    float timerImages = 0;
    [SerializeField] private LineRenderer lineRenderer;

    List<Vector2> positions = new List<Vector2>();

    void Start()
    {
        ClearImages();
        StartRecording();
        lineRenderer.enabled = false;
    }

    public void StartRecording()
    {
        recording = true;
    }

    public void StopRecording()
    {
        recording = false;
    }

    public void ClearImages()
    {
        positions.Clear();
    }

    private void Update()
    {
        if (recording && player)
        {
            timerImages += Time.deltaTime;
            if (timerImages > delayImages)
            {
                timerImages = 0;
                positions.Add(player.position);
            }
        }
    }

    public void ShowPath()
    {
        StopRecording();
        lineRenderer.enabled = true;
        int _nbSegments = positions.Count;
        lineRenderer.positionCount = _nbSegments;
        int _i = 0;
        foreach (Vector2 _segment in positions)
        {
            lineRenderer.SetPosition(_i, _segment);
            _i++;
        }
    }
}
