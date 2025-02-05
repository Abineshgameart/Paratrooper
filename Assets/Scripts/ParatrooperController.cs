using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParatrooperController : MonoBehaviour
{
    //public float parachuteFallSpeed = 2f;
    //public float parachuteOpenDelay = 0.2f;

    //private Rigidbody2D rb;
    //private bool parachuteOpened = false;

    //void Start()
    //{
    //    rb = GetComponent<Rigidbody2D>();
    //    StartCoroutine(OpenParachuteAfterDelay());
    //}

    //IEnumerator OpenParachuteAfterDelay()
    //{
    //    yield return new WaitForSeconds(parachuteOpenDelay);
    //    parachuteOpened = true;
    //}

    //void Update()
    //{
    //    if (parachuteOpened)
    //    {
    //        rb.velocity = new Vector2(rb.velocity.x, -parachuteFallSpeed);
    //    }

    //}


    public float parachuteFallSpeed = 2f;
    public float parachuteOpenDelay = 0.2f;
    private Rigidbody2D rb;
    private bool parachuteOpened = false;
    private bool hasLanded = false;
    private ParatrooperManager paratrooperManager;
    public Transform groundTransform; // Reference to the Ground Transform
    public float groundThreshold = 2f; // Threshold distance from the ground to consider as landing

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        paratrooperManager = FindObjectOfType<ParatrooperManager>();
        groundTransform = GameObject.FindWithTag("Ground").transform;
        StartCoroutine(OpenParachuteAfterDelay());
    }

    IEnumerator OpenParachuteAfterDelay()
    {
        yield return new WaitForSeconds(parachuteOpenDelay);
        parachuteOpened = true;
    }

    void Update()
    {
        if (parachuteOpened && !hasLanded)
        {
            rb.velocity = new Vector2(rb.velocity.x, -parachuteFallSpeed);

            // Check if the paratrooper is within the threshold distance from the ground
            if (transform.position.y - groundTransform.position.y <= groundThreshold)
            {
                LandParatrooper();
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            LandParatrooper();
        }
    }

    void LandParatrooper()
    {
        if (!hasLanded)
        {
            hasLanded = true;
            rb.velocity = Vector2.zero;
            if (paratrooperManager != null)
            {
                paratrooperManager.RegisterParatrooper(gameObject);

                //paratrooperManager.CheckParatrooperLanding(gameObject);
            }
        }
    }


}
