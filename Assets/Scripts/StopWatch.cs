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

    //This methoid is to make sure there is only only one timer in the scene 
    //If there is another it will automatically be destroyed
    //This is needed do to the fact it needs to not be destryoed across scenes and photon doesnt allow that
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
        if (view.IsMine)
        {
            isRunning = true;
        }
    }

    //This method checks to see if the view is the masters and is running
    //If both of these are true then it will increment the time using time.delta.time
    //It also called the method rpcdisplaytime
    private void Update()
    {
        if (view.IsMine && isRunning)
        {
            if (timeSoFar >= 0)
            {
                timeSoFar += Time.deltaTime;
                photonView.RPC("RPC_DisplayTime", RpcTarget.All, timeSoFar);
            }
        }
    }

    //This method calls the display time on all cilents with the updated time as an argument
    [PunRPC]
    private void RPC_DisplayTime(float timeToDisplay)
    {
        DisplayTime(timeToDisplay);
    }

    //This method takes the "timeToDisplay" and formats it into time(mins and seconds). 
    //This then gets assigned to the UI text "timerText" showing the time elasped
    private void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.Floor(timeToDisplay / 60);
        float seconds = Mathf.Floor(timeToDisplay % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    //This is a method to stop the timer(it is also referenced to stop the timer when they submit the data)
    //This happens in the leaderboard script.
    public void StopTimer()
    {
        isRunning = false;
    }
}
