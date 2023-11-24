using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class StopWatch : MonoBehaviourPunCallbacks
{
    public float timeSoFar;
    public bool isRunning = true;
    public Text timerText;

    private static StopWatch instance;

    PhotonView view;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        view = GetComponent<PhotonView>();
        // Ensure the timer starts only for the local player (master client)
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
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StopTimer()
    {
        isRunning = false;
    }
}
