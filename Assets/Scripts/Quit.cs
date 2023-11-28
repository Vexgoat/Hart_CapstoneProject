using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This script is used to quit the game once you lose 
//Otherwise you can alt/f4 ig :0
public class Quit : MonoBehaviour
{
public Button quitButton;


    public void Start()
    {
        if (quitButton != null)
        {
            quitButton.onClick.AddListener(QuitGame);
        }
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
