using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Properties")]
    public float speedPlayer = 8f;
    private float moveInput;

    [Header("Jump Properties")]
    public float jumpForce = 10f;
    private int addJump;
    public int addJumpValue;
    private float jumpTimeCounter;
    public float jumpTime;

    [Header("Flags Status")]
    public bool isGround;
    public bool isJumping;

    [Header("PhisicsCheck")]
    public Transform feetPos;
    public float checkRadius;
    public LayerMask layerGround;

    BoxCollider2D boxCollider;
    Rigidbody2D rigidBody;

    private int direction = 1;
    private float originalXScale;

    float playerHeight;						
    private Vector2 colliderStandSize;              
    private Vector2 colliderStandOffset;			

    void Start()
    {
        addJump = addJumpValue;

        boxCollider = GetComponent<BoxCollider2D>();
        rigidBody = GetComponent<Rigidbody2D>();

        originalXScale = transform.localScale.x;

        playerHeight = boxCollider.size.y;

        colliderStandSize = boxCollider.size;
        colliderStandOffset = boxCollider.offset;
    }

    void Update()
    {
        PhisicsCheck();
        JumpMove();
    }

    void FixedUpdate()
    {
        GroundMove();
    }

    void GroundMove()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        float horizontalVelocity = speedPlayer * moveInput;

        if(horizontalVelocity * direction < 0f)
        {
            FlipCharacter();
        }

        rigidBody.velocity = new Vector2(horizontalVelocity, rigidBody.velocity.y);
    }

    void JumpMove()
    {
        if(isGround == true)
        {
            addJump = addJumpValue;
        }

        if(Input.GetKeyDown(KeyCode.Space) && addJump > 0)
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rigidBody.velocity = Vector2.up * jumpForce;
            addJump--;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && addJump == 0 && isGround == true)
        {
            rigidBody.velocity = Vector2.up * jumpForce;
        }

        if (Input.GetKey(KeyCode.Space) && isJumping == true)
        {
            if (jumpTimeCounter > 0)
            {
                rigidBody.velocity = Vector2.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }
    }

    void FlipCharacter()
    {
        direction = direction * -1;

        Vector3 scale = transform.localScale;

        scale.x = originalXScale * direction;

        transform.localScale = scale;
    }

    void PhisicsCheck()
    {
        isGround = Physics2D.OverlapCircle(feetPos.position, checkRadius, layerGround);
    }
}
