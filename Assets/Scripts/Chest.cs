using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Chest : MonoBehaviourPunCallbacks
{
    public static GameObject key;
    public GameObject skullPrefab;
    public Vector2 skullSpawn = new Vector2(1f, 1f);
    public Sprite closedSprite;
    public Sprite openedSprite;
    public float proxRadius = 2.0f;

    private SpriteRenderer spriteRendy;
    private bool isOpen = false;

    PhotonView view;

    private void Start()
    {
        view = GetComponent<PhotonView>();
        spriteRendy = GetComponent<SpriteRenderer>();
        key = new GameObject("DummyKey");
    }

    //This method check who is viewing, compares the tag player, then checks if the key is gone and if the player pressed e.
    //Afterwards it will switch the sprites across all cilents and also instantiate the skull object across all cilents.
    private void Update()
    {
        if (view.IsMine)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, proxRadius);
            foreach (var collider in colliders)
            {
                if (collider.CompareTag("Player"))
                {
                    if (Input.GetKeyDown(KeyCode.E) && !isOpen && key == null)
                    {
                        view.RPC("OpenChest", RpcTarget.All);
                        
                        Vector2 spawnPosition = (Vector2)transform.position + skullSpawn;
                        PhotonNetwork.Instantiate("Skull", spawnPosition, Quaternion.identity);

                        isOpen = true;
                    }
                }
            }
        }
    }

    //This is the method that gets called in the update method to switch the sprites
    [PunRPC]
    private void OpenChest()
    {
        spriteRendy.sprite = openedSprite;
    }
}