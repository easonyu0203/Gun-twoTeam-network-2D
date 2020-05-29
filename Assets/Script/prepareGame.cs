using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    public GameObject disconnectButton;
    public int startGamePlayerCnt;
    public Vector3 OrgRPlayCntTextPos;
    public Vector3 OrgBPlayCntTextPos;

    //gameObject name
    public ArrayList RedTeam = new ArrayList();
    public ArrayList BlueTeam = new ArrayList();
    public ArrayList playerName = new ArrayList();

    public void restart()
    {
        playerCnt = 0;
        RedplayerCnt = 0;
        BlueplayerCnt = 0;
        RedTeam.Clear();
        BlueTeam.Clear();
        playerName.Clear();
        
    }

    //only run on server
    public void remove(string name)
    {
        Debug.Log("removing " + name);
        for(int i = 0; i < RedplayerCnt; i++)
        {
            if(name == RedTeam[i].ToString())
            {
                RedTeam.RemoveAt(i);
                RedplayerCnt--;
                playerCnt--;
                break;
            }
        }
        for(int i = 0; i < BlueplayerCnt; i++)
        {
            if(name == BlueTeam[i].ToString())
            {
                BlueTeam.RemoveAt(i);
                BlueplayerCnt--;
                playerCnt--;
                break;
            }
        }
    }

    private void Start()
    {
        OrgRPlayCntTextPos = GameObject.Find("RedPlayerCnt").transform.position;
        OrgBPlayCntTextPos = GameObject.Find("BluePlayerCnt").transform.position;
        disconnectButton.GetComponent<Button>().onClick.AddListener(NetworkManager.singleton.StopHost);
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
        startGamePlayerCnt = RedplayerCnt + BlueplayerCnt;
        for(int i = 0; i < RedplayerCnt; i++)
        {
            GameObject temp = (GameObject)Instantiate(RedPlayer, RedSpawnPoint[i].position, RedSpawnPoint[i].rotation);
            GameObject owner = GameObject.Find(RedTeam[i].ToString());
            temp.GetComponent<playerstate>().owner = owner.name;
            temp.name = "RedPlayer" + i.ToString();
            playerName.Add(temp.name);
            NetworkServer.SpawnWithClientAuthority(temp, owner);
        }

        for (int i = 0; i < BlueplayerCnt; i++)
        {
            GameObject temp = (GameObject)Instantiate(BluePlayer, BlueSpawnPoint[i].position, BlueSpawnPoint[i].rotation);
            GameObject owner = GameObject.Find(BlueTeam[i].ToString());
            temp.GetComponent<playerstate>().owner = owner.name;
            temp.name = "BluePlayer" + i.ToString();
            playerName.Add(temp.name);
            NetworkServer.SpawnWithClientAuthority(temp, owner);
        }

        RpcturnOffUI();


    }

    [ClientRpc]
    void RpcturnOffUI()
    {
        GameObject.Find("BluePlayerCnt").transform.position = OrgBPlayCntTextPos;
        GameObject.Find("RedPlayerCnt").transform.position = OrgRPlayCntTextPos;
        GameObject.Find("RedPlayerCnt").SetActive(false);
        GameObject.Find("BluePlayerCnt").SetActive(false);
        GameObject.Find("waitOther").SetActive(false);
        if(GameObject.Find("start") != null) GameObject.Find("start").SetActive(false);
    }

}
