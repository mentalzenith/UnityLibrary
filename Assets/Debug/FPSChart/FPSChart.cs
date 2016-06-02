using UnityEngine;
using System.Collections;

public class FPSChart : MonoBehaviour
{
    public int dataSampleSize = 100;
    public Vector2 size = new Vector2(200, 100);
    public float maxTime = 0.1f;

    Texture2D data;
    Color[] colors;
    float[] deltaTimes;
    float averageDeltaTime;
    float maxDeltaTime;

    int _index;

    int Index
    {
        get
        {
            return _index;
        }
        set
        {
            if (value < data.width)
                _index = value;
            else
                _index = 0;
        }
    }

    Material material;

    void Awake()
    {
//        data = new Texture2D(dataSampleSize, 10);
        data = new Texture2D(dataSampleSize, 1);
        deltaTimes = new float[dataSampleSize];
        colors = new Color[dataSampleSize];
        var shader = Shader.Find("Debug/FPSChart");
        if (shader != null)
            material = new Material(shader);
        else
            Debug.Log("Shader not found");
    }

    void Update()
    {
        deltaTimes[Index++] = Time.deltaTime;
//        ReScale();
//        data.SetPixels(colors);
        maxDeltaTime = GetMaxDeltaTime();
        averageDeltaTime = GetAverage();
        var normalizedValue = Time.deltaTime / maxTime;
        data.SetPixel(Index, 0, new Color(normalizedValue, 0, 0));
        data.Apply();
    }

    void ReScale()
    {
        maxDeltaTime = GetMaxDeltaTime();
        averageDeltaTime = GetAverage();
        for (int i = 0; i < data.width; i++)
            colors[i] = new Color((deltaTimes[i] - averageDeltaTime) / maxDeltaTime, 0, 0);
    }

    float GetMaxDeltaTime()
    {
        float max = 0;
        for (int i = 0; i < deltaTimes.Length; i++)
            if (deltaTimes[i] > max)
                max = deltaTimes[i];
        return max;
    }

    float GetAverage()
    {
        float avg = 0;
        for (int i = 0; i < deltaTimes.Length; i++)
            avg += deltaTimes[i];
        return avg / deltaTimes.Length;
    }

    void OnGUI()
    {
        if (Event.current.type.Equals(EventType.Repaint))
            Graphics.DrawTexture(new Rect(0, Screen.height - size.y, size.x, size.y), data, material);

        var fontStyle = new GUIStyle();
        fontStyle.fontSize = (int)size.y / 3;
        fontStyle.normal.textColor = Color.green;
        GUI.Label(new Rect(size.x, Screen.height - size.y, 100, size.y / 3), "Low: " + (1 / maxDeltaTime).ToString(), fontStyle);
        GUI.Label(new Rect(size.x, Screen.height - size.y / 3, 100, size.y / 3), "Avg: " + (1 / averageDeltaTime).ToString(), fontStyle);
    }
}
