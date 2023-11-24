using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Photon.Pun;
using Photon.Realtime;

public class LeaderBoard : MonoBehaviour
{
    public InputField teamNameInput;
    public Button submitButton;
    public Button quitButton; // Assuming you have a quit button
    public string serverURL = "http://vexgoat.com/Boozzle/submitData.php";
    private string teamName;
    private bool dataSubmitted = false;
    private float completionTime;

    PhotonView view;

    private void Start()
    {
        view = GetComponent<PhotonView>();
        bool isMasterClient = PhotonNetwork.IsMasterClient;

        teamNameInput.gameObject.SetActive(isMasterClient);

        if (submitButton != null)
        {
            submitButton.gameObject.SetActive(isMasterClient);
            submitButton.interactable = isMasterClient;
            submitButton.onClick.AddListener(SubmitData);
        }
        else
        {
            Debug.LogError("submitButton is null.");
        }

        if (quitButton != null)
        {
            quitButton.onClick.AddListener(QuitGame);
        }
        else
        {
            Debug.LogError("quitButton is null.");
        }
    }

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
            else
            {
                Debug.LogWarning("StopWatch component not found.");
            }

            teamNameInput.gameObject.SetActive(false);

            if (submitButton != null)
            {
                submitButton.gameObject.SetActive(false);
                submitButton.interactable = false;
            }
            else
            {
                Debug.LogError("submitButton is null.");
            }

            StartCoroutine(SendData(teamName, completionTime));

            dataSubmitted = true;
        }
    }

    public void QuitGame()
    {
        // Add any additional cleanup or saving logic if needed
        Application.Quit();
        Debug.Log("Application Quit");
    }

    IEnumerator SendData(string teamName, float completionTime)
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("teamName", teamName));
        formData.Add(new MultipartFormDataSection("completionTime", completionTime.ToString("F2")));

        using (UnityWebRequest www = UnityWebRequest.Post(serverURL, formData))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error sending data: " + www.error);
            }
            else
            {
                Debug.Log("Data sent successfully");
            }
        }
    }
}
