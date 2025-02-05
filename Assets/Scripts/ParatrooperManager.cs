using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ParatrooperManager : MonoBehaviour
{
    //public Transform shooter;
    //public Transform obstacle; // The square sprite (obstacle) that paratroopers climb
    //public float delayMoveToShooter = 10f;
    //public float movingSpeed = 1.5f;
    //public float stepHeight = 1.0f; // Height increase for each step
    //public float stepForwardDistance = 0.5f; // How much forward each step is placed

    //private List<GameObject> landedParatroopers = new List<GameObject>();

    //public bool stopSpawning = false;

    //void Update()
    //{
    //    CheckParatroopers();
    //}

    //public void RegisterParatrooper(GameObject paratrooper)
    //{
    //    landedParatroopers.Add(paratrooper);
    //}

    //private void CheckParatroopers()
    //{
    //    int leftHalfCount = 0;
    //    int rightHalfCount = 0;
    //    float screenWidth = Camera.main.orthographicSize * 2 * Camera.main.aspect;

    //    foreach (GameObject paratrooper in landedParatroopers)
    //    {
    //        if (paratrooper != null)
    //        {
    //            if (paratrooper.transform.position.x < 0)
    //            {
    //                leftHalfCount++;
    //            }
    //            else
    //            {
    //                rightHalfCount++;
    //            }
    //        }
    //    }

    //    if (leftHalfCount >= 4)
    //    {
    //        stopSpawning = true;
    //        StartCoroutine(MoveParatroopersAsStairs(true));
    //    }
    //    else if (rightHalfCount >= 4)
    //    {
    //        stopSpawning = true;
    //        StartCoroutine(MoveParatroopersAsStairs(false));
    //    }
    //}

    //private IEnumerator MoveParatroopersAsStairs(bool isLeftHalf)
    //{
    //    float currentHeight = obstacle.position.y; // Start climbing from the obstacle
    //    float currentXPosition = obstacle.position.x; // Start at obstacle's X

    //    foreach (GameObject paratrooper in landedParatroopers)
    //    {
    //        if (paratrooper != null)
    //        {
    //            if ((isLeftHalf && paratrooper.transform.position.x < 0) || (!isLeftHalf && paratrooper.transform.position.x >= 0))
    //            {
    //                yield return StartCoroutine(MoveParatrooperToStep(paratrooper, currentXPosition, currentHeight));
    //                currentHeight += stepHeight; // Increase height for next paratrooper
    //                currentXPosition += (isLeftHalf ? stepForwardDistance : -stepForwardDistance); // Move slightly forward
    //            }
    //        }
    //    }
    //}

    //private IEnumerator MoveParatrooperToStep(GameObject paratrooper, float targetX, float targetY)
    //{
    //    Vector3 targetPosition = new Vector3(targetX, targetY, paratrooper.transform.position.z);

    //    while (Vector3.Distance(paratrooper.transform.position, targetPosition) > 0.1f)
    //    {
    //        paratrooper.transform.position = Vector3.MoveTowards(paratrooper.transform.position, targetPosition, movingSpeed * Time.deltaTime);
    //        yield return null;
    //    }

    //    // Stop movement once in place
    //    paratrooper.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

    //    // If they reach the shooter's height, destroy the shooter
    //    if (targetY >= shooter.position.y - 0.5f)
    //    {
    //        Destroy(shooter.gameObject);
    //        Debug.Log("Shooter Destroyed!");
    //    }
    //}


    public Transform shooter;
    public Transform leftTargetPosition; // Left side target (empty GameObject)
    public Transform rightTargetPosition; // Right side target (empty GameObject)
    public float movingSpeed = 1.5f;
    public float climbingSpeed = 2.0f;
    //public float stepHeight = 1.0f; // Height increase per climb
    //public float stepSpacing = 0.5f; // Distance between paratroopers at ground level
    public bool stopSpawning = false;

    private float screenCenterX = 0f;
    private List<GameObject> landedParatroopers = new List<GameObject>();
    private bool moveLeftSide = false; // Whether the left side paratroopers should move

    void Update()
    {
        CheckParatroopers();

    }

    public void RegisterParatrooper(GameObject paratrooper)
    {
        landedParatroopers.Add(paratrooper);
    }

    private void CheckParatroopers()
    {
        // Calculate the count of paratroopers on the left and right side of the screen
        int leftCount = 0;
        int rightCount = 0;

        foreach (GameObject paratrooper in landedParatroopers)
        {
            if (paratrooper != null)
            {
                if (paratrooper.transform.position.x < screenCenterX)
                {
                    leftCount++;
                }
                else
                {
                    rightCount++;
                }
            }
        }

        // If one side has 4 or more paratroopers, start moving them toward the shooter
        if (leftCount >= 4 && !stopSpawning)
        {
            stopSpawning = true;
            moveLeftSide = true; // Left side will move
            StartCoroutine(WaitAndStartClimbing());
        }
        else if (rightCount >= 4 && !stopSpawning)
        {
            stopSpawning = true;
            moveLeftSide = false; // Right side will move
            StartCoroutine(WaitAndStartClimbing());
        }
    }

    private IEnumerator WaitAndStartClimbing()
    {
        // Wait for 3 seconds after landing
        yield return new WaitForSeconds(3f);

        // Check if there are enough paratroopers
        if (landedParatroopers.Count >= 4)
        {
            //GameObject[] firstFour = landedParatroopers.GetRange(0, 4).ToArray();

            GameObject[] leftSideParatroopers = landedParatroopers.FindAll(p => p.transform.position.x < screenCenterX).ToArray();
            GameObject[] rightSideParatroopers = landedParatroopers.FindAll(p => p.transform.position.x >= screenCenterX).ToArray();

            // Sort the paratroopers by distance to the shooter (only for the left or right side)
            System.Array.Sort(leftSideParatroopers, (a, b) =>
                Mathf.Abs(a.transform.position.x - shooter.position.x).CompareTo(Mathf.Abs(b.transform.position.x - shooter.position.x))
            );

            System.Array.Sort(rightSideParatroopers, (a, b) =>
                Mathf.Abs(a.transform.position.x - shooter.position.x).CompareTo(Mathf.Abs(b.transform.position.x - shooter.position.x))
            );

            GameObject[] firstFour = moveLeftSide
                                    ? leftSideParatroopers.Take(4).ToArray()
                                    : rightSideParatroopers.Take(4).ToArray();

            // Now move them toward the target
            //if (moveLeftSide)
            //{
            //    // First paratrooper (no climbing)
            //    yield return StartCoroutine(MoveToPosition(firstFour[0], leftTargetPosition.position));

            //    // Second paratrooper (move and climb)
            //    yield return StartCoroutine(MoveToPosition(firstFour[1], new Vector3(leftTargetPosition.position.x + stepSpacing, leftTargetPosition.position.y, 0)));
            //    yield return StartCoroutine(ClimbUp(firstFour[1], leftTargetPosition.position.y + stepHeight));

            //    // Third paratrooper (no climbing)
            //    yield return StartCoroutine(MoveToPosition(firstFour[2], new Vector3(leftTargetPosition.position.x - stepSpacing, leftTargetPosition.position.y, 0)));

            //    // Fourth paratrooper (move and climb)
            //    yield return StartCoroutine(MoveToPosition(firstFour[3], new Vector3(leftTargetPosition.position.x, leftTargetPosition.position.y, 0)));
            //    yield return StartCoroutine(ClimbUp(firstFour[3], leftTargetPosition.position.y + stepHeight));
            //}
            //else
            //{
            //    // Right side logic (similar structure as left side)

            //    // First paratrooper (no climbing)
            //    yield return StartCoroutine(MoveToPosition(firstFour[0], rightTargetPosition.position));

            //    // Second paratrooper (move and climb)
            //    yield return StartCoroutine(MoveToPosition(firstFour[1], new Vector3(rightTargetPosition.position.x - stepSpacing, rightTargetPosition.position.y, 0)));
            //    yield return StartCoroutine(ClimbUp(firstFour[1], rightTargetPosition.position.y + stepHeight));

            //    // Third paratrooper (no climbing)
            //    yield return StartCoroutine(MoveToPosition(firstFour[2], new Vector3(rightTargetPosition.position.x + stepSpacing, rightTargetPosition.position.y, 0)));

            //    // Fourth paratrooper (move and climb)
            //    yield return StartCoroutine(MoveToPosition(firstFour[3], new Vector3(rightTargetPosition.position.x, rightTargetPosition.position.y, 0)));
            //    yield return StartCoroutine(ClimbUp(firstFour[3], rightTargetPosition.position.y + stepHeight));
            //}

            //// Destroy the shooter when the fourth paratrooper reaches the top
            //Destroy(shooter.gameObject);
            //Debug.Log("Shooter Destroyed!");

            if (moveLeftSide)
            {
                yield return StartCoroutine(MoveParatroopers(firstFour, leftTargetPosition, true));
            }
            else
            {
                yield return StartCoroutine(MoveParatroopers(firstFour, rightTargetPosition, false));
            }
        }
    }

    private IEnumerator MoveParatroopers(GameObject[] paratroopers, Transform targetPosition, bool isLeftSide)
    {
        // Get the width and height of the first paratrooper (assuming all have the same size)
        SpriteRenderer spriteRenderer = paratroopers[0].GetComponent<SpriteRenderer>();
        float stepSpacing = spriteRenderer.bounds.size.x; // Use sprite width for spacing
        float stepHeight = spriteRenderer.bounds.size.y + 0.5f;  // Use sprite height for climbing

        // First paratrooper (no climbing)
        yield return StartCoroutine(MoveToPosition(paratroopers[0], targetPosition.position));

        // Second paratrooper (move, climb, and step forward)
        Vector3 secondTarget = new Vector3(targetPosition.position.x + (isLeftSide ? -stepSpacing : stepSpacing), targetPosition.position.y, 0);
        yield return StartCoroutine(MoveToPosition(paratroopers[1], secondTarget));
        yield return StartCoroutine(ClimbUp(paratroopers[1], targetPosition.position.y + stepHeight));
        yield return StartCoroutine(MoveToPosition(paratroopers[1], new Vector3(secondTarget.x, secondTarget.y + stepHeight, 0))); // Move slightly forward after climbing

        // Third paratrooper (no climbing)
        Vector3 thirdTarget = new Vector3(targetPosition.position.x - (isLeftSide ? stepSpacing : -stepSpacing), targetPosition.position.y, 0);
        yield return StartCoroutine(MoveToPosition(paratroopers[2], thirdTarget));

        // Fourth paratrooper (move, climb, and step forward)
        yield return StartCoroutine(MoveToPosition(paratroopers[3], targetPosition.position));
        yield return StartCoroutine(ClimbUp(paratroopers[3], targetPosition.position.y + stepHeight));
        yield return StartCoroutine(MoveToPosition(paratroopers[3], new Vector3(targetPosition.position.x, targetPosition.position.y + 0.1f, 0))); // Move slightly forward after climbing
    }


    private IEnumerator MoveToPosition(GameObject paratrooper, Vector3 targetPosition)
    {
        Rigidbody2D rb = paratrooper.GetComponent<Rigidbody2D>();
        if (rb != null) rb.isKinematic = true; // Disable physics

        while (Vector3.Distance(paratrooper.transform.position, targetPosition) > 0.1f)
        {
            paratrooper.transform.position = Vector3.MoveTowards(paratrooper.transform.position, targetPosition, movingSpeed * Time.deltaTime);
            yield return null;
        }

        if (rb != null) rb.isKinematic = false; // Re-enable physics
    }

    private IEnumerator ClimbUp(GameObject paratrooper, float targetY)
    {
        Rigidbody2D rb = paratrooper.GetComponent<Rigidbody2D>();
        if (rb != null) rb.isKinematic = true; // Disable physics

        Vector3 targetPosition = new Vector3(paratrooper.transform.position.x, targetY, 0);
        while (Vector3.Distance(paratrooper.transform.position, targetPosition) > 0.1f)
        {
            paratrooper.transform.position = Vector3.MoveTowards(paratrooper.transform.position, targetPosition, climbingSpeed * Time.deltaTime);
            yield return null;
        }

        if (rb != null) rb.isKinematic = false; // Re-enable physics
    }





    //// Public 
    //public float screenWidth = 16f; // Width of the screen in world units (adjust as necessary)
    //public float landingHeightThreshold = 2f; // Minimum height above the ground to consider as a valid landing
    //public GameObject shooter; // Reference to the shooter object
    //public List<GameObject> leftSideParatroopers = new List<GameObject>(); // List for left side paratroopers
    //public List<GameObject> rightSideParatroopers = new List<GameObject>(); // List for right side paratroopers

    //// Target positions for left and right side paratroopers
    //public Transform leftTargetPoint;
    //public Transform rightTargetPoint;

    //public Transform ground;  // Y position of the ground

    //public bool stopSpawning = false;


    //private int leftHalfParatroopers = 0;
    //private int rightHalfParatroopers = 0;

    //void Update()
    //{
    //    // Here you will need to call CheckLanding when a paratrooper lands
    //    // Example paratrooper landing detection (substitute with actual paratrooper landing logic)
    //    //Vector3 paratrooperPosition = new Vector3(Random.Range(-screenWidth / 2, screenWidth / 2), Random.Range(groundY + landingHeightThreshold, 10f), 0);
    //    //CheckParatrooperLanding(paratrooperPosition);
    //}

    //// This function is called when a paratrooper lands
    //public void CheckParatrooperLanding(GameObject paratrooperObject)
    //{
    //    Vector3 paratrooperPosition = paratrooperObject.transform.position;  

    //    // Check if the paratrooper landed on the ground at or above the specified height
    //    if (paratrooperPosition.y >= ground.position.y + landingHeightThreshold)
    //    {
    //        // Determine which half of the screen the paratrooper landed on
    //        if (paratrooperPosition.x < 0)
    //        {
    //            leftHalfParatroopers++;
    //            leftSideParatroopers.Add(paratrooperObject); // Add to left side list
    //        }
    //        else
    //        {
    //            rightHalfParatroopers++;
    //            rightSideParatroopers.Add(paratrooperObject); // Add to right side list
    //        }

    //        // Log the current counts
    //        Debug.Log("Left Half: " + leftHalfParatroopers + " | Right Half: " + rightHalfParatroopers);

    //        // Check if any half has received 4 paratroopers
    //        if (leftHalfParatroopers >= 4)
    //        {
    //            Debug.Log("Left half got 4 paratroopers first!");
    //            StoreParatroopersBasedOnDistance(leftSideParatroopers, true);
    //        }
    //        else if (rightHalfParatroopers >= 4)
    //        {
    //            Debug.Log("Right half got 4 paratroopers first!");
    //            StoreParatroopersBasedOnDistance(rightSideParatroopers, false);
    //        }
    //    }
    //}

    //// Sort and store paratroopers based on minimum distance from the shooter
    ////void StoreParatroopersBasedOnDistance(List<GameObject> paratrooperList)
    ////{
    ////    // Sort paratroopers by distance to shooter
    ////    paratrooperList.Sort((paratrooper1, paratrooper2) =>
    ////    {
    ////        float distance1 = Vector3.Distance(shooter.transform.position, paratrooper1.transform.position);
    ////        float distance2 = Vector3.Distance(shooter.transform.position, paratrooper2.transform.position);
    ////        return distance1.CompareTo(distance2); // Sort by smallest distance
    ////    });

    ////    // After sorting, the paratroopers in paratrooperList are now ordered by their distance from the shooter
    ////    Debug.Log("Paratroopers sorted by distance from shooter");
    ////    foreach (var paratrooper in paratrooperList)
    ////    {
    ////        Debug.Log(paratrooper.name + " at distance: " + Vector3.Distance(shooter.transform.position, paratrooper.transform.position));
    ////    }
    ////}

    //// Sort and move paratroopers based on minimum distance from the shooter
    //void StoreParatroopersBasedOnDistance(List<GameObject> paratrooperList, bool isLeftSide)
    //{
    //    // Sort paratroopers by distance to shooter
    //    paratrooperList.Sort((paratrooper1, paratrooper2) =>
    //    {
    //        float distance1 = Vector3.Distance(shooter.transform.position, paratrooper1.transform.position);
    //        float distance2 = Vector3.Distance(shooter.transform.position, paratrooper2.transform.position);
    //        return distance1.CompareTo(distance2); // Sort by smallest distance
    //    });

    //    Debug.Log("Paratroopers sorted by distance from shooter");
    //    foreach (var paratrooper in paratrooperList)
    //    {
    //        Debug.Log(paratrooper.name + " at distance: " + Vector3.Distance(shooter.transform.position, paratrooper.transform.position));
    //    }

    //    // Start moving them one by one towards their respective target points
    //    StartCoroutine(MoveParatroopersSequentially(paratrooperList, isLeftSide));
    //}

    //// Coroutine to move each paratrooper one by one towards its respective target point
    //IEnumerator MoveParatroopersSequentially(List<GameObject> paratrooperList, bool isLeftSide)
    //{
    //    Vector3 targetPoint = isLeftSide ? leftTargetPoint.position : rightTargetPoint.position; // Choose target point

    //    foreach (var paratrooper in paratrooperList)
    //    {
    //        yield return StartCoroutine(MoveParatrooper(paratrooper, targetPoint));
    //    }
    //}

    //// Coroutine to move a single paratrooper towards the target position smoothly
    //IEnumerator MoveParatrooper(GameObject paratrooper, Vector3 targetPos)
    //{
    //    float speed = 5f; // Adjust speed as needed
    //    float threshold = 0.1f; // Stop when close enough to target

    //    while (Vector3.Distance(paratrooper.transform.position, targetPos) > threshold)
    //    {
    //        paratrooper.transform.position = Vector3.MoveTowards(paratrooper.transform.position, targetPos, speed * Time.deltaTime);
    //        yield return null;
    //    }

    //    Debug.Log(paratrooper.name + " reached the target position!");
    //}



}
