using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ProceduralQuad : MonoBehaviour
{
    public float upMultiplier = 1f;
    public float rightMultiplier = 1f;

    public Material mat;

    private void OnEnable()
    {
        var mesh = new Mesh
        {
            name = "ProMesh"
        };

        mesh.vertices = new[]
        {
            Vector3.zero,
            Vector3.up * upMultiplier,
            Vector3.right * rightMultiplier,
            new Vector3(1f, 1f),
            // new Vector3(0f, 1.2f),
            // new Vector3(1.2f, 1.2f)
        };

        mesh.triangles = new[] { 0, 1, 2, 1, 3, 2 };
        mesh.uv = new[]
        {
            Vector2.zero,
            Vector2.up * upMultiplier,
            Vector2.right * rightMultiplier,
            // Vector2.right,
            // Vector2.up,
            Vector2.one
        };

        mesh.normals = new[]
        {
            Vector3.back, Vector3.back, Vector3.back,
            Vector3.back
        };

        mesh.tangents = new[]
        {
            new Vector4(1f, 0f, 0f, -1f),
            new Vector4(1f, 0f, 0f, -1f),
            new Vector4(1f, 0f, 0f, -1f),
            new Vector4(1f, 0f, 0f, -1f),
            // new Vector4(1f, 0f, 0f, -1f),
            // new Vector4(1f, 0f, 0f, -1f)
        };

        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshRenderer>().material = mat;
    }
}