using System;

using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] sampahPrefabs;
    [SerializeField] private BoxCollider2D spawnPoint;
    [SerializeField] private GameObject Sampah;
    [SerializeField] private Transform map;

    [Header("Options")]
    [SerializeField] private Vector2[] SpawnTongSampah;
    [SerializeField] private float moveStep = 2f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float endX = -6f; // batas ujung
    



    private Vector3 startPos;
    public static GameManager GameInstance;

    public float Score { get; private set; }
    public Action<float> UpdateScore;

    private GameObject currentSampah;

    private bool isMoving;

    void Awake()
    {
        if (GameInstance != null && GameInstance != this)
        {
            Destroy(gameObject);
            return;
        }

        GameInstance = this;
        //DontDestroyOnLoad(gameObject);
        ResetGame();
    }
    public void ResetGame()
    {
        Score = 0;
        UpdateScore?.Invoke(Score);

        map.position = startPos;

        if (currentSampah != null)
            Destroy(currentSampah);

        SpawnSampah();
    }
    void Start()
    {
        startPos = map.position;

    }

    public void AddScore(float value)
    {
        Score += value;

        UpdateScore?.Invoke(Score);
    }

    public void NextLevel()
    {
        StartCoroutine(MoveAndSpawn());
    }

    void SpawnSampah()
    {


        int randomIndex = UnityEngine.Random.Range(0, sampahPrefabs.Length);

        Bounds bounds = spawnPoint.bounds;

        float randomX = UnityEngine.Random.Range(bounds.min.x, bounds.max.x);
        float randomY = UnityEngine.Random.Range(bounds.min.y, bounds.max.y);

        Vector3 spawnPos = new Vector3(randomX, randomY, 0f);

        currentSampah = Instantiate(sampahPrefabs[randomIndex], spawnPos, Quaternion.identity);
        int RandomIndex = UnityEngine.Random.Range(0, SpawnTongSampah.Length);
        Sampah.gameObject.transform.position =  SpawnTongSampah[randomIndex];
    }

    IEnumerator MoveAndSpawn()
    {
        isMoving = true;

        // GERAK KE KIRI
        while (map.position.x > endX)
        {
            map.position += Vector3.left * moveSpeed * Time.deltaTime;
            yield return null;
        }

        // tunggu sebentar
        yield return new WaitForSeconds(0.5f);

        // reset posisi map
        map.position = startPos;

        // ✅ BARU spawn setelah animasi selesai
        SpawnSampah();

        isMoving = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Sampah"))
        {
            UIManager.UIManagerSingle.ShowPopup();
        }
    }

}