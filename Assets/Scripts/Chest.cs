using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class Chest : MonoBehaviourPunCallbacks
{
    public static GameObject key;
    public GameObject skullPrefab;
    public Vector2 skullSpawn = new Vector2(1f, 1f);
    public Sprite closedSprite;
    public Sprite openedSprite;
    public float proximityRadius = 2.0f; // Adjust this radius to set the proximity range.

    private SpriteRenderer spriteRendy;
    private bool isOpen = false;

    PhotonView view;

    private void Start()
    {
        view = GetComponent<PhotonView>();
        spriteRendy = GetComponent<SpriteRenderer>();
        //key = new GameObject("DummyKey");
    }

    private void Update()
    {
        if (view.IsMine)
        {
            

            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, proximityRadius);
            foreach (var collider in colliders)
            {
                if (collider.CompareTag("Player"))
                {
                    Debug.Log("Player in proximity");
                    if (Input.GetKeyDown(KeyCode.E) && !isOpen && key == null)
                    {
                        Debug.Log("Interacting with chest");

                        // Play the open animation
                        view.RPC("OpenChest", RpcTarget.All);

                        // Instantiate the skull at the spawn point
                        Vector2 spawnPosition = (Vector2)transform.position + skullSpawn;
                        PhotonNetwork.Instantiate("Skull", spawnPosition, Quaternion.identity);

                        isOpen = true; // Ensure this only happens once
                    }
                }
            }
        }
    }

    [PunRPC]
    private void OpenChest()
    {
        spriteRendy.sprite = openedSprite;
    }
}