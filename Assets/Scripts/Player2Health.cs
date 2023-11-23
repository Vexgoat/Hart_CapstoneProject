using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;

public class Player2Health : MonoBehaviourPunCallbacks
{
    public int maxHealth = 100;
    public int currentHealth;
    public Slider healthSlider;

    PhotonView view;

    private void Start()
    {
        view = GetComponent<PhotonView>();
        currentHealth = maxHealth;
        UpdateHealthSlider();
    }

    public void TakeDamage(int damage)
    {
        if (view.IsMine)
        {
            currentHealth -= damage;
            Debug.Log("Player2 Health " + currentHealth);

            UpdateHealthSlider();

            if (currentHealth <= 0)
            {
                view.RPC("Die", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    private void Die()
    {
        PhotonNetwork.LoadLevel("YouLose");
        Debug.Log("Player2 is Now dead");
    }

    private void UpdateHealthSlider()
    {
        if (healthSlider != null)
        {
            // Normalize the current health value to set the slider value
            healthSlider.value = (float)currentHealth / maxHealth;
        }
    }
}
