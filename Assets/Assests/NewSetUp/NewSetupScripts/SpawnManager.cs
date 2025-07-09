using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{ 
 [Header("Spawn Area")]
    public BoxCollider spawnZone;
    public Transform deadZone;
    public Transform backgroundGroup;
    private Bounds bounds;

    [Header("Pools")]
    public GeneralObjPool backgroundPool;
    public GeneralObjPool debrisPool;
    public GeneralObjPool bonusPool;

    [Header("Background Batching")]
    public Vector2Int backgroundBatchRange = new Vector2Int(10, 30);

    [Header("Bonus Batching")]
    public Vector2Int bonusBatchRange = new Vector2Int(1, 5);
    public float bonusInterval = 4f; // ← how often to spawn bonuses

    [Header("Debris Settings")]
    public Vector2 debrisIntervalRange = new Vector2(1f, 3f);
    public int debrisMinPerWave = 1;
    public int debrisMaxPerWave = 5;
    public Vector3 debrisMoveDirection = Vector3.back;
    public float debrisMinSpeed = 10f;
    public float debrisMaxSpeed = 25f;

    [Header("Scale Settings")]
    public bool uniformScale = true;
    public Vector3 backgroundScaleMin = Vector3.one;
    public Vector3 backgroundScaleMax = Vector3.one;
    public Vector3 debrisScaleMin = Vector3.one;
    public Vector3 debrisScaleMax = Vector3.one;
    public Vector3 bonusScaleMin = Vector3.one;
    public Vector3 bonusScaleMax = Vector3.one;

    private List<Transform> debrisObjects = new();
    private List<Transform> bonusObjects = new();
    private List<Transform> backgroundObjects = new();

    void Start()
    {
        bounds = spawnZone.bounds;

        SpawnStaticBatch(backgroundPool, backgroundBatchRange, backgroundScaleMin, backgroundScaleMax, backgroundObjects, backgroundGroup);

        StartCoroutine(DebrisLoop());
        StartCoroutine(BonusLoop()); // ← NEW
    }

    void Update()
    {
        CheckDeadzone(debrisObjects, debrisPool);
        CheckDeadzone(bonusObjects, bonusPool);
        CheckDeadzone(backgroundObjects, backgroundPool);
    }

    void SpawnStaticBatch(GeneralObjPool pool, Vector2Int batchRange, Vector3 minScale, Vector3 maxScale, List<Transform> list, Transform parent)
    {
        int count = Random.Range(batchRange.x, batchRange.y + 1);
        for (int i = 0; i < count; i++)
        {
            GameObject obj = pool.Get();
            obj.transform.position = GetRandomPointInBounds();
            obj.transform.localScale = GetRandomScale(minScale, maxScale);
            obj.transform.rotation = Random.rotation;
            obj.transform.SetParent(parent, true);
            list.Add(obj.transform);
        }
    }

    IEnumerator DebrisLoop()
    {
        while (true)
        {
            SpawnDebris();
            float wait = Random.Range(debrisIntervalRange.x, debrisIntervalRange.y);
            yield return new WaitForSeconds(wait);
        }
    }

    IEnumerator BonusLoop()
    {
        while (true)
        {
            List<Vector3> usedPositions = new();
            float minDistance = 1.5f; // Минимальное расстояние между объектами
            int maxAttempts = 25;

            while (true)
            {
                GameObject obj = bonusPool.Get();
                if (obj == null) break;

                Vector3 spawnPos = Vector3.zero;
                bool valid = false;
                int attempts = 0;

                // Найти подходящую позицию
                while (!valid && attempts < maxAttempts)
                {
                    spawnPos = GetRandomPointInBounds();
                    valid = true;

                    foreach (Vector3 pos in usedPositions)
                    {
                        if (Vector3.Distance(pos, spawnPos) < minDistance)
                        {
                            valid = false;
                            break;
                        }
                    }

                    attempts++;
                }

                if (!valid)
                {
                    // Не удалось найти подходящее место — пропускаем этот объект
                    bonusPool.Return(obj);
                    continue;
                }

                usedPositions.Add(spawnPos);
                obj.transform.position = spawnPos;
                obj.transform.localScale = GetRandomScale(bonusScaleMin, bonusScaleMax);
                obj.transform.SetParent(backgroundGroup, true);
                bonusObjects.Add(obj.transform);
            }

            yield return new WaitForSeconds(bonusInterval);
        }
    }



    void SpawnDebris()
    {
        int count = Random.Range(debrisMinPerWave, debrisMaxPerWave + 1);
        for (int i = 0; i < count; i++)
        {
            GameObject obj = debrisPool.Get();
            obj.transform.SetParent(null); 
            Debug.Log($"[Spawn] {obj.name} scale: {obj.transform.localScale}");
            obj.transform.localScale = GetRandomScale(debrisScaleMin, debrisScaleMax);
            obj.transform.position = GetSpawnPointOnBack(bounds);
            obj.transform.rotation = Random.rotation;



            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = obj.AddComponent<Rigidbody>();
                rb.useGravity = false;
                rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            }

            float speed = Random.Range(debrisMinSpeed, debrisMaxSpeed);
            rb.linearVelocity = debrisMoveDirection.normalized * speed;

            debrisObjects.Add(obj.transform);
        }
    }
void CheckDeadzone(List<Transform> list, GeneralObjPool pool)
{
    for (int i = list.Count - 1; i >= 0; i--)
    {
        Transform obj = list[i];
        if (obj == null) continue;

        if (obj.position.z < deadZone.position.z)
        {
            list.RemoveAt(i);
            //Destroy(gameObject);
            pool.Return(obj.gameObject);
        }
    }
}

    Vector3 GetRandomPointInBounds()
    {
        Bounds b = spawnZone.bounds; // ⚠️ берём актуальные bounds каждый раз
        return new Vector3(
            Random.Range(b.min.x, b.max.x),
            Random.Range(b.min.y, b.max.y),
            Random.Range(b.min.z, b.max.z)
        );
    }

    Vector3 GetSpawnPointOnBack(Bounds b)
    {
        return new Vector3(
            Random.Range(b.min.x, b.max.x),
            Random.Range(b.min.y, b.max.y),
            b.max.z
        );
    }

    Vector3 GetRandomScale(Vector3 min, Vector3 max)
    {
        if (uniformScale)
        {
            float s = Random.Range(min.x, max.x);
            return new Vector3(s, s, s);
        }
        else
        {
            return new Vector3(
                Random.Range(min.x, max.x),
                Random.Range(min.y, max.y),
                Random.Range(min.z, max.z)
            );
        }
    }
}