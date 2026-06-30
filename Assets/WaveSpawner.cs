using UnityEngine;
using System.Collections;

public class WaveSpawner : MonoBehaviour
{
    public enum SpawnState { SPAWNING, WAITING, COUNTDOWN };

    [Header("Zombie Setup")]
    public GameObject baseZombiePrefab;   // Prefab gốc duy nhất
    public ZombieData[] zombieTypes;      // Danh sách file Data (Normal, Fast, Tanker...)

    [Header("Wave Settings")]
    public float timeBetweenWaves = 5f;
    private float waveCountdown;
    private int currentWaveNumber = 1;

    [Header("Screen Spawn Offset")]
    private Camera mainCamera;
    public float spawnDistanceOffset = 3f;

    private SpawnState state = SpawnState.COUNTDOWN;
    private float searchCountdown = 1f;

    void Start()
    {
        mainCamera = Camera.main;
        waveCountdown = timeBetweenWaves;
    }

    void Update()
    {
        if (state == SpawnState.WAITING)
        {
            if (!IsZombieAlive())
            {
                WaveCompleted();
            }
            else
            {
                return;
            }
        }

        if (waveCountdown <= 0)
        {
            if (state != SpawnState.SPAWNING)
            {
                StartCoroutine(SpawnWaveRoutine());
            }
        }
        else
        {
            waveCountdown -= Time.deltaTime;
        }
    }

    void WaveCompleted()
    {
        Debug.Log("Đã xong Wave " + currentWaveNumber + "! Chuẩn bị cho Wave tiếp theo...");
        state = SpawnState.COUNTDOWN;
        waveCountdown = timeBetweenWaves;
        currentWaveNumber++;
    }

    bool IsZombieAlive()
    {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0f)
        {
            searchCountdown = 1f;
            return GameObject.FindGameObjectWithTag("Zombie") != null;
        }
        return true;
    }

    IEnumerator SpawnWaveRoutine()
    {
        Debug.Log("KÍCH HOẠT: Wave " + currentWaveNumber);
        state = SpawnState.SPAWNING;

        int zombieCount = 2 + (currentWaveNumber * 3);
        float spawnRate = 1f + (currentWaveNumber * 0.2f);

        for (int i = 0; i < zombieCount; i++)
        {
            if (zombieTypes.Length > 0)
            {
                // Bốc ngẫu nhiên một file Data trong danh sách bạn đã kéo vào
                ZombieData randomData = zombieTypes[Random.Range(0, zombieTypes.Length)];
                SpawnZombieWithData(baseZombiePrefab, randomData);
            }
            yield return new WaitForSeconds(1f / spawnRate);
        }

        state = SpawnState.WAITING;
        yield break;
    }

    void SpawnZombieWithData(GameObject _prefab, ZombieData _data)
    {
        if (_prefab == null || _data == null) return;

        if (mainCamera == null) mainCamera = Camera.main;

        float camHeight = mainCamera.orthographicSize;
        float camWidth = camHeight * mainCamera.aspect;
        float screenRadius = Mathf.Sqrt(camWidth * camWidth + camHeight * camHeight);
        float spawnRadius = screenRadius + spawnDistanceOffset;

        float randomAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        float spawnX = mainCamera.transform.position.x + Mathf.Cos(randomAngle) * spawnRadius;
        float spawnY = mainCamera.transform.position.y + Mathf.Sin(randomAngle) * spawnRadius;
        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0f);

        GameObject newZombie = Instantiate(_prefab, spawnPosition, Quaternion.identity);

        ZombieController controller = newZombie.GetComponent<ZombieController>();
        if (controller != null)
        {
            controller.zombieData = _data;
        }
        if (PlayerStatus.Instance != null)
        {
            PlayerStatus.Instance.AddWaveCleared();
        }
    }
}