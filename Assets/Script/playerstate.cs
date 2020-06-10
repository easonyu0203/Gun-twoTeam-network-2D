using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class playerstate : NetworkBehaviour
{
    public float maxHealth = 100;
    [SyncVar]
    public float currentHealth;
    public float recoverRate;
    public string enemyBulletTag;
    public DieText dietext;
    public GameObject canvas;
    [SerializeField]
    healthBarBehave hbhave;
    [SerializeField]
    Animator animator;
    public float dieTime;
    private bool isDie;
    private int Orglayer;
    public string owner = "none";

    private void Start()
    {
        animator.SetBool("alive", true);
        currentHealth = maxHealth;
        isDie = false;
        Orglayer = gameObject.layer;
        dietext = GameObject.Find("_Canvas").GetComponentInChildren<DieText>();
        if (hasAuthority)
        {
            GameObject.Find("ButtonDisconnect").GetComponent<Button>().onClick.AddListener(destroythisPlayer);
        }
    }

    void destroythisPlayer()
    {
        CmdDePlayer();
    }

    [Command]
    void CmdDePlayer()
    {
        GameObject.Find("GameManager").GetComponent<prepareGame>().startGamePlayerCnt--;
        Debug.Log("Destroy game object: " + gameObject.name);
        GameObject.Find("GameManager").GetComponent<prepareGame>().playerName.Remove(gameObject.name);
        NetworkServer.Destroy(gameObject);
    }

    private void Update()
    {
        if (!isServer) return;
        if(!isDie) currentHealth += Time.deltaTime * recoverRate;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        RpcSetHealthBar(currentHealth);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isServer) return;
        Debug.Log("hit some thing\n tag: " + collision.tag);
        if(collision.tag == enemyBulletTag)
        {
            currentHealth -= collision.GetComponent<bullet>().damage;
            Debug.Log(gameObject.name + " is hit");
            RpcSetHurt();
        }
        if(currentHealth <= 0)
        {
            isDie = true;
            RpcDie();
            Invoke("CmdReSpawn", dieTime);
            RpcPlayDieUI(dieTime);
        }
    }

    [ClientRpc]
    void RpcSetHurt()
    {
        animator.SetTrigger("hurt");
    }

    [ClientRpc]
    void RpcPlayDieUI(float _dieTime)
    {
        if (hasAuthority)
        {
            //canvas.GetComponentInChildren<DieText>().showTime = _dieTime;
            dietext.showTime = _dieTime;
        }
    }

    [Command]
    void CmdReSpawn()
    {
        //animator.SetBool("alive", true);
        dieTime += 3;
        isDie = false;
        currentHealth = maxHealth;
        RpcReSpawn();
    }

    

    [ClientRpc]
    void RpcDie()
    {
        //Debug.Log("die!!!!");
        animator.SetBool("alive", false);
        animator.Play("die");
        //animator.SetTrigger("die");
        if (hasAuthority)
        {
            GetComponent<PlayerController>().enabled = false;
            GetComponent<PlayerFire>().enabled = false;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
        gameObject.layer = LayerMask.NameToLayer("die");
    }

    [ClientRpc]
    void RpcSetHealthBar(float value)
    {
        hbhave.setHealthBar(value);
    }

    void setHurtF()
    {
        animator.SetBool("hurt", false);
    }

    [ClientRpc]
    void RpcReSpawn()
    {
            animator.SetBool("alive", true);
        if (hasAuthority)
        {
            GetComponent<PlayerController>().enabled = true;
            GetComponent<PlayerFire>().enabled = true;
            if (Orglayer == LayerMask.NameToLayer("blueTeam"))
            {
                transform.position = GameObject.Find("blueSpawn0").transform.position;
            }
            else
            {
                transform.position = GameObject.Find("redSpawn0").transform.position;
            }
        }
            gameObject.layer = Orglayer;
    }
}
