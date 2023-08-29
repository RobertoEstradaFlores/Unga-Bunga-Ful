using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public DataJugador Data;

    private Rigidbody2D rb { get; set; }
    public bool IsFacingRight { get; private set; }
    public bool IsJumping { get; private set; }
    public float LastOnGroundTime { get; private set; }
    public float LastOnWallTime { get; private set; }
    public float LastOnWallRightTime { get; private set; }
    public float LastOnWallLeftTime { get; private set; }
    public float LastPressedJumpTime { get; private set; }
    private bool _isJumpCut;
    private bool _isJumpFalling;
    public float FuerzaMovimiento = 10f;

    public PlayerInput PlayerInput;
    private Vector2 Input;
   
    public Transform GroundCheck;
    public float RadioChecker;
    public LayerMask GroundLayer;
    bool SaltoDoble;
    bool SaltoDobleSkill;
    
   
  
   
 
    // Start is called before the first frame update
    void Start()
    {
       
        rb = GetComponent<Rigidbody2D>();
        PlayerInput =GetComponent<PlayerInput>();
        SetGravityScale(Data.gravityScale);
        IsFacingRight = true;
    }

    // Update is called once per frame
    void Update()
    {
        LastOnGroundTime -= Time.deltaTime;
        LastPressedJumpTime -= Time.deltaTime;

        Input = PlayerInput.actions["Movimiento"].ReadValue<Vector2>();

        if (Input.x != 0)
            CheckDirectionToFace(Input.x > 0);

       


    }
    private void FixedUpdate()
    {
        Run(1);
      

    }
    public void Jump(InputAction.CallbackContext callbackContext) 
    {
        float force = Data.jumpForce;
        if (callbackContext.performed && TocandoPiso())
        {

          
            if (rb.velocity.y > 0)
                force -= rb.velocity.y;

            rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
            Debug.Log("Jump");
            SaltoDoble = true;
            
        
              
        }else if(SaltoDoble&& SaltoDobleSkill)
        {
            rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
            SaltoDoble = false;
        }
       
    }
     private void Run(float lerpAmount)
     {
         float targetSpeed = Input.x * Data.runMaxSpeed;
         //We can reduce are control using Lerp() this smooths changes to are direction and speed
         targetSpeed = Mathf.Lerp(rb.velocity.x, targetSpeed, lerpAmount);

         float accelRate;
         if (LastOnGroundTime > 0)
             accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount : Data.runDeccelAmount;
         else
             accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount * Data.accelInAir : Data.runDeccelAmount * Data.deccelInAir;
         float speedDif = targetSpeed - rb.velocity.x;
         rb.velocity = new Vector2(rb.velocity.x + (Time.fixedDeltaTime  * speedDif * accelRate) / rb.mass, rb.velocity.y);
          //Time.fixedDeltaTime is by default in Unity 0.02 seconds equal to 50 FixedUpdate() calls per second

     }
    
    public bool TocandoPiso()
    {
        return Physics2D.OverlapCircle(GroundCheck.position, RadioChecker, GroundLayer);
    }
    public void CheckDirectionToFace(bool isMovingRight)
    {
        if (isMovingRight != IsFacingRight)
            Turn();
    }
    private void Turn()
    {
        //stores scale and flips the player along the x axis, 
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        IsFacingRight = !IsFacingRight;
    }

    private bool CanJump()
    {
        return LastOnGroundTime > 0 && !IsJumping;
    }
    private bool CanJumpCut()
    {
        return IsJumping && rb.velocity.y > 0;
    }
    public void SetGravityScale(float scale)
    {
        rb.gravityScale = scale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("power up")) 
        {
            SaltoDobleSkill = true;
        }
    }

}
