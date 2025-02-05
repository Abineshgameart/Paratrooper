using UnityEngine;

[CreateAssetMenu(fileName = "EnemyBullet", menuName = "ScriptableObjects/EnemyBullet")]
public class EnemyBullet : ScriptableObject
{
    public string bulletName;
    public GameObject bulletPrefab;
    public float bulletSpeed;
    public float spawnInterval;
}
