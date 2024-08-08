using UnityEngine;

public class Player : MonoBehaviour
{

    private Rigidbody2D rb;
    private Animator anim;

    private float xInput;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;

    private int facingDir = 1;
    private bool isFacingRight = true;

    private bool isGrounded;
    [Header("Collison Check")]
    [SerializeField] private float groundDist;
    [SerializeField] private LayerMask ground;

    [Header("Dash info")]
    [SerializeField] private float dashDuration;
    private float dashTime;
    [SerializeField] private int dashSpeed;
    [SerializeField] private float dashCoolDown;
    private float dashCoolDownTimer;

    [Header("Better jump")]
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowFallMultiplier = 2.0f;

    [Header("Attack info")]
    [SerializeField] private float attackComboTime;
    private float attackComboWindow;
    private int attackComboCount;
    private bool isAttacking;


    
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
        IsMoving();
        CheckInput();
        collisonCheck();
        flipController();
        BetterJump();

        dashTime -= Time.deltaTime;
        dashCoolDownTimer -= Time.deltaTime;
        attackComboWindow -= Time.deltaTime;

        AnimatorController();
    }

    private void BetterJump()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        } else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space)) {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowFallMultiplier - 1) * Time.deltaTime;
        } 
    }

    public void AttackOver()
    {
        isAttacking = false;
        attackComboCount++;

        if (attackComboCount > 2)
        {
            attackComboCount = 0;
        }
    }

    private void CheckInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartAttackEvent();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();

        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            DashAbility();
        }
    }

    private void StartAttackEvent()
    {
        if (!isGrounded)
            return;


        if (attackComboWindow < 0)
            attackComboCount = 0;

        isAttacking = true;
        attackComboWindow = attackComboTime;
    }

    private void DashAbility()
    {
        if (dashCoolDownTimer < 0 && !isAttacking)
        {
            dashCoolDownTimer = dashCoolDown;
            dashTime = dashDuration;
        }
    }

    private void Movement()
    {
        if (isAttacking)
        {
            rb.velocity = new Vector2(0, 0);
        }

        else if (dashTime > 0 && IsMoving())
        {
            rb.velocity = new Vector2(xInput * dashSpeed, 0);
        }
        else
        {
            rb.velocity = new Vector2(xInput * moveSpeed, rb.velocity.y);
        }
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

    private bool IsMoving()
    {
        return rb.velocity.x != 0;
    }

    private void AnimatorController()
    {
        anim.SetFloat("yVelocity", rb.velocity.y);

        anim.SetBool("isMoving", IsMoving());
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isDashing", dashTime > 0 && IsMoving());
        anim.SetBool("isAttacking", isAttacking);
        anim.SetInteger("attackComboCount", attackComboCount);

    }

    private void flip()
    {
        facingDir *= -1;
        isFacingRight = !isFacingRight;
        transform.Rotate(0, 180, 0);
    }

    private void flipController()
    {
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
