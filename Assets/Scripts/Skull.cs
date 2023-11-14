using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class Skull : MonoBehaviourPunCallbacks
{

    public GameObject player1;
    public GameObject player2;
    PhotonView view;

    private void Start()
    {
        view = GetComponent<PhotonView>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Call the PunRPC method to load the scene across all clients
            view.RPC("LoadNextScene", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    private void LoadNextScene()
    {
        // Load the next scene
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        PhotonNetwork.LoadLevel(currentSceneIndex + 1);
    }
}

