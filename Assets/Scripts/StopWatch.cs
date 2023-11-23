using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;

public class StopWatch : MonoBehaviourPunCallbacks
{
    public float timeSoFar;
    public bool isRunning = true;
    public Text timerText;

    private static StopWatch instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            // If an instance already exists, destroy this GameObject
            Destroy(gameObject);
            return;
        }

        // Set this instance as the singleton
        instance = this;

        // Ensure that this GameObject persists across scenes
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Only the "owner" (master client) should start the stopwatch
        if (photonView.IsMine)
        {
            isRunning = true;
        }
    }

    private void Update()
    {
        if (photonView.IsMine && isRunning)
        {
            if (timeSoFar >= 0)
            {
                timeSoFar += Time.deltaTime;
                photonView.RPC("RPC_DisplayTime", RpcTarget.All, timeSoFar);
            }
        }
    }

    [PunRPC]
    void RPC_DisplayTime(float timeToDisplay)
    {
        DisplayTime(timeToDisplay);
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }
}
