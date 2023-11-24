using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;

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

        healthSlider = GameObject.Find("EnemyHealthBar").GetComponent<Slider>();
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    public void TakeDamage(int damage)
    {
        if (!view.IsMine && PhotonNetwork.IsConnected) return;

        view.RPC("ApplyDamage", RpcTarget.All, damage);
    }

    [PunRPC]
    private void ApplyDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Enemy Health: " + currentHealth);

        view.RPC("UpdateHealthSlider", RpcTarget.All);

        if (currentHealth <= 0)
        {
            view.RPC("Die", RpcTarget.All);
        }
    }

    [PunRPC]
    private void UpdateHealthSlider()
    {
        if (healthSlider != null)
        {
            // Normalize the current health value to set the slider value
            healthSlider.value = currentHealth;
        }
    }

    [PunRPC]
    public void Die()
    {
        Debug.Log("Enemy Died");
        view.RPC("SpawnKey", RpcTarget.All);
    }

    [PunRPC]
    public void SpawnKey()
    {
        // Instantiate the key for all players
        Vector2 spawnPosition = (Vector2)transform.position + keySpawn;
        Chest.key = PhotonNetwork.Instantiate("Key", spawnPosition, Quaternion.identity);

        // Ensure the ownership of the spawned key
        PhotonView keyView = Chest.key.GetComponent<PhotonView>();
        keyView.TransferOwnership(view.Owner);

        // Check if the current client is the owner, if not, try to transfer ownership
        if (!view.IsMine)
        {
            view.TransferOwnership(PhotonNetwork.LocalPlayer);
        }

        // Only destroy the enemy if the current client is the owner or the MasterClient
        if (view.IsMine || PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(gameObject);
            GetComponent<Collider2D>().enabled = false;
        }
    }

}
