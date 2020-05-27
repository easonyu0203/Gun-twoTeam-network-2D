using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{


    public float speed;
    public float jumpForce;
    public Transform groundCheck;
    public Transform frontWallCheck;
    public LayerMask whatisGround;

    private bool faceRight = true;
    private bool isGrounded;
    private bool isOnWall;
    private float moveInput;
    private bool jumpInput;
    private Rigidbody2D rg2d;
    private Animator animator;
    private int extraJump = 1;

    // Start is called before the first frame update
    void Start()
    {
        rg2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>().Follow = transform;
        moveInput = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasAuthority) return;

        moveInput = Input.GetAxis("Horizontal");
        jumpInput = Input.GetKeyDown(KeyCode.W);

        if (isGrounded || isOnWall) extraJump = 1;
        if(jumpInput && extraJump > 0 && !isGrounded && !isOnWall)
        {
            rg2d.velocity = Vector2.up * jumpForce;
            extraJump--;

            animator.SetBool("doubleJump", true);
            Invoke("SetDoubleJumpF", 0.3f);

        }else if(jumpInput && (isGrounded || isOnWall))
        {
            rg2d.velocity = Vector2.up * jumpForce;
        }
    }

    void SetDoubleJumpF()
    {
        animator.SetBool("doubleJump", false);
    }

    private void FixedUpdate()
    {
        if (!hasAuthority) return;

        rg2d.velocity = new Vector2(moveInput * speed, rg2d.velocity.y);
        if ((faceRight && moveInput < 0) || (faceRight == false && moveInput > 0))
        {
            Flip();
            Cmdflip();
        }

        isGrounded = Physics2D.Linecast(transform.position, groundCheck.position, whatisGround);
        isOnWall = Physics2D.Linecast(transform.position, frontWallCheck.position, whatisGround) && !isGrounded;


        //animation
        if (moveInput == 0) animator.SetBool("isMoving", false);
        else animator.SetBool("isMoving", true);
        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("isOnWall", isOnWall);
    }

    void Flip()
    {

        if (hasAuthority && !((faceRight && moveInput < 0) || (faceRight == false && moveInput > 0))) return;

        
        faceRight = !faceRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    [Command]
    void Cmdflip()
    {
        RpcFlipAll();
    }

    [ClientRpc]
    void RpcFlipAll()
    {
        Debug.Log("flip");
        Flip();
    }
}
