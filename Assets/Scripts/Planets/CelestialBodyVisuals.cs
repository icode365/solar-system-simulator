using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CelestialBodyVisuals : MonoBehaviour
{
    private readonly Queue<Vector3> _positionHistory = new();

    public void AssignMaterial(Material mat)
    {
        // Get 
        var _renderer = GetComponent<MeshRenderer>();
        if (_renderer == null)
        {
            Debug.LogError("No Renderer on " + gameObject.name);
            return;
        }

        _renderer.material = mat;
    }

    private void OnDrawGizmos()
    {
        Handles.Label(transform.position + Vector3.up * 2f, gameObject.name);
    }
}