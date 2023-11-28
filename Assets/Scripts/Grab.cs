using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Grab : MonoBehaviourPunCallbacks
{
    public Transform grabPoint;
    public float grabRadius; 
    public LayerMask grabLayer; 
    public AudioClip grabSound;

    private GameObject eyeObject;
    private Collider2D[] hitColliders; 

    PhotonView view;

    private void Start()
    {
        view = GetComponent<PhotonView>();
    }

    //This method check if the view is yours, then checks if your holding the eye object and if u are
    //It sets everything to true and gets the position
    //Further down the line it will check if your holding the eyeobject and do the opposite and will not get the position
    private void Update()
    {
        if (view.IsMine)
        {
            hitColliders = Physics2D.OverlapCircleAll(grabPoint.position, grabRadius, grabLayer);

            foreach (Collider2D collider in hitColliders)
            {
                if (eyeObject == null && Input.GetMouseButtonDown(1))
                {
                    eyeObject = collider.gameObject;
                    eyeObject.GetComponent<Rigidbody2D>().isKinematic = true;
                    eyeObject.GetComponent<BoxCollider2D>().isTrigger = true;
                    eyeObject.transform.position = grabPoint.position;
                    eyeObject.transform.SetParent(transform);

                    view.RPC("GrabSound", RpcTarget.All);

                    break;
                }
            }

            if (Input.GetMouseButtonUp(1) && eyeObject != null)
            {
                eyeObject.GetComponent<Rigidbody2D>().isKinematic = false;
                eyeObject.GetComponent<BoxCollider2D>().isTrigger = false;
                eyeObject.transform.SetParent(null);
                eyeObject = null;
            }
        }
    }
    //This just plays the grab sound
    [PunRPC]
    private void GrabSound(){
    if(view.IsMine){
    
        AudioSource.PlayClipAtPoint(grabSound, eyeObject.transform.position);
    
        }
    }
}
