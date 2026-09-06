using System;
using UnityEngine;

public class SolarSystemManager : MonoBehaviour
{
    [Range(0.1f, 1f)] public float SimulationTime = 0.25f;
    public event Action FixedFrameUpdated;

    public float _skipTime = 1f;
    private float _timeElapsed;

    private bool _skipFrame;
    
    public void Update()
    {
        if (_timeElapsed < _skipTime)
        {
            _timeElapsed += Time.deltaTime;
            return;
        }

        if (_timeElapsed >= _skipTime)
        {
            _timeElapsed = 0f;
            
            FixedFrameUpdated?.Invoke();
        }
    }
}
