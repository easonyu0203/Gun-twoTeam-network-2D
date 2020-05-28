using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class playerstate : NetworkBehaviour
{
    public float maxHealth = 100;
    [SyncVar]
    public float currentHealth;
    public float recoverRate;
    public string enemyBulletTag;
    [SerializeField]
    healthBarBehave hbhave;
    [SerializeField]
    Animator animator;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
      
        if (!isServer) return;
        currentHealth += Time.deltaTime * recoverRate;
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
        }
        Debug.Log(gameObject.name + " is hit");
        animator.SetBool("hurt", true);
        Invoke("setHurtF", 0.05f);
        if(currentHealth <= 0)
        {
            CmdDie();
        }
    }
    [Command]
    void CmdDie()
    {
        animator.SetBool("alive", false);
        animator.SetTrigger("die");
        RpcDie();
    }

    [ClientRpc]
    void RpcDie()
    {
        Debug.Log("die!!!!");
        animator.SetTrigger("die");
        GetComponent<Rigidbody2D>().simulated = false;
        GetComponent<PlayerController>().enabled = false;
        GetComponent<PlayerFire>().enabled = false;
        recoverRate = 0;
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
}
