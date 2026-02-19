using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewMonoBehaviourScript : MonoBehaviour

{
    public float walkSpeed = 9;
    public float jumpHeight = 5;
    public Transform cameraTransform;
    public float lookSensitivity = 1f;
    private CharacterController cc;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private float verticalVelocity;
    private float gravity = -20f;
    private float pitch;

    //Interaction variables

    private GameObject currentTarget;
    public ImageConversion reticleImage;
    private bool InteractPress;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        cc = GetComponent<CharacterController>();
        //optional cursor locking 
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //find the reticle 
        reticleImage = GameObject.Find("Reticle").GetComponent<Image> ();
        reticleImage.color = nww Color(r:0, g:0, b:0); 
    }

    // Update is called once per frame
    private void Update()
    {
        HandleLook();
        HandleMovement();
    }



    private void HandleLook()
    {

        float yaw = lookInput.x * lookSensitivity;

        float pitchDelta = lookInput.y * lookSensitivity;

        transform.Rotate(eulers: Vector3.up * yaw);
    }
    
    
    
    private void HandleMovement()
    {
        bool grounded = cc.isGrounded;
        Debug.Log("is grounded:" + grounded);

        if(grounded && verticalVelocity <= 0 )
        {
            verticalVelocity = -2f;

        }

        float currentSpeed = walkSpeed;

        if (isRunning)
        {
            currentSpeed = runSpeed;
        }

        else if (!isRunning)
        {
            currentSpeed = walkSpeed;
        }


        Vector3 move = transform.right * moveInput * currentSpeed + transform.forward * moveInput.y * currentSpeed;

        if (isJumping && grounded)
        {
            verticalVelocity = Mathf.Sqrt(f: jumpHeight * -2f * gravity);

        }

        else
        {
            isJumping = false;

        }

        //convert vertical velocity into movement vector 

        Vector3 velocity = Vector3.up * verticalVelocity;

        //Moving our player 
        cc.Move(motion:(move+velocity)*Time.deltaTime);
    }

    public void onMove()
    {
        ()
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed) InteractPress = true;

    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log("CC collided with:" + hit.gameObject.);
    }

}
