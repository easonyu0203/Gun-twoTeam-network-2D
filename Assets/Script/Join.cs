using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Join : NetworkBehaviour
{

    public prepareGame pregame;

    private void Start()
    {
        pregame = GameObject.Find("GameManager").GetComponent<prepareGame>();
        gameObject.name = gameObject.name + netId.ToString();
        Debug.Log(isLocalPlayer);
        if (!isLocalPlayer) return;
        CmdAddPlayer();
        GameObject.Find("JoinRed").GetComponent<Button>().onClick.AddListener(JoinRed);
        GameObject.Find("JoinBlue").GetComponent<Button>().onClick.AddListener(JoinBlue);
    }

    void JoinRed()
    {
        CmdJoinRed();
        GameObject.Find("ChooseTeam").SetActive(false);
    }

    void JoinBlue()
    {
        CmdJoinBlue();
        GameObject.Find("ChooseTeam").SetActive(false);
    }

    [Command]
    public void CmdJoinRed()
    {
        pregame.RedTeam.Add(gameObject.name);
        pregame.RedplayerCnt++;
        RpcUpdateRedCnt(pregame.RedplayerCnt);
        RpcUpdateBlueCnt(pregame.BlueplayerCnt);
        Debug.Log(gameObject.name + "became red team\n red player cnt: " + pregame.RedplayerCnt);
    }

    [ClientRpc]
    void RpcUpdateRedCnt(int redCnt)
    {
        GameObject.Find("RedPlayerCnt").GetComponent<Text>().text = "current player count:\n" + redCnt +" / 5";
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
        GameObject.Find("BluePlayerCnt").GetComponent<Text>().text = "current player count:\n" + BlueCnt + " / 5";
    }
    [Command]
    void CmdAddPlayer()
    {
        pregame.AddPlayer();
        RpcUpdateRedCnt(pregame.RedplayerCnt);
        RpcUpdateBlueCnt(pregame.BlueplayerCnt);
    }
}
