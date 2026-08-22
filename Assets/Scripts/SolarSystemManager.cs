using System;
using UnityEngine;

public class SolarSystemManager : MonoBehaviour
{
    [Range(0.1f, 1f)] public float SimulationTime = 0.25f;
    public event Action FixedFrameUpdated;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FixedUpdate()
    {
        FixedFrameUpdated?.Invoke();
    }
}
