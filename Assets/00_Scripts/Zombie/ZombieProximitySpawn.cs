using System.Collections;
using UnityEngine;

public class ZombieProximitySpawn : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnRadius = 10f;
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private int maxEnemyCount = 10;
    [SerializeField] private Collider2D playAreaCollider;
    [SerializeField] private MessageRemove messageRemoveSwitch;
    [SerializeField] private MessageRemove messageRemoveWave;
    [SerializeField] private MessageRemove messageEndBox;

    private int swarmSize;
    private Transform playerTransform;
    private int currentEnemyCount;
    private float timer;
    private bool messageShownSwitch;
    private bool messageShownWave;
    private int movementSpeed = 2;
    private bool endIsNear;

    private void Start()
    {
        currentEnemyCount = 0;
        timer = spawnInterval;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (currentEnemyCount < maxEnemyCount)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                if (IsOutsidePlayerFieldOfView() && IsInsidePlayArea(transform.position))
                {
                    SpawnSwarm();
                    currentEnemyCount += swarmSize;
                }

                timer = spawnInterval;
            }
        }
        if (currentEnemyCount >= 5 && !messageShownSwitch)
        {
            messageRemoveSwitch.ShowImage();
            messageShownSwitch = true;
        }
        if (currentEnemyCount >= maxEnemyCount && !messageShownWave)
        {
            messageRemoveWave.ShowImage();
            messageShownWave = true;
            StartCoroutine(StartWave());
        }
        if (currentEnemyCount >= maxEnemyCount && endIsNear)
        {
            messageEndBox.ShowImage();
        }
    }

    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(2f);
        currentEnemyCount = 0;
        movementSpeed = 4;
        endIsNear = true;
    }

    private bool IsOutsidePlayerFieldOfView()
    {
        Vector2 playerToSpawner = transform.position - playerTransform.position;
        Vector2 playerForward = playerTransform.up;

        float dotProduct = Vector2.Dot(playerToSpawner.normalized, playerForward);

        return dotProduct < 0f;
    }

    private void SpawnSwarm()
    {
        swarmSize = Random.Range(1, 4);
        for (int i = 0; i < swarmSize; i++)
        {
            Vector3 spawnPosition = GenerateRandomSpawnPosition();
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            enemyPrefab.GetComponent<ZombieMovement>().SetRunningSpeed(movementSpeed);
        }
    }

    private Vector3 GenerateRandomSpawnPosition()
    {
        Vector3 playerPosition = playerTransform.position;
        float angle = Random.Range(0f, 360f);
        float distance = Random.Range(spawnRadius, spawnRadius * 1.5f);
        Vector3 spawnPosition = playerPosition + Quaternion.Euler(0f, 0f, angle) * Vector3.up * distance;

        if (!IsInsidePlayArea(spawnPosition))
        {
            spawnPosition = GenerateRandomSpawnPosition();
        }

        return spawnPosition;
    }

    private bool IsInsidePlayArea(Vector3 position)
    {
        if (playAreaCollider == null)
        {
            Debug.LogWarning("Play area collider is not assigned!");
            return false;
        }

        Collider2D enemyCollider = enemyPrefab.GetComponent<Collider2D>();
        Bounds enemyBounds = enemyCollider.bounds;
        Collider2D[] colliders = Physics2D.OverlapBoxAll(position, enemyBounds.size, 0f);

        foreach (Collider2D collider in colliders)
        {
            if (collider == playAreaCollider)
            {
                return true;
            }
        }

        return false;
    }
}
