using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CelestialBodyVisuals : MonoBehaviour
{
    //Debug

    [Header("Live Trail Settings")] public bool showLiveTrail = true;
    public int maxHistoryPoints = 200; // How many past positions to remember

    private readonly Queue<Vector3> _positionHistory = new();

    private void Update()
    {
        _positionHistory.Enqueue(transform.position);

        // Remove older points so the history doesn't grow forever
        if (_positionHistory.Count > maxHistoryPoints)
        {
            _positionHistory.Dequeue();
        }
    }

    private void OnDrawGizmos()
    {
        Handles.Label(transform.position + Vector3.up * 2f, gameObject.name);
        if (showLiveTrail && _positionHistory.Count > 1)
        {
            Gizmos.color = Color.yellow;

            Vector3[] pointsArray = _positionHistory.ToArray();
            for (int i = 0; i < pointsArray.Length - 1; i++)
            {
                // Draw a continuous line connecting all past physical locations
                Gizmos.DrawLine(pointsArray[i], pointsArray[i + 1]);
            }
        }
    }
}