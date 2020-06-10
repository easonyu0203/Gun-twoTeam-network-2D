﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class restartTheGame : NetworkBehaviour
{

    public GameObject gameOver;
    public GameObject ChooseTeam;
    public GameObject RedPlayerCntText;
    public GameObject BluePlayerCntText;
    public GameObject start;
    public GameObject joinBlue;
    public GameObject JoinRed;
    public GameObject thisClient;
    public GameObject waitUI;
    public DieText dietest;
    public prepareGame pGame;

    //public prepareGame preGame;
    public void restartGamepress()
    {
        //GetComponent<prepareGame>().restart();
        dietest.showTime = 0f;
        RestartChooseTeamUI();

    }

    public void RestartChooseTeamUI()
    {
        
        gameOver.SetActive(false);
        ChooseTeam.SetActive(true);
        RedPlayerCntText.SetActive(true);
        BluePlayerCntText.SetActive(true);
        waitUI.SetActive(false);
        RedPlayerCntText.transform.position = pGame.OrgRPlayCntTextPos;
        BluePlayerCntText.transform.position = pGame.OrgBPlayCntTextPos;
        if(isServer) start.SetActive(true);
        thisClient.GetComponent<Join>().CmdConfigeCnt();
        //CmdConfigeCnt();
    }

    //[Command]
    //void CmdConfigeCnt()
    //{
    //    RpcUpdateBlueCnt(GetComponent<prepareGame>().BlueplayerCnt);
    //    RpcUpdateRedCnt(GetComponent<prepareGame>().RedplayerCnt);
    //}

    //[ClientRpc]
    //void RpcUpdateBlueCnt(int BlueCnt)
    //{
    //    Debug.Log("bluecnt " + BlueCnt);
    //    BluePlayerCntText.GetComponent<Text>().text = "Blue team current player count:\n" + BlueCnt + " / 5";
    //    if (BlueCnt == 5) joinBlue.SetActive(false);
    //}

    //[ClientRpc]
    //void RpcUpdateRedCnt(int redCnt)
    //{
    //    Debug.Log("Redcnt " + redCnt);
    //    RedPlayerCntText.GetComponent<Text>().text = "Red team current player count:\n" + redCnt + " / 5";
    //    if (redCnt == 5) JoinRed.SetActive(false);
    //}
}
