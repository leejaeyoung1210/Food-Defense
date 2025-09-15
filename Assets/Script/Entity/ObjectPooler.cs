using JetBrains.Annotations;
using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class ObjectPooler : MonoBehaviour
{
    public GameObject prefab;
    public int poolSize = 0;
    private List<GameObject> pool;
    void Start()
    {
        pool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            CteateNewGameObject();
        }
    }

    private GameObject CteateNewGameObject()
    {
        GameObject obj = Instantiate(prefab, transform);
        obj.SetActive(false);
        pool.Add(obj);
        return obj;
    }
    public GameObject GetPoolobject()
    {
        foreach (var obj in pool)
        {
            if (!obj.activeSelf)
            {               
                    return obj;
            }
        }
        return CteateNewGameObject();
    }

}
