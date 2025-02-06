using UnityEngine;

public class EnemyBulletSpawner : MonoBehaviour
{
    public EnemyBullet enemyBullet;

    public Transform spawnPoint;    // The point from where the bullet will be spawned
    public Transform shooter;       // The target transform (shooter)

    private void Start()
    {
        shooter = GameObject.FindWithTag("Shooter").transform;
        if (enemyBullet != null)
        {
            InvokeRepeating(nameof(SpawnBullet), enemyBullet.spawnInterval, enemyBullet.spawnInterval);
        }
        else
        {
            Debug.LogError("enemyBullet is null! Assign it in the Inspector.");
        }

    }

    private void SpawnBullet()
    {
        if (enemyBullet.bulletPrefab == null || spawnPoint == null || shooter == null)
        {
            return;
        }

        // Instantiate the bullet at the spawn point
        GameObject bullet = Instantiate(enemyBullet.bulletPrefab, spawnPoint.position, spawnPoint.rotation);

        // Calculate the direction towards the shooter
        Vector2 direction = (shooter.position - spawnPoint.position).normalized;

        // Set the bullet's velocity to move towards the shooter
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * enemyBullet.bulletSpeed;
        }
    }



}
