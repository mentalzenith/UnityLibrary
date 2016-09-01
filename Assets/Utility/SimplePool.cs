using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameObjectPool
{
    GameObject prefab;
    Queue<GameObject> pool;

    public GameObjectPool(GameObject prefab)
    {
        this.prefab = prefab;
        pool = new Queue<GameObject>();
    }

    public GameObject Spawn()
    {
        if (pool.Count == 0)
            return GameObject.Instantiate(prefab);
        else
        {
            var gameObject = pool.Dequeue();
            gameObject.SetActive(true);
            return gameObject;
        }
    }

    public void Despawn(GameObject o)
    {
        o.SetActive(false);
        pool.Enqueue(o);
    }
}
