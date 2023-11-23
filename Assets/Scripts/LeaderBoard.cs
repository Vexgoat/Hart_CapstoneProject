using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;
using Photon.Pun;

public class LeaderBoard : MonoBehaviourPunCallbacks
{
    public InputField teamNameInput;
    public Button submitButton;
    public Text timerText;

    private float completionTime;
    private bool dataSubmitted = false;
    private StopWatch stopWatch; // Reference to the StopWatch script

    public string serverURL = "http://vexgoat.com/Boozzle/boozzleConnect.php";

    PhotonView view;

    private void Start()
    {
        submitButton.onClick.AddListener(SubmitData);
        stopWatch = FindObjectOfType<StopWatch>(); // Assuming StopWatch is attached to the same GameObject

        view = GetComponent<PhotonView>();

        // Only enable UI elements for the master client (player1)
        if (PhotonNetwork.IsMasterClient)
        {
            teamNameInput.gameObject.SetActive(true);
            submitButton.gameObject.SetActive(true);
        }
        else
        {
            teamNameInput.gameObject.SetActive(false);
            submitButton.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        // Check if the current scene is the "YouWin" scene
        if (view != null && view.IsMine && dataSubmitted && SceneManager.GetActiveScene().name == "YouWin")
        {
            // Get the data from the master client and send it to the server
            StartCoroutine(SendData(teamNameInput.text, completionTime));

            // Reset the dataSubmitted flag to ensure this block is executed only once
            dataSubmitted = false;
        }
    }

    public void SubmitData()
    {
        if (view.IsMine && stopWatch != null && !dataSubmitted)
        {
            // Get the current scene name
            string currentScene = SceneManager.GetActiveScene().name;

            // Check if the current scene is the "YouWin" scene
            if (currentScene == "YouWin")
            {
                string teamName = teamNameInput.text;

                // Get completion time from the timer
                completionTime = stopWatch.timeSoFar;

                // Send data to the server
                StartCoroutine(SendData(teamName, completionTime));

                // Disable UI elements after submission only for the master client
                if (PhotonNetwork.IsMasterClient)
                {
                    teamNameInput.gameObject.SetActive(false);
                    submitButton.gameObject.SetActive(false);
                }

                // Set flag to indicate data has been submitted
                dataSubmitted = true;
            }
        }
    }

    IEnumerator SendData(string teamName, float completionTime)
    {
        WWWForm form = new WWWForm();
        form.AddField("teamName", teamName);
        form.AddField("completionTime", completionTime.ToString());

        using (UnityWebRequest www = UnityWebRequest.Post(serverURL, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                Debug.Log("Data sent successfully");
            }
        }
    }
}
