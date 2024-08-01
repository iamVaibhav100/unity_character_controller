using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private Rigidbody2D rb;
    private Animator anim;

    private float x;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;

    private int facingDir = 1;
    private bool isFacingRight = true;

    private bool isGrounded;
    [Header("Collison Check")]
    [SerializeField] private float groundDist;
    [SerializeField] private LayerMask ground;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        CheckInput();
        collisonCheck();
        flipController();

        AnimatorController();
    }

    private void CheckInput()
    {
        x = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();

        }
    }

    private void Movement()
    {
        rb.velocity = new Vector2(x * moveSpeed, rb.velocity.y);
    }

    private void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private void collisonCheck()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundDist, ground);
    }

    private void AnimatorController()
    {
        bool isMoving = rb.velocity.x != 0;

        anim.SetFloat("yVelocity", rb.velocity.y);

        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isGrounded", isGrounded);

    }

    private void flip()
    {
        facingDir *= -1;
        isFacingRight = !isFacingRight;
        transform.Rotate(0, 180, 0);
    }

    private void flipController() { 
        if (rb.velocity.x < 0 && isFacingRight)
        {
            flip();
        }
        else if (rb.velocity.x > 0 && !isFacingRight)
        {
            flip();
        }
    }
}
