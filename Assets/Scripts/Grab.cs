using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Grab : MonoBehaviourPunCallbacks
{
    public Transform grabPoint;
    public float grabRadius; 
    public LayerMask grabLayer; 

    private GameObject eyeObject;
    private Collider2D[] hitColliders; 

    PhotonView view;

    private void Start()
    {
        view = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (view.IsMine)
        {
            // Check for nearby objects in a circular area
            hitColliders = Physics2D.OverlapCircleAll(grabPoint.position, grabRadius, grabLayer);

            // Iterate through the colliders to find a grabbable object
            foreach (Collider2D collider in hitColliders)
            {
                if (eyeObject == null && Input.GetMouseButtonDown(1))
                {
                    Debug.Log("IS Working");
                    eyeObject = collider.gameObject;
                    eyeObject.GetComponent<Rigidbody2D>().isKinematic = true;
                    eyeObject.GetComponent<BoxCollider2D>().isTrigger = true;
                    eyeObject.transform.position = grabPoint.position;
                    eyeObject.transform.SetParent(transform);
                    break; // Exit the loop after grabbing the first object
                    
                }
            }

            // Release the grabbed object on right-click
            if (Input.GetMouseButtonUp(1) && eyeObject != null)
            {
                eyeObject.GetComponent<Rigidbody2D>().isKinematic = false;
                eyeObject.GetComponent<BoxCollider2D>().isTrigger = false;
                eyeObject.transform.SetParent(null);
                eyeObject = null;
            }
        }

        DebugDrawGrabArea();
    }

    void DebugDrawGrabArea()
    {
        // Debug draw the grab area
        Debug.DrawLine(grabPoint.position, grabPoint.position + Vector3.right * grabRadius, Color.red);
    }
}  