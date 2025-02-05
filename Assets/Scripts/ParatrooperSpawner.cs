using UnityEngine;

public class ParatrooperSpawner : MonoBehaviour
{
    // Public 
    public GameObject paratrooperPrefab;
    public Transform spawnPoint;
    public float minSpawnInterval = 0.5f;
    public float maxSpawnInterval = 2.0f;
    public float landingSpeed = 1.0f;
    public float centerGap = 4.0f;

    private ParatrooperManager paratrooperManager;

    
    // Start is called before the first frame update
    void Start()
    {
        paratrooperManager = FindAnyObjectByType<ParatrooperManager>();

        if (!paratrooperManager.stopSpawning)
        {
            float spawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
            Invoke(nameof(SpawnParatroopers), spawnInterval);
        }
        
    }


    void SpawnParatroopers()
    {
        if (spawnPoint == null || paratrooperPrefab == null)
        {
            return;
        }

        // Calculate the Screen Bounds
        float screenHeight = Camera.main.orthographicSize * 2;
        float screenWidth = screenHeight * Camera.main.aspect;

        // Define the valid spawn region relative to the plane's position
        float spawnX = spawnPoint.position.x;

        if (Mathf.Abs(spawnX) < centerGap / 2)
        {
            // Adjust the spawnX to be outside the center gap
            spawnX = (spawnX < 0) ? -(centerGap / 2) : (centerGap / 2);
        }

        // Clamp the spawnX within screen bounds
        spawnX = Mathf.Clamp(spawnX, -screenWidth / 2, screenWidth / 2);

        Vector2 spawnPosition = new Vector2(spawnX, spawnPoint.position.y);

        GameObject paratrooper = Instantiate(paratrooperPrefab, spawnPosition, spawnPoint.rotation);

        Rigidbody2D rb = paratrooper.GetComponent<Rigidbody2D>();
        if (rb != null )
        {
            rb.velocity = Vector2.down * landingSpeed;
        }

    }
}
