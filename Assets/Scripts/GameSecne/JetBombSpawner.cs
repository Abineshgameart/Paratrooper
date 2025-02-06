using UnityEngine;

public class JetBombSpawner : MonoBehaviour
{
    public GameObject bulletPrefab; // The bullet prefab to spawn
    public Transform spawnPoint;    // The point from where the bullet will be spawned
    public Transform shooter;       // The target transform (shooter)
    public float bombSpeed = 5f;  // The speed of the bullet
    public float spawnInterval = 1f; // Time interval between bullet spawns

    private void Start()
    {
        shooter = GameObject.FindWithTag("Shooter").transform;
        InvokeRepeating(nameof(SpawnBullet), spawnInterval, spawnInterval);
    }

    private void SpawnBullet()
    {
        if (bulletPrefab == null || spawnPoint == null || shooter == null)
        {
            Debug.LogError("Bullet prefab, spawn point, or shooter reference is missing!");
            return;
        }

        // Instantiate the bullet at the spawn point
        GameObject bullet = Instantiate(bulletPrefab, spawnPoint.position, spawnPoint.rotation);

        // Calculate the direction towards the shooter
        Vector2 direction = (shooter.position - spawnPoint.position).normalized;

        // Set the bullet's velocity to move towards the shooter
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * bombSpeed;
        }

        Debug.Log("Bullet spawned and moving towards shooter at position: " + shooter.position);



    }
}
