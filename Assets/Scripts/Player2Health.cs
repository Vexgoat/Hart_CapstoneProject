using System.Collections;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

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
