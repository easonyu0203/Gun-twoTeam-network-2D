using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class treasury : NetworkBehaviour
{
    public float collectingTime;
    public float currentProgress;
    public GameObject gameover;
    public GameObject endworlds;
    public prepareGame pGame;
    bool win;

    [SerializeField]
    Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        slider.maxValue = collectingTime;
        currentProgress = 0;
        slider.value = 0;
        win = false;
    }

    public void setProccess(float value)
    {
        slider.value = value;
    }

 

    private void OnTriggerStay2D(Collider2D collision)
    {
       
        currentProgress += Time.fixedDeltaTime;
        slider.value = currentProgress;
        if(currentProgress >= collectingTime)
        {
            Debug.Log(gameObject.name + " had been stole!");
            //Game over
            if (isServer)
            {
                CmdGameOVER();
            }
        }
    }

    [Command]
    void CmdGameOVER()
    {
        //delet players
        for(int i = 0; i < pGame.startGamePlayerCnt; i++)
        {
            NetworkServer.Destroy(GameObject.Find(pGame.playerName[i].ToString()));
        }

        pGame.restart();

        RpcgameOverUI();
    }

    [ClientRpc]
    void RpcgameOverUI()
    {
       //maybe some clean up
       if (gameObject.tag == "redTeam") endworlds.GetComponent<Text>().text = "Blue Team win!!\nthank for playing!!";
       else endworlds.GetComponent<Text>().text = "Red Team win!!\nthank for playing!!";
       gameover.SetActive(true);
        //GameObject.Find("GameManager").GetComponent<prepareGame>().restart();
        GameObject.Find("blueTeamTreasure").GetComponent<treasury>().setProccess(0f);
        GameObject.Find("blueTeamTreasure").GetComponent<treasury>().currentProgress = 0f ;
        GameObject.Find("redTeamTreasure").GetComponent<treasury>().setProccess(0f);
        GameObject.Find("redTeamTreasure").GetComponent<treasury>().currentProgress = 0f;
    }
}
