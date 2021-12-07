using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{
    public static GameHandler instance;
    public Canvas winScreen;
    [HideInInspector]public Player thisPlayer;

    void Awake()
    {
        instance = this;
    }


    public void SetWonTeam(Team wonTeam)
    {
        if(thisPlayer.thisChampion.team == wonTeam)
        {
            winScreen.transform.GetChild(0).GetComponent<Text>().text = "You lose";
        }
        else
        {
            winScreen.transform.GetChild(0).GetComponent<Text>().text = "You win";
        }
    }
}
