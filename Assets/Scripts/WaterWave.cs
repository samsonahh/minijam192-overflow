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

    [Header("Epicenter Settings")]
    public Vector3 epicenter = Vector3.zero; // Set this in Inspector or code
    public float pushRadius = 5f;
    public float pushForce = 10f;

    [Header("Gizmo Settings")]
    public bool showGizmos = true; // Toggle for gizmo drawing

    private Mesh mesh;
    private Vector3[] baseVertices; // original flat mesh, so we can reset each frame

    // Store previous values to detect changes
    private int prevWidth;
    private int prevLength;
    private float prevScale;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        CreateMesh();

        prevWidth = width;
        prevLength = length;
        prevScale = scale;
    }

    void Update()
    {
        // If any mesh setting changed, rebuild the mesh
        if (width != prevWidth || length != prevLength || !Mathf.Approximately(scale, prevScale))
        {
            CreateMesh();
            prevWidth = width;
            prevLength = length;
            prevScale = scale;
        }

        AnimateWaves();
    }

    void FixedUpdate()
    {
        PushObjectsFromEpicenter();
    }

    // Makes a flat grid mesh using plane mesh Note: Only works for planes aligned with XZ plane
    void CreateMesh()
    {
        Vector3[] vertices = new Vector3[(width + 1) * (length + 1)];
        int[] triangles = new int[width * length * 6];
        Vector2[] uv = new Vector2[vertices.Length];

        // Center the mesh
        float xOffset = (width * scale) * 0.5f;
        float zOffset = (length * scale) * 0.5f;

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

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.RecalculateNormals();

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

    // Constantly sets rigidbodies' outward velocity from the epicenter while they're in the radius
    void PushObjectsFromEpicenter()
    {
        Collider[] colliders = Physics.OverlapSphere(epicenter, pushRadius);
        foreach (var col in colliders)
        {
            Rigidbody rb = col.attachedRigidbody;
            if (rb != null)
            {
                Vector3 dir = (col.transform.position - epicenter);
                dir.y = 0f; // Only push in XZ plane
                if (dir.sqrMagnitude > 0.0001f)
                {
                    dir.Normalize();
                    Vector3 velocity = rb.linearVelocity;
                    // Set XZ velocity to a flat value outward, keep Y velocity for floating
                    Vector3 outwardVelocity = dir * pushForce;
                    rb.linearVelocity = new Vector3(outwardVelocity.x, velocity.y, outwardVelocity.z);
                }
            }
        }
    }

    // Returns the water height at a given (x, z) world position
    public float GetWaveHeightAtPosition(float x, float z)
    {
        // Convert world position to local position relative to the water mesh
        Vector3 local = transform.InverseTransformPoint(new Vector3(x, 0, z));
        // Use the same wave formula as AnimateWaves
        return amplitude * Mathf.Sin(frequency * local.x + Time.time * speed) * Mathf.Cos(frequency * local.z + Time.time * speed) + transform.position.y;
    }

    void OnDrawGizmos()
    {
        if (!showGizmos) return;

        // Draw the epicenter as a small red sphere (always visible)
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(epicenter, 0.15f);

        // Draw the push radius as a wire sphere (cyan)
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(epicenter, pushRadius);

        // Draw arrows (lines) radiating outward from the epicenter
        Gizmos.color = Color.magenta;
        int arrowCount = 32; // Number of arrows/lines to draw
        float arrowLength = pushRadius;
        float arrowHeadLength = 0.3f;
        float arrowHeadAngle = 20f;

        for (int i = 0; i < arrowCount; i++)
        {
            float angle = (360f / arrowCount) * i;
            float rad = angle * Mathf.Deg2Rad;
            Vector3 dir = new Vector3(Mathf.Cos(rad), 0, Mathf.Sin(rad));
            Vector3 end = epicenter + dir * arrowLength;

            // Draw main line
            Gizmos.DrawLine(epicenter, end);

            // Draw arrowhead
            Vector3 right = Quaternion.Euler(0, arrowHeadAngle, 0) * -dir;
            Vector3 left = Quaternion.Euler(0, -arrowHeadAngle, 0) * -dir;
            Gizmos.DrawLine(end, end + right * arrowHeadLength);
            Gizmos.DrawLine(end, end + left * arrowHeadLength);
        }
    }
}