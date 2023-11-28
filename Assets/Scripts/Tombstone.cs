using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Tombstone : MonoBehaviourPunCallbacks
{
    public int maxHealth = 1;
    public Sprite intactSprite; 
    public Sprite destroyedSprite;
    public GameObject keyPrefab;
    public Vector2 keySpawn = new Vector2(1f, 1f);

    private int currentHealth;
    private SpriteRenderer spriteRendy;

    PhotonView view;

    private void Start()
    {
        view = GetComponent<PhotonView>();
        currentHealth = maxHealth;
        spriteRendy = GetComponent<SpriteRenderer>();
    }

    //This method will allow the tombstone to take damage and call the die method
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    //This method call the change sprite method
    public void Die()
    {
        spriteRendy.sprite = destroyedSprite;
        view.RPC("ChangeSprite", RpcTarget.All);
    }

    //This method will change the sprite to the destroyed one and spawn a key.
    //It will then disable the collider to prevent anymore keys from being spawned in.
     [PunRPC]
        public void ChangeSprite()
        {
            spriteRendy.sprite = destroyedSprite;

            Vector2 spawnPosition = (Vector2)transform.position + keySpawn;
            Chest.key = PhotonNetwork.Instantiate("Key", spawnPosition, Quaternion.identity);

            GetComponent<Collider2D>().enabled = false;
        }


}