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

    private void Start()
    {
        currentHealth = maxHealth;
        spriteRendy = GetComponent<SpriteRenderer>();

        // Output the initial health to the console
        Debug.Log("Tombstone Health: " + currentHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // Output the current health after taking damage
        Debug.Log("Tombstone Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Change the sprite to the destroyed one
        Debug.Log("Die method called");
        spriteRendy.sprite = destroyedSprite;
        photonView.RPC("ChangeSprite", RpcTarget.All);
    }

     [PunRPC]
    void ChangeSprite()
    {
        spriteRendy.sprite = destroyedSprite;

        Vector2 spawnPosition = (Vector2)transform.position + keySpawn;
        Instantiate(keyPrefab, spawnPosition, Quaternion.identity);

        GetComponent<Collider2D>().enabled = true;
    }

}
