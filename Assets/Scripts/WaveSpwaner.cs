using UnityEngine;

[System.Serializable]
public class Wave
{
    public string waveName;
    public int NumOfEnemies;
    public GameObject[] typesOfEnemies;
    public float minSpwanInterval;
    public float maxSpwanInterval;
}


public class WaveSpwaner : MonoBehaviour
{
    // Public
    public Wave[] waves;
    public Transform[] spwanPoints;
    public Transform jetSpwan;

    // Private
    private Wave currentWave;
    private int currentWaveNumber;
    private float nextSpwanTime;

    private bool canSpwan = true;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentWave = waves[currentWaveNumber];
        SpawnWave();
        
    }

    void SpawnWave()
    {
        GameObject[] totalEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (canSpwan && nextSpwanTime < Time.time)
        {
            GameObject randomEnemy = currentWave.typesOfEnemies[Random.Range(0, currentWave.typesOfEnemies.Length)];
            Transform randomPoint = spwanPoints[Random.Range(0, spwanPoints.Length)];
            if (currentWaveNumber == 1)
            {
                randomPoint = jetSpwan;
            }

            Instantiate(randomEnemy, randomPoint.position, randomPoint.rotation);
            currentWave.NumOfEnemies--;
            nextSpwanTime = Time.time + Random.Range(currentWave.minSpwanInterval, currentWave.maxSpwanInterval);

            if(currentWave.NumOfEnemies == 0)
            {
                canSpwan = false;
            }
        }

        if (totalEnemies.Length == 0 && !canSpwan && (currentWaveNumber + 1) != waves.Length)
        {
            SpawnNextWave();
        }

    }

    void SpawnNextWave()
    {
        currentWaveNumber++;
        canSpwan = true;
    }

}
