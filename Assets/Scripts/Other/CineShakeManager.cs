using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class CineShakeManager : MonoBehaviour
{
    public static CineShakeManager instance;
    [SerializeField] private CinemachineBasicMultiChannelPerlin cam;

    List<Tuple<float, float>> shakes = new List<Tuple<float, float>>();
    List<float> oneShakes = new List<float>();

    public void Shake(float _duration, float _intensity)
    {
        shakes.Add(new Tuple<float, float>(_duration, _intensity));
    }
    public void ShakeOneFrame(float _intensity)
    {
        oneShakes.Add(_intensity);
    }

    private void Start()
    {
        cam.AmplitudeGain = 0;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


    void Update()
    {
        float _intens = 0;
        for (int _i = 0; _i < shakes.Count; _i++)
        {
            shakes[_i] = new Tuple<float, float>(shakes[_i].Item1 - Time.deltaTime, shakes[_i].Item2);
            if (shakes[_i].Item1 > 0)
            {
                _intens += shakes[_i].Item2;
            } else {
                shakes.Remove(shakes[_i]);
                _i--;
            }
        }
        foreach (var _shake in oneShakes)
        {
            _intens += _shake;
        }
        oneShakes.Clear();
        cam.AmplitudeGain = _intens;
    }
}
