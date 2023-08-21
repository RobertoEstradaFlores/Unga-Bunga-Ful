using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private float UpForce =185f;
    private float MovForce = 7f;
    private float MaxSpeed = 100f;
    private Rigidbody2D rb;
    public PlayerInput PlayerInput;
    private Vector2 Input;
    Vector2 Gravedad;
    public Transform GroundCheck;
    public float RadioChecker;
    public LayerMask GroundLayer;
    bool SaltoDoble;
    bool SaltoDobleSkill;
    
   
  
   
 
    // Start is called before the first frame update
    void Start()
    {
        Gravedad = new Vector2(0,- Physics2D.gravity.y);
        rb = GetComponent<Rigidbody2D>();
        PlayerInput =GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        
       Input= PlayerInput.actions["Movimiento"].ReadValue<Vector2>();


        if (rb.velocity.magnitude >MaxSpeed) 
        {
            rb.velocity =(Vector2)Vector3.ClampMagnitude((Vector3)rb.velocity, MaxSpeed);
        }

    }
    private void FixedUpdate()
    {
        rb.AddForce(new Vector3(Input.x,0f, Input.y) * MovForce);
    }
    public void Jump(InputAction.CallbackContext callbackContext) 
    {
        if (callbackContext.performed && TocandoPiso())
        {
            
                rb.AddForce(Vector3.up * UpForce);
                Debug.Log("Jump");
            SaltoDoble = true;
            
        
              
        }else if(SaltoDoble&& SaltoDobleSkill)
        {
            rb.AddForce(Vector3.up * UpForce*0.40f);
            SaltoDoble= false;
        }
       
    }
    public bool TocandoPiso()
    {
        return Physics2D.OverlapCircle(GroundCheck.position, RadioChecker, GroundLayer);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("power up")) 
        {
            SaltoDobleSkill = true;
        }
    }

}
