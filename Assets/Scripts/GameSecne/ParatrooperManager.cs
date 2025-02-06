using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ParatrooperManager : MonoBehaviour
{
    // Public 
    public float waitingTimeToStartClimbing = 5f;

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
        yield return new WaitForSeconds(waitingTimeToStartClimbing);

        // Check if there are enough paratroopers
        if (landedParatroopers.Count >= 4)
        {

            GameObject[] leftSideParatroopers = landedParatroopers.FindAll(p => p.transform.position.x < screenCenterX).ToArray();
            GameObject[] rightSideParatroopers = landedParatroopers.FindAll(p => p.transform.position.x >= screenCenterX).ToArray();

            System.Array.Sort(leftSideParatroopers, (a, b) =>
                Vector2.Distance(a.transform.position, shooter.position)
                .CompareTo(Vector2.Distance(b.transform.position, shooter.position))
            );

            System.Array.Sort(rightSideParatroopers, (a, b) =>
                Vector2.Distance(a.transform.position, shooter.position)
                .CompareTo(Vector2.Distance(b.transform.position, shooter.position))
            );  


            GameObject[] firstFour = moveLeftSide
                                    ? leftSideParatroopers.Take(4).ToArray()
                                    : rightSideParatroopers.Take(4).ToArray();


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
        float stepHeight = spriteRenderer.bounds.size.y + 0.2f;  // Use sprite height for climbing

        // First paratrooper (no climbing)
        yield return StartCoroutine(MoveToPosition(paratroopers[0], targetPosition.position));

        // Second paratrooper (move, climb, and step forward)
        Vector3 secondTarget = new Vector3(targetPosition.position.x + (isLeftSide ? -stepSpacing : stepSpacing), targetPosition.position.y, 0);
        yield return StartCoroutine(MoveToPosition(paratroopers[1], secondTarget));
        yield return StartCoroutine(ClimbUp(paratroopers[1], targetPosition.position.y + stepHeight));
        yield return StartCoroutine(MoveToPosition(paratroopers[1], new Vector3(secondTarget.x + (isLeftSide ? stepSpacing : -stepSpacing), secondTarget.y + stepHeight, 0))); // Move slightly forward after climbing

        // Third paratrooper (no climbing)
        Vector3 thirdTarget = new Vector3(targetPosition.position.x - (isLeftSide ? stepSpacing : -stepSpacing), targetPosition.position.y, 0);
        yield return StartCoroutine(MoveToPosition(paratroopers[2], thirdTarget));

        // Fourth paratrooper (move, climb three times)
        // Move to initial fourth position
        Vector3 fourthTarget = new Vector3(targetPosition.position.x + (isLeftSide ? -stepSpacing * 2 : stepSpacing * 2), targetPosition.position.y, 0);
        yield return StartCoroutine(MoveToPosition(paratroopers[3], fourthTarget));

        // First climb
        Vector3 climb1Pos = new Vector3(fourthTarget.x + (isLeftSide ? stepSpacing : -stepSpacing), fourthTarget.y + stepHeight, 0);
        yield return StartCoroutine(ClimbUp(paratroopers[3], climb1Pos.y));
        yield return StartCoroutine(MoveToPosition(paratroopers[3], new Vector3(climb1Pos.x + (isLeftSide ? 0.3f : -0.3f), climb1Pos.y, 0)));

        // Second climb
        Vector3 climb2Pos = new Vector3(climb1Pos.x + (isLeftSide ? stepSpacing : -stepSpacing), climb1Pos.y + stepHeight, 0);
        yield return StartCoroutine(ClimbUp(paratroopers[3], climb2Pos.y - 0.5f));
        yield return StartCoroutine(MoveToPosition(paratroopers[3], climb2Pos));

        // Third climb
        Vector3 climb3Pos = new Vector3(climb2Pos.x + (isLeftSide ? stepSpacing : -stepSpacing), climb2Pos.y, 0);
        yield return StartCoroutine(ClimbUp(paratroopers[3], climb3Pos.y));
        yield return StartCoroutine(MoveToPosition(paratroopers[3], climb3Pos));

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



}
