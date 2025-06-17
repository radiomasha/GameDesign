using System.Collections.Generic;
using UnityEngine;

public class GeneralObjPool : MonoBehaviour
{

    public GameObject prefab;
    public int initialSize = 10;
    public int maxPoolSize = 100;

    private Queue<GameObject> pool = new Queue<GameObject>();
    private List<GameObject> allObjects = new List<GameObject>();

    void Awake()
    {
        ExpandPool(initialSize);
    }

    public GameObject Get()
    {
        if (pool.Count == 0)
        {
            if (allObjects.Count >= maxPoolSize)
            {
                Debug.LogError($"Pool for {prefab.name} reached max size.");
                return null;
            }
            ExpandPool(5);
        }

        GameObject obj = pool.Dequeue();
        obj.SetActive(true);
        obj.transform.SetParent(null, true); // Отключаем наследование трансформа
        return obj;
    }

    public void Return(GameObject obj)
    {
        if (obj == null) return;

        obj.SetActive(false);
        obj.transform.SetParent(transform, true);
        pool.Enqueue(obj);
    }

    private void ExpandPool(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject obj = Instantiate(prefab, transform);
            obj.SetActive(false);
            pool.Enqueue(obj);
            allObjects.Add(obj);
        }
    }
}