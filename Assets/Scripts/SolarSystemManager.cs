using System;
using UnityEngine;

public class SolarSystemManager : MonoBehaviour
{

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
