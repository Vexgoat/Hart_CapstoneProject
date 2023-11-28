using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    //This method allows the player2 to take damage.
    //It also calls the method that will update the healthbar
    public void TakeDamage(int damage)
    {
        if (view.IsMine)
        {
            currentHealth -= damage;

            UpdateHealthSlider();
            view.RPC("UpdateHealthSliderRPC", RpcTarget.Others, currentHealth);

            if (currentHealth <= 0)
            {
                view.RPC("Die", RpcTarget.All);
            }
        }
    }

    //Self explantory but, when player2 dies losing scene is loaded on the network
    [PunRPC]
    private void Die()
    {
        PhotonNetwork.LoadLevel("YouLose");
    }

    //This method updates the healthbar on all screens
    [PunRPC]
    private void UpdateHealthSliderRPC(int newHealth)
    {
        currentHealth = newHealth;
        UpdateHealthSlider();
    }

    //:0
    private void UpdateHealthSlider()
    {
        if (healthSlider != null)
        {
            healthSlider.value = (float)currentHealth / maxHealth;
        }
    }
}
