using System.Collections;
using UnityEngine;
using Photon.Pun;

public class Player2Health : MonoBehaviourPunCallbacks
{
    public int maxHealth = 100;
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (photonView.IsMine)
        {
            currentHealth -= damage;
            Debug.Log("Player2 Health " + currentHealth);

            if (currentHealth <= 0)
            {
                photonView.RPC("Die", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    private void Die()
    {
        PhotonNetwork.LoadLevel("YouLose");
        Debug.Log("Player2 is Now dead");
    }
}
