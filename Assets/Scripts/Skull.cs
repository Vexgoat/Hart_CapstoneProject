using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class Skull : MonoBehaviourPunCallbacks
{
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
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        // Load the next scene
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}
