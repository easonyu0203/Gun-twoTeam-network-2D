using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerFire : NetworkBehaviour
{
    [SerializeField]
    Transform firePoint;
    [SerializeField]
    GameObject whatBullet;
    [SerializeField]
    Animator animator;
    public float fireCooldownTime;
    private float nextShootTime = -1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasAuthority) return;
        if (Input.GetKey(KeyCode.Space) )
        {
            animator.SetBool("shoot", true);
            if (Time.time < nextShootTime) return;
            nextShootTime = Time.time + fireCooldownTime;
            if(transform.localScale.x > 0)
            {
                CmdFire(true);
            }
            else
            {
                CmdFire(false);
            }

        }
        else
        {
            animator.SetBool("shoot", false);
        }

    }

    [Command]
    void CmdFire(bool faceRight)
    {
        GameObject temp = (GameObject)Instantiate(whatBullet, firePoint.position, firePoint.rotation);
            //Vector3 Scaler = temp.transform.localScale;
            //Scaler.x *= -1;
            //temp.transform.localScale = Scaler;
        if (!faceRight)
        {
            temp.GetComponent<Rigidbody2D>().velocity = Vector2.left * temp.GetComponent<bullet>().speed;
        }
        else
        {
            temp.GetComponent<Rigidbody2D>().velocity = Vector2.right * temp.GetComponent<bullet>().speed;
        }
        NetworkServer.Spawn(temp);

    }
}
