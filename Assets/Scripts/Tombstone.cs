using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Tombstone : MonoBehaviour
{
    public int maxHealth = 1;
    private int currentHealth;
    public Sprite intactSprite; 
    public Sprite destroyedSprite; 
    private SpriteRenderer spriteRendy;

    private void Start()
    {
        currentHealth = maxHealth;
        //spriteRendy.sprite = intactSprite;
        //spriteRendy = GetComponent<SpriteRenderer();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
{
    // Change the sprite to the destroyed one
    spriteRendy.sprite = destroyedSprite;
    GetComponent<Collider2D>().enabled = false;
}

}
