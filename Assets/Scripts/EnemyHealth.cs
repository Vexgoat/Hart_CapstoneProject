using System.Collections;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviourPunCallbacks
{
    public int maxHealth = 3;
    public GameObject keyPrefab;
    public Vector2 keySpawn = new Vector2(1f, 1f);
    public int currentHealth;
    public Slider healthSlider;

    PhotonView view;

    private void Start()
    {
        view = GetComponent<PhotonView>();
        currentHealth = maxHealth;

        healthSlider = GameObject.Find("EnemyHealthbBar").GetComponent<Slider>();

    }

    public void TakeDamage(int damage)
    {
        if (!view.IsMine) return; // Only process damage on the owner

        currentHealth -= damage;
        Debug.Log("Enemy Health: " + currentHealth);

        UpdateHealthSlider();

        if (currentHealth <= 0)
        {
            view.RPC("Die", RpcTarget.All);
        }
    }

    private void UpdateHealthSlider()
    {
        if (healthSlider != null)
        {
            // Normalize the current health value to set the slider value
            healthSlider.value = (float)currentHealth / maxHealth;
        }
    }

    [PunRPC]
    public void Die()
    {
        Debug.Log("Enemy Died");
        view.RPC("SpawnKey", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void SpawnKey()
    {
        // Check if the current client owns the PhotonView
        if (view.IsMine)
        {
            Vector2 spawnPosition = (Vector2)transform.position + keySpawn;
            Chest.key = PhotonNetwork.Instantiate("Key", spawnPosition, Quaternion.identity);
            PhotonNetwork.Destroy(gameObject);
            GetComponent<Collider2D>().enabled = false;
        }
    }

}
