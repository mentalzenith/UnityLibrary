using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AdvanceTrailRenderer : MonoBehaviour
{
    public AdvanceTrailManager manager;

    MeshRenderer meshRenderer;
    MeshFilter meshFilter;

    Mesh mesh;
    Vector3[] vertices;
    Vector4[] tangents;
    Color[] colors;
    int[] triangles;
    Vector2[] uv;
    List<Vector4> uv2;
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
        tangents = new Vector4[maxPointPerBatch * 6];
        triangles = new int[maxPointPerBatch * 6];
        colors = new Color[maxPointPerBatch * 6];
        uv = new Vector2[maxPointPerBatch * 6];
        uv2 = new List<Vector4>(maxPointPerBatch * 6);
        for (int i = 0; i < maxPointPerBatch * 6; i++)
            uv2.Add(new Vector4());
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

        var offset = Vector3.zero;
        var data = new Vector4(point.width, point.life, Time.time, 0);

        var lastColor = point.lastPoint.color;

        //first triangle

        uv[verticesIndex] = new Vector2(0, 0);
        uv[verticesIndex + 1] = new Vector2(0, 1);
        uv[verticesIndex + 2] = new Vector2(1, 0);

        uv2[verticesIndex] = data;
        uv2[verticesIndex + 1] = data;
        uv2[verticesIndex + 2] = data;

        colors[verticesIndex] = lastColor;
        colors[verticesIndex + 1] = point.color;
        colors[verticesIndex + 2] = lastColor;

        triangles[verticesIndex] = verticesIndex;
        triangles[verticesIndex + 1] = verticesIndex + 1;
        triangles[verticesIndex + 2] = verticesIndex + 2;

        tangents[verticesIndex] = avgTangent;
        tangents[verticesIndex + 1] = newTangent;
        tangents[verticesIndex + 2] = avgTangent;

        vertices[verticesIndex++] = pLast - offset;
        vertices[verticesIndex++] = pNew - offset;
        vertices[verticesIndex++] = pLast + offset;

        //second triangle

        uv[verticesIndex] = new Vector2(1, 0);
        uv[verticesIndex + 1] = new Vector2(0, 1);
        uv[verticesIndex + 2] = new Vector2(1, 1);

        uv2[verticesIndex] = data;
        uv2[verticesIndex + 1] = data;
        uv2[verticesIndex + 2] = data;

        colors[verticesIndex] = lastColor;
        colors[verticesIndex + 1] = point.color;
        colors[verticesIndex + 2] = point.color;

        triangles[verticesIndex] = verticesIndex;
        triangles[verticesIndex + 1] = verticesIndex + 1;
        triangles[verticesIndex + 2] = verticesIndex + 2;

        tangents[verticesIndex] = avgTangent;
        tangents[verticesIndex + 1] = newTangent;
        tangents[verticesIndex + 2] = newTangent;

        vertices[verticesIndex++] = pLast + offset;
        vertices[verticesIndex++] = pNew - offset;
        vertices[verticesIndex++] = pNew - offset;

//        print(lastColor+" "+point.color);
    }

    public void UpdatePoint(AdvanceTrailPoint point)
    {
        if (point.lastPoint == null)
            return;
        
        isDirty = true;

        var index = point.index;
        var newTangent = UpdateLastPoint(point);     
        var data = new Vector4(point.width, point.life, Time.time, 0);
        var lastColor = point.lastPoint.color;
       
        vertices[index + 1] = point.position;
        vertices[index + 4] = point.position;
        vertices[index + 5] = point.position;

        uv2[index + 1] = data;
        uv2[index + 4] = data;
        uv2[index + 5] = data;

        colors[index] = lastColor;
        colors[index + 1] = point.color;
        colors[index + 2] = lastColor;

        colors[index + 3] = lastColor;
        colors[index + 4] = point.color;
        colors[index + 5] = point.color;


        tangents[index + 0] = newTangent;
        tangents[index + 1] = newTangent;
        tangents[index + 2] = newTangent;

        tangents[index + 3] = newTangent;
        tangents[index + 4] = newTangent;
        tangents[index + 5] = newTangent;
    }

    Vector3 UpdateLastPoint(AdvanceTrailPoint point)
    {
        var lastPoint = point.lastPoint;
        if (lastPoint.batch == null)
            return Vector3.zero;

        var lastLastPoint = lastPoint.lastPoint; 
        if (lastLastPoint == null)
            return lastPoint.batch.UpdateLastPoint(lastPoint.index, lastPoint.position, point.position);
        else
            return lastPoint.batch.UpdateLastPoint(lastPoint.index, lastLastPoint.position, point.position);
    }

    Vector3 UpdateLastPoint(int index, Vector3 lastPoint, Vector3 nextPoint)
    {
//        var newTangent = (tangent[index] + (nextPoint - vertices[index])).normalized;
        var newTangent = (nextPoint - lastPoint).normalized;

        tangents[index + 1] = newTangent;
        tangents[index + 4] = newTangent;
        tangents[index + 5] = newTangent;

        return newTangent;
    }

    public bool IsFull
    {
        get
        {
            return verticesIndex / 6 >= maxPointPerBatch;
        }
    }

    public bool UpdateMesh()
    {
        if (!isDirty)
            return false;
        
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.tangents = tangents;
        mesh.uv = uv;
        mesh.SetUVs(1, uv2);
        mesh.colors = colors;

        mesh.RecalculateBounds();

        isDirty = false;
        return true;
    }
}
