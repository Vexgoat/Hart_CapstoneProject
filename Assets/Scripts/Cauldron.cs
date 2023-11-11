using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Cauldron : MonoBehaviour
{
    public float interactionRadius = 1.0f;
    public GameObject prefabToInstantiate;
    public Transform interactionPoint; // Set this to the specific spot on the cauldron where you want to check for objects

    private void Update()
    {
        // Check if the player is not holding down right click
        if (!Input.GetMouseButton(1))
        {
            // Check if the cauldron has the specific tag
            if (gameObject.CompareTag("Cauldron"))
            {
                // Get all colliders within the interaction radius of the cauldron at the specific interaction point
                Collider2D[] hitColliders = Physics2D.OverlapCircleAll(interactionPoint.position, interactionRadius);

                // Iterate through all the colliders found
                foreach (Collider2D collider in hitColliders)
                {
                    // Check if the collider has the "grab" tag
                    if (collider.CompareTag("grab"))
                    {
                        // Destroy the eye object
                        Destroy(collider.gameObject);

                        // Instantiate the prefab
                        Instantiate(prefabToInstantiate, interactionPoint.position, Quaternion.identity);

                        // Break out of the loop if you only want to interact with one object at a time
                        break;
                    }
                }
            }
        }
    }
}
