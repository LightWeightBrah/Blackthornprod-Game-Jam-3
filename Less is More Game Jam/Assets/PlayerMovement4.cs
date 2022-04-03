using UnityEngine;

public class PlayerMovement4 : MonoBehaviour
{
    [Header("For Movement")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float airMoveSpeed;
    private float XDirectionalInput;
    private bool facingRight = true;
    private bool isMoving;

    [Header("For Jumping")]
    [SerializeField] float jumpForce = 16f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform groundCheckPoint;
    [SerializeField] Vector2 groundCheckSize;
    private bool grounded;
    private bool canJump;

    public int maxJumpCounter = 1;
    [SerializeField] int jumpCounter;

    [Header("For WallSliding")]
    [SerializeField] float wallSlideSpeed;
    [SerializeField] LayerMask wallLayer;
    [SerializeField] Transform WallCheckPoint;
    [SerializeField] Vector2 wallCheckSize;

    private bool isTouchingWall;
    public bool isWallSliding;

    [Header("For WallJumping")]
    [SerializeField] float walljumpforce;
    [SerializeField] Vector2 walljumpAngle;
    [SerializeField] float walljumpDirection = -1;

    [Header("For Dash")]
    [SerializeField] bool dashing;
    public float dashForce;
    public int maxDashCounter = 1;
    [SerializeField] int dashCounter;
    [SerializeField] float dashDirectiom=1;
    [SerializeField] float dashTime = 2f;
    [SerializeField] float dashTimeCounter;
    bool dddash = false; 

    [Header("Other")]
    public Animator anim;
    private Rigidbody2D rb;

    public bool canMove = true;

    public bool isAbleToDash = false;
    public bool isAbleToWallJump = false;

    public bool isTalking;

    private void Start()
    {
        dashTimeCounter = dashTime;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        walljumpAngle.Normalize();
    }
    private void Update()
    {
        Inputs();
        CheckWorld();
    }
    private void FixedUpdate()
    {
        if(isTalking)
        {
            //anim.Play("protagonist_idle");
            anim.SetBool("isMoving", false);
            return;
        }
        if (!canMove) return;
        Move();
        Jump();
        WallJump();
        WallSlide();
        Dash();
        canJump = false;
        dashing = false;
    }
    void Inputs()
    {
        XDirectionalInput = Input.GetAxis("Horizontal");
        if (Input.GetButtonDown("Jump"))
        {
            canJump = true;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && isAbleToDash)
        {
            dashing = true;
        }
    }
    void CheckWorld()
    {
        grounded = Physics2D.OverlapBox(groundCheckPoint.position, groundCheckSize, 0, groundLayer);
        isTouchingWall = Physics2D.OverlapBox(WallCheckPoint.position, wallCheckSize, 0, wallLayer);
    }
    void Move()
    {
        if (dashing && XDirectionalInput != 0)
        {
            rb.AddForce(new Vector2(dashForce * XDirectionalInput, 0));
            if (Mathf.Abs(rb.velocity.x) > moveSpeed)
            {
                rb.velocity = new Vector2(XDirectionalInput * moveSpeed, rb.velocity.y);
            }

            dddash = true;

            return;
        }

        if(dddash)
        {
            dashTimeCounter -= Time.deltaTime;

            if(dashTimeCounter < 0)
            {
                dddash = false;
                dashTimeCounter = dashTime;
            }

            if(grounded || isWallSliding)
            {
                dddash = false;
                dashTimeCounter = dashTime;
            }
 

            return;
        }
        else if (XDirectionalInput!=0)
        {
            isMoving = true;
            anim.SetBool("isMoving", isMoving);
        }
        else
        {
            isMoving = false;
            anim.SetBool("isMoving", isMoving);
        }
        if (grounded)
        {
            rb.velocity = new Vector2(XDirectionalInput * moveSpeed, rb.velocity.y);
        }
        else if (!grounded&&!isWallSliding&& !dashing&&XDirectionalInput!=0)
        {
            rb.AddForce(new Vector2(airMoveSpeed*XDirectionalInput,0));
            if (Mathf.Abs(rb.velocity.x)>moveSpeed)
            {
                rb.velocity = new Vector2(XDirectionalInput * moveSpeed, rb.velocity.y);
            }
        }

        if (XDirectionalInput<0&&facingRight)
        {
            Flip();
        }
        else if(XDirectionalInput > 0 && !facingRight)
        {
            Flip();
        }
    }
    void Flip()
    {
        walljumpDirection *= -1;
        dashDirectiom *= -1;

        facingRight = !facingRight;
        transform.Rotate(0,180,0);
    }
    void Jump()
    {
        if (canJump && jumpCounter >= 1)
        {
            //dodalem to 
            anim.SetTrigger("jump");
            //koniec
            grounded = false;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCounter--;
        }
        if (grounded)
        {
            jumpCounter=maxJumpCounter;
            dashCounter = maxDashCounter;
            //dodalem to
            anim.SetBool("isJumping", false);
            //koniec
        }
        //dodalem ta linijke
        else
        {
            anim.SetBool("isJumping", true);
        }
        //koniec
    }
    void Dash()
    {
        if (dashing &&dashCounter >= 1&&!grounded)
        {
            rb.AddForce(new Vector2(dashForce * dashDirectiom, XDirectionalInput ), ForceMode2D.Impulse);
            dashCounter--;
        }

    }
    void WallJump()
    {
        if (!isAbleToWallJump) return;

        if ((isWallSliding || isTouchingWall)&& canJump)
        {
            rb.AddForce(new Vector2(walljumpforce * walljumpDirection * walljumpAngle.x, walljumpforce * walljumpAngle.y),ForceMode2D.Impulse);
            canJump = false;
    
        }
    }
    void WallSlide()
    {
        if (!isAbleToWallJump) return;

        if (isTouchingWall&& !grounded&&rb.velocity.y<0&& isMoving)
        {
            isWallSliding = true;
            anim.SetBool("isWallSliding", true);
        }
        else
        {
            isWallSliding = false;
            anim.SetBool("isWallSliding", false);
        }
        if (isWallSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, wallSlideSpeed);
            jumpCounter = maxJumpCounter;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(groundCheckPoint.position, groundCheckSize);

        Gizmos.color = Color.red;
        Gizmos.DrawCube(WallCheckPoint.position, wallCheckSize);
    }


}
