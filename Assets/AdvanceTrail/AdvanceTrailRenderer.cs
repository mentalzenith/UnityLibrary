using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AdvanceTrailRenderer : MonoBehaviour
{
    MeshRenderer meshRenderer;
    MeshFilter meshFilter;

    Mesh mesh;
    Vector3[] vertices;
    Vector3[] tangent;
    int[] triangles;
    Vector2[] uv;
    int verticesIndex;

    int maxPointPerBatch;
    float life;

    List<int> newPoints;

    bool isDirty;

    void Awake()
    {
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshFilter = gameObject.AddComponent<MeshFilter>();
    }

    void Update()
    {
        life -= Time.deltaTime;
    }

    public void Init(int maxPointPerBatch, Material material)
    {
        this.maxPointPerBatch = maxPointPerBatch;
        vertices = new Vector3[maxPointPerBatch * 6];
        tangent = new Vector3[maxPointPerBatch * 6];
        triangles = new int[maxPointPerBatch * 6];
        uv = new Vector2[maxPointPerBatch * 6];
        verticesIndex = 0;

        mesh = new Mesh();
        meshFilter.mesh = mesh;
        meshRenderer.sharedMaterial = material;
    }

    public void ResetMeshData()
    {
        for (int i = 0; i < maxPointPerBatch * 6; i++)
        {
            vertices[i] = Vector3.zero;
            triangles[i] = 0;
            uv[i] = Vector2.zero;
        }
        verticesIndex = 0;
    }

    public void BuildPoint(AdvanceTrailPoint point)
    {
        if (point.lastPoint == null)
            return;

        isDirty = true;

        if (life < point.life)
            life = point.life;

        point.batch = this;
        point.index = verticesIndex;


        var pLast = point.lastPoint.position;
        var pNew = point.position;

        var avgTangent = UpdateLastPoint(point);
        var newTangent = pNew - pLast;

        Vector3 offset = Vector3.zero;
            
        uv[verticesIndex] = new Vector2(0, 0);
        uv[verticesIndex + 1] = new Vector2(0, 1);
        uv[verticesIndex + 2] = new Vector2(1, 0);

        triangles[verticesIndex] = verticesIndex;
        triangles[verticesIndex + 1] = verticesIndex + 1;
        triangles[verticesIndex + 2] = verticesIndex + 2;

        tangent[verticesIndex] = avgTangent;
        tangent[verticesIndex + 1] = newTangent;
        tangent[verticesIndex + 2] = avgTangent;

        vertices[verticesIndex++] = pLast - offset;
        vertices[verticesIndex++] = pNew - offset;
        vertices[verticesIndex++] = pLast + offset;


        uv[verticesIndex] = new Vector2(1, 0);
        uv[verticesIndex + 1] = new Vector2(0, 1);
        uv[verticesIndex + 2] = new Vector2(1, 1);

        triangles[verticesIndex] = verticesIndex;
        triangles[verticesIndex + 1] = verticesIndex + 1;
        triangles[verticesIndex + 2] = verticesIndex + 2;

        tangent[verticesIndex] = avgTangent;
        tangent[verticesIndex + 1] = newTangent;
        tangent[verticesIndex + 2] = newTangent;

        vertices[verticesIndex++] = pLast + offset;
        vertices[verticesIndex++] = pNew - offset;
        vertices[verticesIndex++] = pNew - offset;
    }

    Vector3 UpdateLastPoint(AdvanceTrailPoint point)
    {
        var lastPoint = point.lastPoint;        
        return point.batch.UpdateLastPoint(point.index, point.position);
    }

    public Vector3 UpdateLastPoint(int index, Vector3 nextPoint)
    {
        var newTangent = (tangent[index] + (nextPoint - vertices[index])).normalized;

        tangent[index + 1] = newTangent;
        tangent[index + 4] = newTangent;
        tangent[index + 5] = newTangent;

        return newTangent;
    }

    public bool IsFull
    {
        get
        {
            return verticesIndex / 6 >= maxPointPerBatch;
        }
    }

    public void UpdateMesh()
    {
        if (!isDirty)
            return;
        
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        isDirty = false;
    }
}
