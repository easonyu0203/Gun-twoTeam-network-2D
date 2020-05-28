using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class prepareGame : NetworkBehaviour
{
   
    public int playerCnt = 0;
    public int RedplayerCnt = 0;
    public int BlueplayerCnt = 0;
    public GameObject RedPlayer;
    public GameObject BluePlayer;
    public Transform[] RedSpawnPoint;
    public Transform[] BlueSpawnPoint;

    //gameObject name
    public ArrayList RedTeam = new ArrayList();
    public ArrayList BlueTeam = new ArrayList();

    private void Start()
    {
       
        if (!isServer)
        {
            GameObject.Find("start").SetActive(false);
        }
    }

    private void Update()
    {
        if (RedplayerCnt + BlueplayerCnt == 10) startGame();
    }

    public void AddPlayer()
    {
        playerCnt++;
        Debug.Log("playerCnt: " + playerCnt);
    }

    public void startGame()
    {
        CmdStartGame();
  
    }

    [Command]
    void CmdStartGame()
    {
        for(int i = 0; i < RedplayerCnt; i++)
        {
            GameObject temp = (GameObject)Instantiate(RedPlayer, RedSpawnPoint[i]);
            GameObject owner = GameObject.Find(RedTeam[i].ToString());
            NetworkServer.SpawnWithClientAuthority(temp, owner);
        }

        for (int i = 0; i < BlueplayerCnt; i++)
        {
            GameObject temp = (GameObject)Instantiate(BluePlayer, BlueSpawnPoint[i]);
            GameObject owner = GameObject.Find(BlueTeam[i].ToString());
            NetworkServer.SpawnWithClientAuthority(temp, owner);
        }

        RpcturnOffUI();


    }

    [ClientRpc]
    void RpcturnOffUI()
    {
        GameObject.Find("RedPlayerCnt").SetActive(false);
        GameObject.Find("BluePlayerCnt").SetActive(false);
        if(GameObject.Find("start") != null) GameObject.Find("start").SetActive(false);
    }

}
