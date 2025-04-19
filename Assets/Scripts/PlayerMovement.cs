using UnityEngine;

[RequireComponent(typeof(Rigidbody))] //require rigidbody component, automatically add it if not there
public class PlayerMovement : MonoBehaviour
{
    //movement settings
    //public variables in inspector
    public float moveSpeed = 5f; //speed of the player
    public float jumpForce = 5f; //force applied when jumping
    public float groundCheckDistance = 1.1f; //distance to check for ground
    public LayerMask groundMask; //layer mask to check for ground
    //private variables not in inspector, only used in script
    private Rigidbody rb; //reference to the rigidbody component
    private Transform cameraTransform; //reference to the camera transform
    private bool isGrounded; //boolean for checking if the player is grounded

    void Start()
    { //initialize references so the script can use them later
        rb = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;
        rb.freezeRotation = true;
    }

    void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundMask); //check if the player is grounded
        
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) //check if the player is grounded and space key is pressed
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z); //reset y velocity to prevent double jump
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); //apply jump force
        }
    }

    void FixedUpdate()
    {
        //get input from wasd or arrow keys
        //GetAxisRaw for instant response
        float horizontal = Input.GetAxisRaw("Horizontal"); 
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = cameraTransform.forward * vertical + cameraTransform.right * horizontal; //calculate the move direction based on camera direction
        moveDirection.y = 0; //ignore the y component to prevent moving up/down
        moveDirection.Normalize(); //normalize the direction to prevent faster diagonal movement
        //apply movement
        Vector3 velocity = moveDirection * moveSpeed; //set the velocity based on the move direction and speed
        velocity.y = rb.linearVelocity.y; //keep the y velocity for jumping
        rb.linearVelocity = velocity; //apply the velocity to the rigidbody
    }
}