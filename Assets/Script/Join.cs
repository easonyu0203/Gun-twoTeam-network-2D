using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Join : NetworkBehaviour
{

    public prepareGame pregame;

    public GameObject ChooseTeam;
    public GameObject RedPlayerCntText;
    public GameObject BluePlayerCntText;
    public GameObject start;
    public GameObject joinBlueBut;
    public GameObject JoinRedBut;
    public restartTheGame reGame;
    public GameObject waitOtherUI;
    public Vector3 newRPCntTextPos;
    public Vector3 newBPCntTextPos;

    private void Start()
    {
        newRPCntTextPos = new Vector3(0, 89, 0) + new Vector3(1662.7f, 1247,0);
        newBPCntTextPos = new Vector3(0, -82, 0) + new Vector3(1662.7f, 1247, 0);
        pregame = GameObject.Find("GameManager").GetComponent<prepareGame>();
        reGame = GameObject.Find("GameManager").GetComponent<restartTheGame>();
        gameObject.name = gameObject.name + netId.ToString();   
        if (!isLocalPlayer) return;
        waitOtherUI = GameObject.Find("waitOther");
        waitOtherUI.SetActive(false);
        CmdAddPlayer();
        ChooseTeam = GameObject.Find("ChooseTeam");
        RedPlayerCntText = GameObject.Find("RedPlayerCnt");
        BluePlayerCntText = GameObject.Find("BluePlayerCnt");
        joinBlueBut = GameObject.Find("JoinBlue");
        JoinRedBut = GameObject.Find("JoinRed");
        JoinRedBut.GetComponent<Button>().onClick.AddListener(JoinRed);
        joinBlueBut.GetComponent<Button>().onClick.AddListener(JoinBlue);
        GameObject.Find("ButtonDisconnect").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("ButtonDisconnect").GetComponent<Button>().onClick.AddListener(buttonDis);
        //GameObject.Find("ButtonDisconnect").GetComponent<Button>().onClick.AddListener(NetworkManager.singleton.StopHost);

        reGame.thisClient = gameObject;
        
    }

    void buttonDis()
    {
        //DClient();
        Debug.Log("press disconnect button");
        CmdRemove();
        CmdConfigeCnt();
        Invoke("Dis", 0.5f);
    }

    void Dis()
    {
        if (!isServer) NetworkManager.singleton.StopClient();
        else NetworkManager.singleton.StopHost();
    }

    [Command]
    void CmdRemove()
    {
        Debug.Log("server removing" + gameObject.name);
        pregame.remove(gameObject.name);
    }



    [Command]
    public void CmdConfigeCnt()
    {
        RpcUpdateBlueCnt(pregame.BlueplayerCnt);
        RpcUpdateRedCnt(pregame.RedplayerCnt);
    }

  

    public void RestartChooseTeamUI()
    {
        ChooseTeam.SetActive(true);
        RedPlayerCntText.SetActive(true);
        BluePlayerCntText.SetActive(true);
        start.SetActive(true);
    }

    void JoinRed()
    {
        if (isServer) pregame.startButton.SetActive(true);
        Debug.Log("press join red button");
        CmdJoinRed();
        ChooseTeam.SetActive(false);
        waitOtherUI.SetActive(true);
        RedPlayerCntText.transform.position = newRPCntTextPos;
        BluePlayerCntText.transform.position = newBPCntTextPos;
    }

    void JoinBlue()
    {
        if (isServer) pregame.startButton.SetActive(true);
        CmdJoinBlue();
        ChooseTeam.SetActive(false);
        waitOtherUI.SetActive(true);
        RedPlayerCntText.transform.position = newRPCntTextPos;
        BluePlayerCntText.transform.position = newBPCntTextPos;
    }

    [Command]
    public void CmdJoinRed()
    {
        Debug.Log("sever kown somone join red");
        pregame.RedTeam.Add(gameObject.name);
        pregame.RedplayerCnt++;
        RpcUpdateRedCnt(pregame.RedplayerCnt);
        RpcUpdateBlueCnt(pregame.BlueplayerCnt);
        Debug.Log(gameObject.name + "became red team\n red player cnt: " + pregame.RedplayerCnt);
    }

    [ClientRpc]
    void RpcUpdateRedCnt(int redCnt)
    {
        if (GameObject.Find("RedPlayerCnt") == null) return;
        GameObject.Find("RedPlayerCnt").GetComponent<Text>().text = "Red team current player count:\n" + redCnt +" / 6";
        if (redCnt == 5) GameObject.Find("JoinRed").SetActive(false);
    }

    [Command]
    public void CmdJoinBlue()
    {
        pregame.BlueTeam.Add(gameObject.name);
        pregame.BlueplayerCnt++;
        RpcUpdateRedCnt(pregame.RedplayerCnt);
        RpcUpdateBlueCnt(pregame.BlueplayerCnt);
        Debug.Log(gameObject.name + "became blue team\n blue player cnt: " + pregame.BlueplayerCnt);
    }

    [ClientRpc]
    void RpcUpdateBlueCnt(int BlueCnt)
    {
        if (GameObject.Find("BluePlayerCnt") == null) return;
        GameObject.Find("BluePlayerCnt").GetComponent<Text>().text = "Blue team current player count:\n" + BlueCnt + " / 6";
        if (BlueCnt == 5) GameObject.Find("JoinBlue").SetActive(false);
    }
    [Command]
    void CmdAddPlayer()
    {
        pregame.AddPlayer();
        RpcUpdateRedCnt(pregame.RedplayerCnt);
        RpcUpdateBlueCnt(pregame.BlueplayerCnt);
    }
}
