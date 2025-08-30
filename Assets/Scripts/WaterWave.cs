using UnityEngine;

// This script makes a wavy water mesh at runtime.
// Needs MeshFilter and MeshRenderer on the same GameObject.
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class WaterWave : MonoBehaviour
{
    [Header("Mesh Settings")]
    public int width = 20;    // how many quads wide
    public int length = 20;   // how many quads long
    public float scale = 1f;  // distance between verts

    [Header("Wave Settings")]
    public float amplitude = 1f;   // how tall the waves get
    public float frequency = 1f;   // how many waves fit in the mesh
    public float speed = 1f;       // how fast the waves move

    private Mesh mesh;
    private Vector3[] baseVertices; // original flat mesh, so we can reset each frame

    void Start()
    {
        // Make a new mesh and assign it to the MeshFilter
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        CreateMesh();
    }

    void Update()
    {
        AnimateWaves();
    }

    // Makes a flat grid mesh using plane mesh Note: Only works for planes aligned with XZ plane
    void CreateMesh()
    {
        Vector3[] vertices = new Vector3[(width + 1) * (length + 1)];
        int[] triangles = new int[width * length * 6];
        Vector2[] uv = new Vector2[vertices.Length];

        // These offsets make the mesh centered instead of starting at (0,0,0)
        float xOffset = (width * scale) * 0.5f;
        float zOffset = (length * scale) * 0.5f;

        // Make all the vertices
        for (int z = 0, i = 0; z <= length; z++)
        {
            for (int x = 0; x <= width; x++, i++)
            {
                float vx = x * scale - xOffset;
                float vz = z * scale - zOffset;
                vertices[i] = new Vector3(vx, 0, vz);
                uv[i] = new Vector2((float)x / width, (float)z / length);
            }
        }

        // Make all the triangles (two per quad)
        int vert = 0;
        int tris = 0;
        for (int z = 0; z < length; z++)
        {
            for (int x = 0; x < width; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + width + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + width + 1;
                triangles[tris + 5] = vert + width + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.RecalculateNormals();

        // Save the flat mesh for reference
        baseVertices = mesh.vertices;
    }

    // method for mesh wavy
    void AnimateWaves()
    {
        Vector3[] vertices = new Vector3[baseVertices.Length];
        float time = Time.time * speed;

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 v = baseVertices[i];
            // Main wave formula for creating different wave patterns.
            v.y = amplitude * Mathf.Sin(frequency * v.x + time) * Mathf.Cos(frequency * v.z + time);
            vertices[i] = v;
        }

        mesh.vertices = vertices;
        mesh.RecalculateNormals();
    }
}