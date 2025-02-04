using System.Collections.Generic;
using UnityEngine;

public class ParatrooperController : MonoBehaviour
{
    //// Public
    //public Transform shooter;
    //public float moveSpeed = 1f;
    //public float climbSpeed = 3f;
    //public float stopDistance = 0.5f;
    //public LayerMask paratrooperLayerMask;

    //private static List<ParatrooperController> paratrooperGroup = new List<ParatrooperController>();
    //private bool isClimbing = false;
    //private bool isMoving = false;
    //private Transform climbTarget;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    shooter = GameObject.FindWithTag("Shooter").transform;
    //    paratrooperGroup.Add(this);
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    if (paratrooperGroup.Count >= 4 && !isMoving && !isClimbing)
    //    {
    //        isMoving = true;
    //        StartCoroutine(MoveToShooter());
    //    }
    //}

    //IEnumerator MoveToShooter()
    //{
    //    while (Vector2.Distance(transform.position, shooter.position) > stopDistance)
    //    {
    //        transform.position = Vector2.MoveTowards(transform.position, shooter.position, moveSpeed * Time.deltaTime);
    //        yield return null;
    //    }

    //    CheckStacking();
    //}

    //void CheckStacking()
    //{
    //    Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(transform.position, 1f, paratrooperLayerMask);
    //    List<Transform> stackList = new List<Transform>();

    //    foreach (var enemy in nearbyEnemies)
    //    {
    //        stackList.Add(enemy.transform);
    //    }

    //    stackList = stackList.OrderBy(paratrooper => paratrooper.position.y).ToList();

    //    if (stackList.Count >= 4)
    //    {
    //        StartStacking(stackList);
    //    }
    //}

    //void StartStacking(List<Transform> stackList)
    //{
    //    isClimbing = true;

    //    if (stackList.Count >= 4)
    //    {
    //        climbTarget = stackList[2]; // Start Climbing on the Third enemy
    //        StartCoroutine(ClimbSequence());
    //    }
    //}

    //IEnumerator ClimbSequence()
    //{
    //    while (Vector2.Distance(transform.position, climbTarget.position + Vector3.up * 1f) > 0.1f)
    //    {
    //        transform.position = Vector2.MoveTowards(transform.position, climbTarget.position + Vector3.up * 1f, climbSpeed * Time.deltaTime);
    //        yield return null;
    //    }

    //    isClimbing =false;
    //}

    //private void nDestroy()
    //{
    //    paratrooperGroup.Remove(this);
    //}


    public float moveSpeed = 2.0f;
    public Transform shooter; // Reference to the shooter's position
    public LayerMask enemyLayer; // Layer mask for enemies
    public float gizmosRadius = 0.5f;

    private static Queue<ParatrooperController> enemyQueue = new Queue<ParatrooperController>();
    private bool isStacked = false;
    private bool isMoving = false;
    private static bool isAnyEnemyMoving = false;

    void Start()
    {
        shooter = GameObject.FindWithTag("Shooter").transform;
        enemyQueue.Enqueue(this);
    }

    void Update()
    {
        if (isStacked) return;

        if (!isMoving && !isAnyEnemyMoving && enemyQueue.Peek() == this)
        {
            isMoving = true;
            isAnyEnemyMoving = true;
        }

        if (isMoving)
        {
            MoveTowardsShooter();
        }
    }

    void MoveTowardsShooter()
    {
        Vector3 direction = (shooter.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, shooter.position);

        if (distance > 2.0f) // Move towards the shooter
        {
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
        else // Check for stacking
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, 0.5f, enemyLayer);
            if (hitEnemies.Length > 1)
            {
                foreach (Collider2D enemy in hitEnemies)
                {
                    if (enemy.gameObject != gameObject)
                    {
                        transform.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y + 1.0f, transform.position.z);
                        isStacked = true;
                        isMoving = false;
                        isAnyEnemyMoving = false;
                        enemyQueue.Dequeue();

                        if (enemyQueue.Count > 0)
                        {
                            enemyQueue.Peek().isMoving = true;
                            isAnyEnemyMoving = true;
                        }
                        break;
                    }
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, gizmosRadius);
    }


}
