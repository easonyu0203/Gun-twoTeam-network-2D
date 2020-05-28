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

    [SyncVar]
    private bool faceRight;

    // Start is called before the first frame update
    void Start()
    {
        if (isServer)
        {
            if (transform.localScale.x > 0) faceRight = true;
            else faceRight = false;
            if (faceRight) rg2d.velocity = Vector2.right * speed;
            else rg2d.velocity = Vector2.left * speed;
            Debug.Log(transform.localScale.x);
        }
        else
        {
            if(!faceRight)
            {
                Vector3 Scaler = transform.localScale;
                Scaler.x *= -1;
                transform.localScale = Scaler;
            }
        }


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
