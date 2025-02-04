using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParatrooperSpawner : MonoBehaviour
{
    // Public 
    public GameObject paratrooperPrefab;
    public Transform spawnPoint;
    public float minSpwanInterval = 0.5f;
    public float maxSpwanInterval = 2.0f;
    public float landingSpeed = 1.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(spawnParatroopers), Random.Range(minSpwanInterval, maxSpwanInterval));
    }

    void spawnParatroopers()
    {
        if (spawnPoint == null || paratrooperPrefab == null)
        {
            return;
        }

        GameObject paratrooper = Instantiate(paratrooperPrefab, spawnPoint.position, spawnPoint.rotation);

        Rigidbody2D rb = paratrooper.GetComponent<Rigidbody2D>();
        if (rb != null )
        {
            rb.velocity = Vector2.down * landingSpeed;
        }

    }
}
