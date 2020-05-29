using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


//need some modify if change from host to sever only
public class MyNetHUD : NetworkManager
{

    public void StartupHost()
    {
        SetPort();
        NetworkManager.singleton.StartHost();
        Debug.Log("StartHost");
    }

    public void JoinGame()
    {
        SetIPAddress();
        SetPort();
        NetworkManager.singleton.StartClient();
        Debug.Log("StartClient");
    }

    void SetIPAddress()
    {
        string ipAddress = GameObject.Find("InputFieldIPAddress").transform.Find("Text").GetComponent<Text>().text;
        NetworkManager.singleton.networkAddress = ipAddress;            
    }

    void SetPort()
    {
        NetworkManager.singleton.networkPort = 7777;
    }

    private void OnLevelWasLoaded(int level)
    {
        if(level == 0)
        {
            Invoke("SetupMenuSceneButton", 0.2f);
        }
        else
        {
            SetupMainSceneButton();
        }
    }

    void SetupMenuSceneButton()
    {
        GameObject.Find("ButtonStartHost").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("ButtonStartHost").GetComponent<Button>().onClick.AddListener(StartupHost);

        GameObject.Find("ButtonJointGame").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("ButtonJointGame").GetComponent<Button>().onClick.AddListener(JoinGame);
    }

    void SetupMainSceneButton()
    {
        GameObject.Find("ButtonDisconnect").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("ButtonDisconnect").GetComponent<Button>().onClick.AddListener(NetworkManager.singleton.StopHost);
    }


}
