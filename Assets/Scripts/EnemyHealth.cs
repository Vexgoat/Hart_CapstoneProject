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

    //This method sets the slider value to the current health of the enemy  
    private void Start()
    {
        view = GetComponent<PhotonView>();
        currentHealth = maxHealth;

        healthSlider = GameObject.Find("EnemyHealthBar2").GetComponent<Slider>();
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }
    //This method checks if the view is not the local player and if connected to the network if it is it returns preventing anything further from a non local player
    //This method also calls two other methods to take damage and update the health slider
    public void TakeDamage(int damage)
    {
        if (!view.IsMine && PhotonNetwork.IsConnected) return;

        view.RPC("ApplyDamage", RpcTarget.All, damage);
        view.RPC("UpdateHealthSlider", RpcTarget.All);
    }

    //This method allows the enemy to take damage
    [PunRPC]
    private void ApplyDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Enemy Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            view.RPC("Die", RpcTarget.All);
        }
    }

    //This method updates the healthbar for the enemy
    [PunRPC]
    private void UpdateHealthSlider()
    {
        healthSlider.value = currentHealth;
    }

    //This method is used to call another method smh
    [PunRPC]
    public void Die()
    {
        view.RPC("SpawnKey", RpcTarget.All);
    }

    //This method is used to spawn the key! it also transfers the ownership to the local player so that player2 can pick it up. Then it destroys the enemy and disables the collider.
    [PunRPC]
    public void SpawnKey()
    {
        Vector2 spawnPosition = (Vector2)transform.position + keySpawn;
        Chest.key = PhotonNetwork.Instantiate("Key", spawnPosition, Quaternion.identity);
        
        PhotonView keyView = Chest.key.GetComponent<PhotonView>();
        keyView.TransferOwnership(view.Owner);

        if (!view.IsMine)
        {
            view.TransferOwnership(PhotonNetwork.LocalPlayer);
        }

        if (view.IsMine || PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(gameObject);
            GetComponent<Collider2D>().enabled = false;
        }
    }

}
