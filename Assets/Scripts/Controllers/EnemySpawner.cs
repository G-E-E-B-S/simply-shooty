using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] GameObject enemyPrefab;

    private void Start()
    {
        foreach(Transform spawnPoint in spawnPoints)
        {
            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            enemy.transform.parent = spawnPoint;
        }
    }
}
