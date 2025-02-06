using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCollisionHandler : MonoBehaviour
{
    public ParatrooperManager paratrooperManager; // Reference to the ParatrooperLandingCalculation script

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision object is a paratrooper (using tag or by checking if it's the correct type)
        if (collision.gameObject.CompareTag("Paratrooper"))
        {
            // Get the position of the paratrooper
            Vector3 paratrooperPosition = collision.transform.position;

            // Send the paratrooper's position to CheckParatrooperLanding
            //paratrooperManager.CheckParatrooperLanding(paratrooperPosition);
        }
    }
}
