using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class TreasureSpawner : MonoBehaviour
{
    [Header("AR Components")]
    public ARRaycastManager arRaycastManager;
    public ARPlaneManager arPlaneManager;

    [Header("Treasure Settings")]
    public GameObject treasurePrefab;
    public GameObject bonusTreasurePrefab;
    public int maxTreasures = 8;
    public float minSpawnDistance = 0.4f;
    public float spawnInterval = 2.5f;

    private List<GameObject> _spawnedTreasures = new List<GameObject>();
    private bool _spawning = false;
    private Coroutine _spawnCoroutine;

    public void StartSpawning()
    {
        _spawning = true;
        _spawnCoroutine = StartCoroutine(SpawnRoutine());
    }

    public void StopSpawning()
    {
        _spawning = false;
        if (_spawnCoroutine != null)
            StopCoroutine(_spawnCoroutine);
    }

    public void ClearAllTreasures()
    {
        foreach (var t in _spawnedTreasures)
        {
            if (t != null) Destroy(t);
        }
        _spawnedTreasures.Clear();
    }

    IEnumerator SpawnRoutine()
    {
        while (_spawning)
        {
            _spawnedTreasures.RemoveAll(t => t == null);
            if (_spawnedTreasures.Count < maxTreasures)
                TryAutoSpawn();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void TryAutoSpawn()
    {
        foreach (var plane in arPlaneManager.trackables)
        {
            if (_spawnedTreasures.Count >= maxTreasures) break;

            Vector3 randomPoint = plane.transform.position + new Vector3(
    Random.Range(-2.5f, 2.5f),
    0.05f,
    Random.Range(-2.5f, 2.5f)
);

            if (IsFarEnoughFromOthers(randomPoint))
            {
                SpawnTreasure(randomPoint);
                break;
            }
        }
    }

    bool IsFarEnoughFromOthers(Vector3 point)
    {
        foreach (var t in _spawnedTreasures)
        {
            if (t != null && Vector3.Distance(t.transform.position, point) < minSpawnDistance)
                return false;
        }
        return true;
    }

    void SpawnTreasure(Vector3 position)
    {
        bool isBonus = bonusTreasurePrefab != null && Random.value < 0.08f;
        GameObject prefab = isBonus ? bonusTreasurePrefab : treasurePrefab;

        if (prefab == null) return;

        GameObject treasure = Instantiate(prefab, position, Quaternion.identity);

        TreasureItem item = treasure.GetComponent<TreasureItem>();
        if (item == null) item = treasure.AddComponent<TreasureItem>();
        if (isBonus) item.pointValue = 30;

        _spawnedTreasures.Add(treasure);
    }

    public void RemoveTreasure(GameObject treasure)
    {
        _spawnedTreasures.Remove(treasure);
    }
}