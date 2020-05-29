using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class bullet : NetworkBehaviour
{

    [SerializeField]
    Animator animator;
    [SerializeField]
    Rigidbody2D rg2d;
    public LayerMask canHit;
    public float speed;
    public float damage;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("bullet speed" + GetComponent<Rigidbody2D>().velocity.x);
        if (!isServer) return;

        if(rg2d.velocity.x < 0)
        {
            RpcFlip();
        }

    }

    [ClientRpc]
    void RpcFlip()
    {
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isServer) return;
        rg2d.velocity = Vector2.zero;
        animator.SetBool("hit", true);
        Invoke("CmdDestroy", 0.3f);
    }

    [Command]
    void CmdDestroy()
    {
        NetworkServer.Destroy(gameObject);
    }

    
}
