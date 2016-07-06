using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public partial class Drawer : SingletonMono<Drawer>
{
    public GameObject pointPrefab;
    Queue<GameObject> queue;
    List<GameObject> list;

    void Awake()
    {
        queue = new Queue<GameObject>();
        list = new List<GameObject>();
    }
	
    void OnRenderObject()
    {
        foreach (var pointObject in list)
        {
            pointObject.SetActive(false);
            queue.Enqueue(pointObject);
        }
        list.Clear();
    }

    public static void DrawPoint(Vector3 position, Color color, float diameter)
    {
        DrawPoint((Vector2)Camera.main.WorldToScreenPoint(position), color, diameter);
    }

    public static void DrawPoint(Vector2 screenPoint, Color color, float diameter)
    {
        GameObject pointObject = null;
        if (Instance.queue.Count > 0)
            pointObject = Instance.queue.Dequeue();
        else
            pointObject = Instantiate(Instance.pointPrefab) as GameObject;

        pointObject.transform.position = screenPoint;
        pointObject.SetActive(true);
        pointObject.transform.SetParent(Instance.transform, false);

        var point = pointObject.GetComponentInChildren<Image>();
        point.color = color;
        point.rectTransform.sizeDelta = new Vector2(diameter, diameter);

        Instance.list.Add(pointObject);
    }
}