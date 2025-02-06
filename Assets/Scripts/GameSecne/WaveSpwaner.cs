using UnityEngine;

[System.Serializable]
public class Wave
{
    public string waveName;
    public int NumOfEnemies;
    public GameObject[] typesOfEnemies;
    public float minSpwanInterval;
    public float maxSpwanInterval;
    public float movementSpeed;
}


public class WaveSpwaner : MonoBehaviour
{
    // Public
    public Wave[] waves;
    public Transform[] spwanPoints;
    public Transform jetSpwan;

    public GameObject CompletedMenuUI;


    // Private
    private Wave currentWave;
    private int currentWaveNumber;
    private float nextSpwanTime;
    private GameObject spawnedEnemy;
    private Rigidbody2D spawnedEnemyRb;
    private GameObject randomEnemy;
    private Transform randomPoint;

    private bool canSpwan = true;

    ParatrooperManager ParatrooperManager;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentWave = waves[currentWaveNumber];

        ParatrooperManager = FindAnyObjectByType<ParatrooperManager>();
        if (!ParatrooperManager.stopSpawning)
        {
            SpawnWave();
        }

    }

    void SpawnWave()
    {
        GameObject[] totalEnemies = GameObject.FindGameObjectsWithTag("Aircraft");

        if (canSpwan && nextSpwanTime < Time.time)
        {
            randomEnemy = currentWave.typesOfEnemies[Random.Range(0, currentWave.typesOfEnemies.Length)];


            randomPoint = spwanPoints[Random.Range(0, spwanPoints.Length)];

            if (currentWaveNumber == 1)
            {
                randomPoint = jetSpwan;
            }

            spawnedEnemy = Instantiate(randomEnemy, randomPoint.position, randomPoint.rotation);

            spawnedEnemyRb = spawnedEnemy.GetComponent<Rigidbody2D>();

            EnemyMovments();

            currentWave.NumOfEnemies--;
            nextSpwanTime = Time.time + Random.Range(currentWave.minSpwanInterval, currentWave.maxSpwanInterval);

            if (currentWave.NumOfEnemies == 0)
            {
                canSpwan = false;
            }
        }

        if (totalEnemies.Length == 0 && !canSpwan && (currentWaveNumber + 1) != waves.Length)
        {
            SpawnNextWave();
        }

        //else if((currentWaveNumber + 1) == waves.Length) {
        //    completedMenu();
        //}

    }

    void SpawnNextWave()
    {
        currentWaveNumber++;
        canSpwan = true;
    }

    void EnemyMovments()
    {
        if (spawnedEnemyRb != null)
        {
            spawnedEnemyRb.velocity = randomPoint.right * currentWave.movementSpeed;
        }
    }


    void completedMenu()
    {
        Time.timeScale = 1f;
        CompletedMenuUI.SetActive(true);
    }

}
