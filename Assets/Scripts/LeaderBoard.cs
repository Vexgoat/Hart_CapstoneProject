using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Photon.Pun;
using Photon.Realtime;

public class LeaderBoard : MonoBehaviourPunCallbacks
{
    public InputField teamNameInput;
    public Button submitButton;
    public Button quitButton; 
    public string serverURL = "http://vexgoat.com/Boozzle/submitData.php";

    private string teamName;
    private bool dataSubmitted = false;
    private float completionTime;

    PhotonView view;

    //This method checks if the cilent is master(player1), then will only show player1 the teamName and submit button
    //When the submit button is pressed the submitdata method is called
    private void Start()
    {
        view = GetComponent<PhotonView>();
        bool isMasterClient = PhotonNetwork.IsMasterClient;

        if (teamNameInput != null && submitButton != null)
        {
            teamNameInput.gameObject.SetActive(isMasterClient);
            submitButton.gameObject.SetActive(isMasterClient);
            submitButton.interactable = isMasterClient;

            if (isMasterClient)
            {
                submitButton.onClick.AddListener(SubmitData);
            }
        }

        if (quitButton != null)
        {
            quitButton.onClick.AddListener(QuitGame);
        }
    }

    //This method checks if the team name has been submitted, if it has it collects that data, hides the UI and starts the senddata coroutine 
    public void SubmitData()
    {
        if (!dataSubmitted)
        {
            teamName = teamNameInput.text;

            StopWatch stopWatch = FindObjectOfType<StopWatch>();

            if (stopWatch != null)
            {
                completionTime = stopWatch.timeSoFar;
                stopWatch.StopTimer();
            }

            teamNameInput.gameObject.SetActive(false);

            if (submitButton != null)
            {
                submitButton.gameObject.SetActive(false);
                submitButton.interactable = false;
            }

            StartCoroutine(SendData(teamName, completionTime));

            dataSubmitted = true;
        }
    }

    //This will quit the game :O
    public void QuitGame()
    {
        Application.Quit();
    }

    //This method creates a list containing the team name and the completion time
    //Then it send a POST request to the URl above
    //After that the completion time is converted to a string with 2 decimal places
    //Then waits for it to be completed
    //yay
    IEnumerator SendData(string teamName, float completionTime)
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("teamName", teamName));
        formData.Add(new MultipartFormDataSection("completionTime", completionTime.ToString("F2")));

        using (UnityWebRequest www = UnityWebRequest.Post(serverURL, formData))
        {
            yield return www.SendWebRequest();
        }
    }
}
